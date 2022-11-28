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
		public bool IdCardExists(string idCard) => Context.Drivers.Any(d => d.IdCard == idCard);
		public bool EmailExists(string email) => Context.Drivers.Any(d => d.Email == email);
		public bool PhoneExists(string phone) => Context.Drivers.Any(d => d.Phone == phone);
		public bool DriverExists(int id) => Context.Drivers.Any(d => d.Id == id);

		public Result<ModelDriver> InsertDriver(InputDriver input)
		{
			if (IdCardExists(input.IdCard))
				return new Result<ModelDriver>() { Success = false, ErrorMessage = "Driver with this Id card already exist." };

			if (EmailExists(input.Email))
				return new Result<ModelDriver> { Success = false, ErrorMessage = "Driver with this email already exist." };

			if (PhoneExists(input.Phone))
				return new Result<ModelDriver> { Success = false, ErrorMessage = "Driver with this phone already exist." };

			var driver = Map.To<Driver>(input);
			Context.Drivers.Add(driver);
			Context.SaveChanges();
			return new Result<ModelDriver>() { Success = true, Payload = Map.To<ModelDriver>(driver) };
		}

		public ModelDriver GetDriver(int id)
		{
			return Map.To<ModelDriver>(Context.Drivers.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelDriver> UpdateDriver(int id, InputDriver input)
		{
			if (!DriverExists(id))
				return new Result<ModelDriver> { Success = false, ErrorMessage = "Driver with this Id do not exist" };
			
			var driver = Context.Drivers.Where(c => c.Id == id).FirstOrDefault();

			if (driver.IdCard != input.IdCard && IdCardExists(input.IdCard))
				return new Result<ModelDriver> { Success = false, ErrorMessage = "Driver with this Id card already exist." };

			if (driver.Email != input.Email && EmailExists(input.Email))
				return new Result<ModelDriver> { Success = false, ErrorMessage = "Driver with this email already exist." };

			if (driver.Phone != input.Phone && PhoneExists(input.Phone))
				return new Result<ModelDriver> { Success = false, ErrorMessage = "Driver with this phone already exist." };

			driver = Map.To(input, driver);
			Context.SaveChanges();

			return new Result<ModelDriver> { Success = true, Payload = Map.To<ModelDriver>(driver) };
		}

		public Result DeleteDriver(int id)
		{
			if (!DriverExists(id))
				return new Result { Success = false, ErrorMessage = "Driver with this Id do not exist" };

			var driver = new Driver() { Id = id };
			Context.Drivers.Attach(driver);
			Context.Drivers.Remove(driver);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
