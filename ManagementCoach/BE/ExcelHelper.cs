using Aspose.Cells;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Repositories;
using Microsoft.Win32;
using Npgsql;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace ManagementCoach.BE
{
	public class ExcelHelper
	{
		public static void ExportSingleSheetAs<TEntity>(string sheetName, IEnumerable<TEntity> items) 
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = "Excel File|*.xlsx";
			dialog.Title = "Save an Excel File";
			dialog.ShowDialog();
	

			if (dialog.FileName != "")
			{
				var fileInfo = new FileInfo(dialog.FileName);
				if (fileInfo.Exists) { 
					//check if file is in use
					try
					{
						using (Stream stream = new FileStream(dialog.FileName, FileMode.Open))
						{
							//do nothing
						}
					}
					catch
					{
						MessageBox.Show($"The file \"{dialog.FileName}\" is being use by another process.\n\nPlease close the file before exporting.", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}

				Export(dialog.FileName, sheetName, items);

				var result = MessageBox.Show($"Exported to \"{dialog.FileName}\".\n\nDo you want to open the file now?", "Export Sucessfully", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
				if (result == DialogResult.Yes)
				{
					Open(dialog.FileName);
				}
			}
		}

		public static void ImportFromFile<TEntity>(string sheetName = null) where TEntity : class, new()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Excel File|*.xlsx";
			dialog.Title = "Open an Excel File";
			dialog.ShowDialog();

			if (dialog.FileName == "")
			{
				return;
			}

			var result = MessageBox.Show($"Do you want to delete all of the current data before importing?", "Export Sucessfully", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
			try
			{
				if (result == DialogResult.Yes)
				{
					Import<TEntity>(dialog.FileName, sheetName, dropCurrentData: true);
				}
				else
				{
					Import<TEntity>(dialog.FileName, sheetName, dropCurrentData: false);
				}
			}
			catch 
			{
				return;
			}

			MessageBox.Show($"Import Completed.", "Import Sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		public static void Export<TEntity>(string filePath, string sheetName, IEnumerable<TEntity> items) {
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
			var fileInfo = new FileInfo(filePath);
			using (var pck = new ExcelPackage(fileInfo))
			{
				if (!fileInfo.Exists)
				{
					pck.Workbook.Properties.Author = "CMB";
					pck.Workbook.Properties.Title = $"{fileName} - CoachManContext Export";
				}

				var worksheet = pck.Workbook.Worksheets.FirstOrDefault(s => s.Name == sheetName);
				if (worksheet == null)
				{
					worksheet = pck.Workbook.Worksheets.Add(sheetName);
				}
				else
				{
					int index = worksheet.Index;
					pck.Workbook.Worksheets.Delete(index); 
					worksheet = pck.Workbook.Worksheets.Add(sheetName);
					if (pck.Workbook.Worksheets.Count > 1)
					{
						pck.Workbook.Worksheets.MoveBefore(worksheet.Index, index);
					}
				}
				worksheet.Cells[1, 1].LoadFromCollection(items, true, TableStyles.Light21);
				worksheet.Cells.AutoFitColumns();
				worksheet.Select();
				pck.Save();
			}
		}

		public static void Import<TEntity>(string filePath, string sheetName = null, bool dropCurrentData = false) where TEntity : class, new()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			var context = new CoachManContext();

			using (var pck = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheets = pck.Workbook.Worksheets.ToList();
				if (!string.IsNullOrWhiteSpace(sheetName))
				{
					worksheets = worksheets.Where(w => w.Name == sheetName).ToList();
				}

				if (worksheets.Count() == 0)
				{
					MessageBox.Show($"Could not find Sheet \"{sheetName}\" in file \"{filePath}\"", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				else 
				{
					foreach (var worksheet in worksheets) {
						var entities = worksheet.ConvertToObjects<TEntity>();

						context.Set<TEntity>().AttachRange(entities);
						try
						{
							if (dropCurrentData)
							{
								context.BulkSynchronize(entities, o => o.SynchronizeKeepidentity = true);
							}
							else
							{
								context.BulkMerge(entities, o => o.MergeKeepIdentity = true);
							}
						}
						catch (Exception ex)
						{
							if (ex is TargetInvocationException tex && tex.InnerException is Npgsql.PostgresException postgresEx)
							{
								var result = MessageBox.Show($"Error While importing from Sheet \"{worksheet.Name}\":\n\n{postgresEx.MessageText}\n\n{postgresEx.Detail}", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							else if (ex is PostgresException postEx)
							{
								var result = MessageBox.Show($"{postEx.MessageText}\n\n{postEx.Detail}", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							else
							{
								var result = MessageBox.Show(ex.Message, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							throw ex;
						}
					}
				}
			}
		}

		private static void Open(string filePath)
		{
			using (Process fileopener = new Process())
			{
				fileopener.StartInfo.FileName = "explorer";
				fileopener.StartInfo.Arguments = "\"" + filePath + "\"";
				fileopener.Start();
			}
		}
	}
}
