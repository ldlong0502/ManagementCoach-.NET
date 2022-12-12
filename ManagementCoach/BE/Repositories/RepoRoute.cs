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

			var restAreas = input.RouteRestAreaIdList.Select( (restAreaId, index) => new RouteRestArea() { 
				RestAreaId = restAreaId, RouteId = route.Id, StopOrder = index + 1
			});

			Context.RouteRestArea.AddRange(restAreas);
			Context.SaveChanges();

			var payload = Map.To<ModelRoute>(route);
			payload.RestAreas = Context.RouteRestArea
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
			var route = Map.To<ModelRoute>(Context.Routes.Where(c => c.Id == id).First());
			route.RestAreas = GetRestAreasOfRoute(id);
			return route;
		}

		public List<ModelRestArea> GetRestAreasOfRoute(int routeId)
		{
			return Context.RouteRestArea
							.Where(r => r.RouteId == routeId)
							.Include(r => r.RestArea)
							.Select(r => r.RestArea)
							.ToList()
							.Select(r => Map.To<ModelRestArea>(r))
							.ToList();
		} 

		///// <summary>
		///// Lấy thông tin các tuyến đường
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelRoute> GetRoutes(int pageNum = 1, int limit = 20)
		{
			var page = PaginationFactory.Create<ModelRoute>(limit, pageNum,
				() => Context.Routes.Include(r => r.RouteRestAreas)
			) ;

			page.Items.ForEach(r => r.RestAreas = GetRestAreasOfRoute(r.Id));
			return page;
		}

		
		///// <summary>
		///// Lấy thông tin các tuyến đường theo station 
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelRoute> GetRoutesFromStation(int originStationId, int destinationStationId, int pageNum = 1, int limit = 20)
		{
			var page = PaginationFactory.Create<ModelRoute>(limit, pageNum,
				() => Context.Routes
							 .Where(r => r.OriginStationId == originStationId && r.DestinationStationId == destinationStationId).OrderBy(r => r.Id)
			);

			page.Items.ForEach(r => r.RestAreas = GetRestAreasOfRoute(r.Id));
			return page;
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

			var restAreas = input.RouteRestAreaIdList.Select((restAreaId, index) => new RouteRestArea()
			{
				RestAreaId = restAreaId,
				RouteId = route.Id,
				StopOrder = index + 1
			});
			Context.RouteRestArea.AddRange(restAreas);

			Context.SaveChanges();
			var payload = Map.To<ModelRoute>(route);
			payload.RestAreas = GetRestAreasOfRoute(id);

			return new Result<ModelRoute> { Success = true, Payload = payload };
		}

		public Result DeleteRoute(int id)
		{
			if (!RouteExists(id))
				return new Result { Success = false, ErrorMessage = "Route with this Id do not exist" };

			var rr = Context.RouteRestArea
					.Where(r => r.RouteId == id)
					.ToList();
			Context.RouteRestArea.RemoveRange(rr);
			
			var route = new Route() { Id = id };
			Context.Routes.Attach(route);
			Context.Routes.Remove(route);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
