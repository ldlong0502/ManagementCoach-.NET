﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelRoute
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public int OriginStationId { get; set; }
		[Required]
		public int DestinationStationId { get; set; }
		[Required]
		public int Price { get; set; }

		/// <summary>
		/// Đơn vị là phút
		/// </summary>
		public int EstimatedTime { get; set; }


		/// <summary>
		/// Các trạm dừng chân mà tuyến đường này đi qua, thứ tự dừng của trạm sẽ là thứ tự phần từ trong danh sách + 1
		/// </summary>
		public List<ModelRestArea> RouteRestAreas { get; set; }
	}
}
