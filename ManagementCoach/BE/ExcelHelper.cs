using Aspose.Cells;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Repositories;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
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
		public static void ExportSingleSheetAs<T>(string sheetName, IEnumerable<T> items)
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

		public static void ImportSingleSheet(string sheetName)
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
			if (result == DialogResult.Yes)
			{
				Import(dialog.FileName, sheetName, dropCurrentData: true);
			}
			else
			{
				Import(dialog.FileName, sheetName, dropCurrentData: false);
			}
		}

		public static void Export<T>(string filePath, string sheetName, IEnumerable<T> items) {
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

		private static readonly Dictionary<string, (Type entityType, Func<ExcelWorksheet, IEnumerable<object>> handler)> 
			_typeAndHandlerOf = new Dictionary<string, (Type entityType, Func<ExcelWorksheet, IEnumerable<object>> handler)>()
		{
			{ "Trips", (typeof(Trip), worksheet => worksheet.ConvertToObjects<Trip>()) },
			{ "Coaches", (typeof(Coach),worksheet => worksheet.ConvertToObjects<Coach>()) },
			{ "Drivers", (typeof(Driver),worksheet => worksheet.ConvertToObjects<Driver>()) },
			{ "Passengers", (typeof(Passenger),worksheet => worksheet.ConvertToObjects<Passenger>()) },
			{ "Stations", (typeof(Station),worksheet => worksheet.ConvertToObjects<Station>()) },
			{ "RestAreas",(typeof(RestArea), worksheet => worksheet.ConvertToObjects<RestArea>()) },
			{ "Routes", (typeof(Route),worksheet => worksheet.ConvertToObjects<Route>()) },
		};

		public static void ImportAll(string filePath, bool dropCurrentData)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			var context = new CoachManContext();

			using (var pck = new ExcelPackage(new FileInfo(filePath)))
			{
				foreach (var worksheet in pck.Workbook.Worksheets)
				{
					if (_typeAndHandlerOf.ContainsKey(worksheet.Name))
					{
						var name = worksheet.Name;
						var (entityType, convertToEntities) = _typeAndHandlerOf[name];
						var entities = convertToEntities(worksheet);

						try
						{
							if (dropCurrentData)
							{
								context.BulkSynchronize(entityType, entities);
							}
							else
							{
								context.BulkMerge(entityType, entities);
							}
						}
						catch (TargetInvocationException ex)
						{
							if (ex.InnerException is Npgsql.PostgresException postgresEx)
							{
								var result = MessageBox.Show($"Error While importing from Sheet \"{worksheet.Name}\":\n\n{postgresEx.MessageText}\n\n{postgresEx.Detail}", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							return;
						}
					}
				}
			}
		}
		public static void Import(string filePath, string sheetName, bool dropCurrentData)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			var context = new CoachManContext();

			using (var pck = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheets = pck.Workbook.Worksheets.Where(w => w.Name == sheetName);

				if (worksheets.Count() == 0)
				{
					MessageBox.Show($"Could not find Sheet \"{sheetName}\" in file \"{filePath}\"", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}


				if (!_typeAndHandlerOf.ContainsKey(sheetName))
				{
					MessageBox.Show($"Could not indentify table to import to for Sheet of name \"{sheetName}\"", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				else 
				{
					foreach (var worksheet in worksheets) {
						var (entityType, convertToEntities) = _typeAndHandlerOf[sheetName];
						var entities = convertToEntities(worksheet);

						try
						{
							if (dropCurrentData)
							{
								context.BulkSynchronize(entityType, entities);
							}
							else
							{
								context.BulkMerge(entityType, entities);
							}
						}
						catch (TargetInvocationException ex)
						{
							if (ex.InnerException is Npgsql.PostgresException postgresEx)
							{
								var result = MessageBox.Show($"Error While importing from Sheet \"{worksheet.Name}\":\n\n{postgresEx.MessageText}\n\n{postgresEx.Detail}", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							return;
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
