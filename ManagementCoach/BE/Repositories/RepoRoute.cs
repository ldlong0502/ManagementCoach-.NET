using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Data;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;

namespace ManagementCoach.BE.Repositories
{
	public class RepoRoute : Repository
	{
		public bool RoutePathExists(int originSationId, int destStationId, int departTime) => Context.Routes.Any(d => d.OriginStationId == originSationId && d.DestinationStationId == destStationId && d.DepartTime ==departTime);
		public bool RouteExists(int id) => Context.Routes.Any(d => d.Id == id);

		public Result<ModelRoute> InsertRoute(InputRoute input)
		{
			if (RoutePathExists(input.DestinationStationId, input.DestinationStationId, input.DepartTime))
				return new Result<ModelRoute>() { Success = false, ErrorMessage = "Route that goes from and to these routes aleeady exist." };

			var route = Map.To<Route>(input);
			Context.Routes.Add(route);
            Context.SaveChanges();

            var restAreas = input.RouteRestAreas.Select( (restAreaId, index) => new RouteRestArea() { 
				RestAreaId = restAreaId, RouteId = route.Id, StopOrder = index + 1
			});

			Context.RouteRestArea.AddRange(restAreas);
			Context.SaveChanges();

			var payload = Map.To<ModelRoute>(route);
			payload.RouteRestAreas = Context.RouteRestArea
											.Where(r => r.RouteId == route.Id)
											.Include(r => r.RestArea)
											.Select(r => r.RestArea)
											.ToList()
											.Select(r => Map.To<ModelRestArea>(r))
											.ToList();

			return new Result<ModelRoute>() { Success = true, Payload = payload };
		}

		public ModelRoute GetRoute(int id)
		{
			return Map.To<ModelRoute>(Context.Routes.Where(c => c.Id == id).FirstOrDefault());
		}

		///// <summary>
		///// Lấy thông tin các tuyến đường
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelRoute> GetRoutes(int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelRoute>(limit, pageNum,
				() => Context.Routes.Include(r => r.RouteRestAreas).OrderBy(r => r.Id)
			); 
		}

		
		///// <summary>
		///// Lấy thông tin các tuyến đường theo station 
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelRoute> GetRoutesFromStation(int originStationId, int destinationStationId, int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelRoute>(limit, pageNum,
				() => Context.Routes
							 .Where(r => r.OriginStationId == originStationId && r.DestinationStationId == destinationStationId).OrderBy(r => r.Id)
							 .Include(r => r.RouteRestAreas)
			);
		}


		public Result<ModelRoute> UpdateRoute(int id, InputRoute input)
		{
			if (!RouteExists(id))
				return new Result<ModelRoute> { Success = false, ErrorMessage = "Route with this Id do not exist" };

			var route = Context.Routes.Where(c => c.Id == id).FirstOrDefault();

			if (
				(route.OriginStationId != input.OriginStationId
				|| route.DestinationStationId != input.DestinationStationId)
				&& RoutePathExists(input.DestinationStationId, input.DestinationStationId, input.DepartTime)
			)
				return new Result<ModelRoute>() { Success = false, ErrorMessage = "Route that goes from and to these routes aleeady exist." };


			route = Map.To(input, route);

			var routeRestAreas = Context.RouteRestArea.Where(r => r.RouteId == id).ToList();
			Context.RouteRestArea.RemoveRange(routeRestAreas);

			var restAreas = input.RouteRestAreas.Select((restAreaId, index) => new RouteRestArea()
			{
				RestAreaId = restAreaId,
				RouteId = route.Id,
				StopOrder = index + 1
			});
			Context.RouteRestArea.AddRange(restAreas);

			Context.SaveChanges();
			var payload = Map.To<ModelRoute>(route);
			payload.RouteRestAreas = Context.RouteRestArea
											.Where(r => r.RouteId == route.Id)
											.Include(r => r.RestArea)
											.Select(r => r.RestArea)
											.ToList()
											.Select(r => Map.To<ModelRestArea>(r))
											.ToList();

			return new Result<ModelRoute> { Success = true, Payload = payload };
		}

		public Result DeleteRoute(int id)
		{
			if (!RouteExists(id))
				return new Result { Success = false, ErrorMessage = "Route with this Id do not exist" };

			var route = new Route() { Id = id };
			Context.Routes.Attach(route);
			Context.Routes.Remove(route);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
