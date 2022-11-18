﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelRouteRestArea
	{
		[Required]
		public int RouteId { get; set; }
		[Required]
		public int RestAreaId { get; set; }
		[Required]
		public int StopOrder { get; set; }
	}
}
