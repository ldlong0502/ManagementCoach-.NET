using ManagementCoach.BE.Data;
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
		public bool StationExists(int id) => Context.Stations.Any(d => d.Id == id);

		public Result<ModelStation> InsertStation(InputStation input)
		{
			if (Context.Stations.Any(s => s.ProvinceId == input.ProvinceId))
				return new Result<ModelStation>() { Success = false, ErrorMessage = "Station with this province already exist." };

			var station = Map.To<Station>(input);
			Context.Stations.Add(station);
			Context.SaveChanges();
			return new Result<ModelStation>() { Success = true, Payload = Map.To<ModelStation>(station) };
		}

		public ModelStation GetStation(int id)
		{
			return Map.To<ModelStation>(Context.Stations.Where(c => c.Id == id).FirstOrDefault());
		}

		///// <summary>
		///// Lấy thông tin các station theo tên hoặc id
		///// </summary>
		///// <param name="keyword">theo từ khóa, nếu từ khóa trống thì lấy thông tin mới nhất</param>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelStation> GetStations(string keyword, int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelStation>(limit, pageNum,
				() => Context.Stations
							 .Where(c => c.Name.Contains(keyword) || c.Id.ToString().Contains(keyword))
							.OrderBy(c => c.Id)
			);
		}

		public Result<ModelStation> UpdateStation(int id, InputStation input)
		{
			if (!StationExists(id))
				return new Result<ModelStation> { Success = false, ErrorMessage = "Station with this Id do not exist" };

			var station = Context.Stations.Where(c => c.Id == id).FirstOrDefault();

			if (station.ProvinceId != input.ProvinceId && Context.Stations.Any(s => s.ProvinceId == input.ProvinceId))
					return new Result<ModelStation>() { Success = false, ErrorMessage = "Station with this province already exist." };

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
