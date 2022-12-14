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


		public Page<ModelTrip> GetTrips(int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelTrip>(limit, pageNum,
				() => Context.Trips.OrderBy(c => c.Id)
			);
		}
		///// <summary>
		///// Lấy thông tin các trạm dừng chân theo driver
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelTrip> GetTripsByDriver(int driverId, int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelTrip>(limit, pageNum,
				() => Context.Trips
							 .Where(c => c.DriverId == driverId)
							 .OrderBy(c => c.Id)
			);
		}

		///// <summary>
		///// Lấy thông tin các trạm dừng chân theo coach
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelTrip> GetTripsByCoach(int coachId, int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelTrip>(limit, pageNum,
				() => Context.Trips
							 .Where(c => c.CoachId == coachId)
							 .OrderBy(c => c.Id)
			);
		}

		///// <summary>
		///// Lấy thông tin các trạm dừng chân theo route
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelTrip> GetTripsByRoute(int routeId, int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelTrip>(limit, pageNum,
				() => Context.Trips
							 .Where(c => c.RouteId == routeId)
							 .OrderBy(c => c.Id)
			);
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
