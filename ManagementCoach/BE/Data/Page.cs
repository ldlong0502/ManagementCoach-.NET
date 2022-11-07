
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE
{
	public class Page<T>
	{
		/// <summary>
		/// Danh sách các kết quả của trang hiện tại
		/// </summary>
		public List<T> Items { get; set; }

		/// <summary>
		/// Trang hiện tại
		/// </summary>
		public int CurrentPage { get; set; }

		/// <summary>
		/// Tổng số lượng trang chứa kết quả
		/// </summary>
		public int PageCount { get; set; }
	}
}
