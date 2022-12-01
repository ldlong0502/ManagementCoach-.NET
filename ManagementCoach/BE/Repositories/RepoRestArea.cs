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
	public class RepoRestArea : Repository
	{
		public bool ProvinceIdExists(int provinceId) => Context.RestAreas.Any(d => d.ProvinceId == provinceId);
		public bool RestAreaExists(int id) => Context.RestAreas.Any(d => d.Id == id);

		public Result<ModelRestArea> InsertRestArea(InputRestArea input)
		{
			if (ProvinceIdExists(input.ProvinceId))
				return new Result<ModelRestArea>() { Success = false, ErrorMessage = "RestArea with this province Id already exist." };

			var restArea = Map.To<RestArea>(input);
			Context.RestAreas.Add(restArea);
			Context.SaveChanges();
			return new Result<ModelRestArea>() { Success = true, Payload = Map.To<ModelRestArea>(restArea) };
		}

		public ModelRestArea GetRestArea(int id)
		{
			return Map.To<ModelRestArea>(Context.RestAreas.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelRestArea> UpdateRestArea(int id, InputRestArea input)
		{
			if (!RestAreaExists(id))
				return new Result<ModelRestArea> { Success = false, ErrorMessage = "RestArea with this Id do not exist" };

			var restArea = Context.RestAreas.Where(c => c.Id == id).FirstOrDefault();

			if (restArea.ProvinceId != input.ProvinceId && ProvinceIdExists(input.ProvinceId))
				return new Result<ModelRestArea> { Success = false, ErrorMessage = "RestArea with this province Id already exist." };

			restArea = Map.To(input, restArea);
			Context.SaveChanges();

			return new Result<ModelRestArea> { Success = true, Payload = Map.To<ModelRestArea>(restArea) };
		}

		public Result DeleteRestArea(int id)
		{
			if (!RestAreaExists(id))
				return new Result { Success = false, ErrorMessage = "RestArea with this Id do not exist" };

			var restArea = new RestArea() { Id = id };
			Context.RestAreas.Attach(restArea);
			Context.RestAreas.Remove(restArea);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}