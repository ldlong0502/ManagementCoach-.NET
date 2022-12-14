using Aspose.Cells.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE
{
	/// <summary>
	/// Modified from https://stackoverflow.com/a/37746915/12957195
	/// </summary>
	public static class EPPLusExtensions
	{
		public static IEnumerable<T> ConvertToObjects<T>(this ExcelWorksheet worksheet) where T : new()
		{
			var props = typeof(T).GetProperties().ToList();

			var rowCount = worksheet.Dimension.End.Row;
			var colCount = worksheet.Dimension.End.Column;

			var objects = new List<T>();
			for (int r = 2; r <= rowCount; r++)
			{
				var obj = new T();

				for (int c = 1; c <= colCount; c++)
				{
					var colName = worksheet.Cells[1, c].GetValue<string>();
					var prop = props.Where(p => p.Name == colName).First();

					
					//This is the real wrinkle to using reflection - Excel stores all numbers as double including int
					//If it is numeric it is a double since that is how excel stores all numbers
					var cell = worksheet.Cells[r, c];
					if (cell.Value == null)
					{
						prop.SetValue(obj, null);
					}
					else if (prop.PropertyType == typeof(Int32))
					{
						prop.SetValue(obj, cell.GetValue<int>());
					}
					else if(prop.PropertyType == typeof(double))
					{
						prop.SetValue(obj, cell.GetValue<double>());
					}
					else if(prop.PropertyType == typeof(DateTime))
					{
						prop.SetValue(obj, cell.GetValue<DateTime>());
					}
					else if (prop.PropertyType == typeof(DateTimeOffset))
					{
						prop.SetValue(obj, DateTimeOffset.Parse(cell.GetValue<string>()));
					}
					else if (prop.PropertyType == typeof(bool))
					{
						prop.SetValue(obj, cell.GetValue<bool>());
					}
					else
					{
						prop.SetValue(obj, cell.GetValue<string>());
					}
				}

				objects.Add(obj);
			}

			return objects;
		}
	}
}
