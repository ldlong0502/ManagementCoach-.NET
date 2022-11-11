using AutoMapper;
using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE
{
	public class Map
	{
		private static Mapper _mapper;

		static Map()
		{
			_mapper = new Mapper(new MapperConfiguration(config =>
			{
				config.CreateMap<InputCoach, Coach>();
				config.CreateMap<Coach, ModelCoach>();
				config.CreateMap<InputCoachSeat, CoachSeat>();
				config.CreateMap<CoachSeat, ModelCoachSeat>();
			}));
		}

		public static T To<T>(object source) => _mapper.Map<T>(source);
	}
}
