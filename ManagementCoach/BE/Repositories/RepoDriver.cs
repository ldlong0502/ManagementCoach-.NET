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
	public class RepoDriver : Repository
	{
		public bool IdCardExists(string idCard)
		{
			return Context.Drivers.Any(d => d.IdCard == idCard);
		}

		public Result<ModelDriver> InsertDriver(InputDriver input)
		{
			if (IdCardExists(input.IdCard))
				return new Result<ModelDriver>() { Success = false, ErrorMessage = "Driver with this Id card already exist." };

			var driver = Map.To<Driver>(input);
			Context.Drivers.Add(driver);
			Context.SaveChanges();
			return new Result<ModelDriver>() { Success = true, Payload = Map.To<ModelDriver>(driver) };
		}

		public ModelDriver GetDriver(int id)
		{
			return Map.To<ModelDriver>(Context.Drivers.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelDriver> UpdateCoach(int coachId, InputDriver input)
		{
			var driver = Context.Drivers.Where(c => c.Id == coachId).FirstOrDefault();

			if (driver.IdCard != input.IdCard && IdCardExists(input.IdCard))
				return new Result<ModelDriver> { Success = false, ErrorMessage = "Coach with this registration already exist." };

			driver = Map.To(input, driver);
			Context.SaveChanges();

			return new Result<ModelDriver> { Success = true, Payload = Map.To<ModelDriver>(driver) };
		}

		public Result DeleteCoach(int id)
		{
			if (!DriverExists(id))
				return new Result { Success = false, ErrorMessage = "Coach with this Id do not exist" };

			var driver = new Driver() { Id = id };
			Context.Drivers.Attach(driver);
			Context.Drivers.Remove(driver);
			Context.SaveChanges();

			return new Result { Success = true };
		}

		public bool DriverExists(int id) => Context.Drivers.Any(d => d.Id == id);
	}
}
