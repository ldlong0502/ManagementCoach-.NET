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

				config.CreateMap<InputDriver, Driver>();
				config.CreateMap<Driver, ModelDriver>();

				config.CreateMap<InputPassenger, Passenger>();
				config.CreateMap<Passenger, ModelPassenger>();

				config.CreateMap<InputRestArea, RestArea>();
				config.CreateMap<RestArea, ModelRestArea>();

				config.CreateMap<InputRoute, Route>();
				config.CreateMap<int, RouteRestArea>();
				config.CreateMap<RouteRestArea, ModelRestArea>();
				config.CreateMap<Route, ModelRoute>();

				config.CreateMap<InputStation, Station>();
				config.CreateMap<Station, ModelStation>();

				config.CreateMap<InputTicket, Ticket>();
				config.CreateMap<Ticket, ModelTicket>();

				config.CreateMap<InputTrip, Trip>();
				config.CreateMap<Trip, ModelTrip>();

				config.CreateMap<InputUser, User>();
				config.CreateMap<User, ModelUser>();

				config.CreateMap<Province, ModelProvince>();
			}));
		}

		public static T To<T>(object source) => _mapper.Map<T>(source);
		
		public static DestT To<SrcT, DestT>(SrcT source, DestT destination) => _mapper.Map<SrcT, DestT>(source, destination);
	}
}
