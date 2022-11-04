﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class Province : Models.ModelProvince
	{
		public RestArea RestArea { get; set; }
		public List<Station> Stations { get; set; }
	}
}