using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Data;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Repositories
{
	public class RepoTrip : Repository
	{
		public bool TripExists(int id) => Context.Trips.Any(d => d.Id == id);

		public Result<ModelTrip> InsertTrip(InputTrip input)
		{
			var trip = Map.To<Trip>(input);
			Context.Trips.Add(trip);
			Context.SaveChanges();
			return new Result<ModelTrip>() { Success = true, Payload = Map.To<ModelTrip>(trip) };
		}

		public ModelTrip GetTrip(int id)
		{
			return Map.To<ModelTrip>(Context.Trips.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelTrip> UpdateTrip(int id, InputTrip input)
		{
			if (!TripExists(id))
				return new Result<ModelTrip> { Success = false, ErrorMessage = "Trip with this Id do not exist" };

			var trip = Context.Trips.Where(c => c.Id == id).FirstOrDefault();

			trip = Map.To(input, trip);
			Context.SaveChanges();

			return new Result<ModelTrip> { Success = true, Payload = Map.To<ModelTrip>(trip) };
		}

		public Result DeleteTrip(int id)
		{
			if (!TripExists(id))
				return new Result { Success = false, ErrorMessage = "Trip with this Id do not exist" };

			var trip = new Trip() { Id = id };
			Context.Trips.Attach(trip);
			Context.Trips.Remove(trip);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
