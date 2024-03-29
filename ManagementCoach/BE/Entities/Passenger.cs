﻿using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class Passenger : Models.ModelPassenger
	{
		[EpplusIgnore]
		public List<Ticket> Tickets { get; set; }
	}
}
