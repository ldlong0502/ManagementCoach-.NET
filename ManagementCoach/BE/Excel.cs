using ManagementCoach.BE.Repositories;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace ManagementCoach.BE
{
	public class Excel
	{
		public static void ExportAs<T>(IEnumerable<T> items)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = "Excel File|*.xlsx";
			dialog.Title = "Save an Image File";
			dialog.ShowDialog();
			//var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			//var filePath = System.IO.Path.Combine(documentPath, $"{fileName}.xlsx");

			if (dialog.FileName != "")
			{
				Export(dialog.FileName, "Test", items);
			}
		}

		public static void Export<T>(string filePath, string sheetName, IEnumerable<T> items) {
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
			using (var pck = new ExcelPackage(new FileInfo(filePath)))
			{
				pck.Workbook.Properties.Author = "CMB";
				pck.Workbook.Properties.Title = $"{fileName} - CoachManContext Export";

				var sheet = pck.Workbook.Worksheets.FirstOrDefault(s => s.Name == sheetName);
				if (sheet == null)
				{
					sheet = pck.Workbook.Worksheets.Add(sheetName);
				}
				else
				{
					sheet.Cells.Clear();
				}
				var workSheet = pck.Workbook.Worksheets[sheet.Index];
				workSheet.Cells[1, 1].LoadFromCollection(items, true, TableStyles.Light21);
				pck.Save();
			}

			Open(filePath);
		}

		public static void Open(string filePath)
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
