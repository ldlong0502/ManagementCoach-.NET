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
	public class RepoProvince : Repository
	{
		public bool NameExists(string name) => Context.Provinces.Any(d => d.Name == name);
		public bool ProvinceExists(int id) => Context.Provinces.Any(d => d.Id == id);

		public Result<ModelProvince> InsertProvince(string provinceName)
		{
			if (NameExists(provinceName))
				return new Result<ModelProvince>() { Success = false, ErrorMessage = "Province with this name already exist." };

			var province = new Province() { Name = provinceName };
			Context.Provinces.Add(province);
			Context.SaveChanges();
			return new Result<ModelProvince>() { Success = true, Payload = Map.To<ModelProvince>(province) };
		}

		public ModelProvince GetProvince(int id)
		{
			return Map.To<ModelProvince>(Context.Provinces.Where(c => c.Id == id).FirstOrDefault());
		}

		///// <summary>
		///// Lấy thông tin các province theo tên
		///// </summary>
		///// <param name="keyword">theo từ khóa, nếu từ khóa trống thì lấy hết</param>
		public List<ModelProvince> GetProvinces(string keyword)
		{
			return Context.Provinces.Where(p => p.Name.Contains(keyword) || p.Id.ToString().Contains(keyword))
						  .ToList().Select(p => Map.To<ModelProvince>(p))
						  .ToList();
		}

		public Result<ModelProvince> UpdateProvince(int id, string provinceName)
		{
			if (!ProvinceExists(id))
				return new Result<ModelProvince> { Success = false, ErrorMessage = "Province with this Id do not exist" };

			var province = Context.Provinces.Where(c => c.Id == id).FirstOrDefault();

			if (province.Name != provinceName && NameExists(provinceName))
				return new Result<ModelProvince> { Success = false, ErrorMessage = "Province with this name already exist." };

			province.Name = provinceName;
			Context.SaveChanges();

			return new Result<ModelProvince> { Success = true, Payload = Map.To<ModelProvince>(province) };
		}

		public Result DeleteProvince(int id)
		{
			if (!ProvinceExists(id))
				return new Result { Success = false, ErrorMessage = "Province with this Id do not exist" };

			var province = new Province() { Id = id };
			Context.Provinces.Attach(province);
			Context.Provinces.Remove(province);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
