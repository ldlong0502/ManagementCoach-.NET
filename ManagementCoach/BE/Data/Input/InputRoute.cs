using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Data.Input
{
	public class InputRoute
	{
		public int OriginStationId { get; set; }
		public int DestinationStationId { get; set; }
		public int Price { get; set; }

		/// <summary>
		/// Đơn vị là phút
		/// </summary>
		public int EstimatedTime { get; set; }

		/// <summary>
		/// Id các trạm dừng chân mà tuyến đường này đi qua, thứ tự dừng của trạm sẽ là thứ tự phần từ trong danh sách + 1
		/// </summary>
		public List<int> RouteRestAreas { get; set; }
	}
}
