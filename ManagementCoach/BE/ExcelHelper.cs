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
using MessageBox = System.Windows.Forms.MessageBox;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace ManagementCoach.BE
{
	public class ExcelHelper
	{
		public static void ExportSingleSheetAs<T>(string sheetName, IEnumerable<T> items)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = "Excel File|*.xlsx";
			dialog.Title = "Save an Image File";
			dialog.ShowDialog();
			//var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			//var filePath = System.IO.Path.Combine(documentPath, $"{fileName}.xlsx");

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

		public static void Import(string filePath)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			var queryContext = new CoachManContext();
			var context = new CoachManContext();

			var typeAndHandlerOf = new Dictionary<string, (Type entityType, Func<ExcelWorksheet, IEnumerable<object>> handler)>()
			{
				//{ "Trips", (typeof(Trip), worksheet => worksheet.ConvertToObjects<Trip>()) },
				//{ "Coaches", (typeof(Coach),worksheet => worksheet.ConvertToObjects<Coach>()) },
				//{ "Drivers", (typeof(Driver),worksheet => worksheet.ConvertToObjects<Driver>()) },
				//{ "Passengers", (typeof(Passenger),worksheet => worksheet.ConvertToObjects<Passenger>()) },
				//{ "Stations", (typeof(Station),worksheet => worksheet.ConvertToObjects<Station>()) },
				//{ "RestAreas",(typeof(RestArea), worksheet => worksheet.ConvertToObjects<RestArea>()) },
				//{ "Routes", (typeof(Route),worksheet => worksheet.ConvertToObjects<Route>()) },
			};

			using (var pck = new ExcelPackage(new FileInfo(filePath)))
			{
				foreach (var worksheet in pck.Workbook.Worksheets)
				{
					if (typeAndHandlerOf.ContainsKey(worksheet.Name))
					{
						var name = worksheet.Name;
						var (entityType, convertToEntities) = typeAndHandlerOf[name];
						var entities = convertToEntities(worksheet);
						var queryDbSet = queryContext.Set(entityType);
						var dbSet = context.Set(entityType);

						var entitiesToAdd = entities.Where(e =>
							{
								var a = queryDbSet.Find(entityType.GetProperty("Id").GetValue(e, null)) == null;
								return a;
							}
						).ToList();

						context.Set(entityType).AddRange(entitiesToAdd);

						var entitiesToUpdate = entities.Where(e => !entitiesToAdd.Any(addE => addE == e)).ToList();
						foreach (var e in entitiesToUpdate)
						{
							dbSet.Attach(e);
							context.Entry(e).State = EntityState.Modified;
						}
					}
				}

				context.SaveChanges();
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
