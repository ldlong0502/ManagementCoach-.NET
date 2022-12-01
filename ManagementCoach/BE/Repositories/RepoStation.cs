﻿using ManagementCoach.BE.Data;
using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Repositories
{
	public class RepoStation : Repository
	{
		public bool DistrictExists(string district) => Context.Stations.Any(d => d.District == district);
		public bool StationExists(int id) => Context.Stations.Any(d => d.Id == id);

		public Result<ModelStation> InsertStation(InputStation input)
		{
			if (DistrictExists(input.District))
				return new Result<ModelStation>() { Success = false, ErrorMessage = "Station with this district already exist." };

			var station = Map.To<Station>(input);
			Context.Stations.Add(station);
			Context.SaveChanges();
			return new Result<ModelStation>() { Success = true, Payload = Map.To<ModelStation>(station) };
		}

		public ModelStation GetStation(int id)
		{
			return Map.To<ModelStation>(Context.Stations.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelStation> UpdateStation(int id, InputStation input)
		{
			if (!StationExists(id))
				return new Result<ModelStation> { Success = false, ErrorMessage = "Station with this Id do not exist" };

			var station = Context.Stations.Where(c => c.Id == id).FirstOrDefault();

			if (station.District != input.District && DistrictExists(input.District))
				return new Result<ModelStation> { Success = false, ErrorMessage = "Station with this district already exist." };

			station = Map.To(input, station);
			Context.SaveChanges();

			return new Result<ModelStation> { Success = true, Payload = Map.To<ModelStation>(station) };
		}

		public Result DeleteStation(int id)
		{
			if (!StationExists(id))
				return new Result { Success = false, ErrorMessage = "Station with this Id do not exist" };

			var station = new Station() { Id = id };
			Context.Stations.Attach(station);
			Context.Stations.Remove(station);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}