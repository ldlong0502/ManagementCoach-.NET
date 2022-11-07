using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Data
{
	public class Result<T>
	{
		/// <summary>
		/// Kết quả có thành công hay không
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Kết quả trả về, null nếu Success = false
		/// </summary>
		public T Payload { get; set; }

		/// <summary>
		/// Thông báo lỗi, là null nếu Success = true, là string nếu Success = false. Dùng string này để hiển thị lí do lỗi cho người dùng.
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}
