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
	public class RepoPassenger : Repository
	{
		public bool IdCardExists(string idCard) => Context.Passengers.Any(d => d.IdCard == idCard);
		public bool EmailExists(string email) => Context.Passengers.Any(d => d.Email == email);
		public bool PhoneExists(string phone) => Context.Passengers.Any(d => d.Phone == phone);
		public bool PassengerExists(int id) => Context.Passengers.Any(d => d.Id == id);

		public Result<ModelPassenger> InsertPassenger(InputPassenger input)
		{
			if (IdCardExists(input.IdCard))
				return new Result<ModelPassenger>() { Success = false, ErrorMessage = "Passenger with this Id card already exist." };
			
			if (EmailExists(input.Email))
				return new Result<ModelPassenger> { Success = false, ErrorMessage = "Passenger with this email already exist." };

			if (PhoneExists(input.Phone))
				return new Result<ModelPassenger> { Success = false, ErrorMessage = "Passenger with this phone already exist." };

			var passenger = Map.To<Passenger>(input);
			Context.Passengers.Add(passenger);
			Context.SaveChanges();
			return new Result<ModelPassenger>() { Success = true, Payload = Map.To<ModelPassenger>(passenger) };
		}

		public ModelPassenger GetPassenger(int id)
		{
			return Map.To<ModelPassenger>(Context.Passengers.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelPassenger> UpdatePassenger(int id, InputPassenger input)
		{
			if (!PassengerExists(id))
				return new Result<ModelPassenger> { Success = false, ErrorMessage = "Passenger with this Id do not exist" };

			var passenger = Context.Passengers.Where(c => c.Id == id).FirstOrDefault();

			if (passenger.IdCard != input.IdCard && IdCardExists(input.IdCard))
				return new Result<ModelPassenger> { Success = false, ErrorMessage = "Passenger with this registration already exist." };

			if (passenger.Email != input.Email && EmailExists(input.Email))
				return new Result<ModelPassenger> { Success = false, ErrorMessage = "Passenger with this email already exist." };

			if (passenger.Phone != input.Phone && PhoneExists(input.Phone))
				return new Result<ModelPassenger> { Success = false, ErrorMessage = "Passenger with this phone already exist." };

			passenger = Map.To(input, passenger);
			Context.SaveChanges();

			return new Result<ModelPassenger> { Success = true, Payload = Map.To<ModelPassenger>(passenger) };
		}

		public Result DeletePassenger(int id)
		{
			if (!PassengerExists(id))
				return new Result { Success = false, ErrorMessage = "Province with this Id do not exist" };

			var passenger = new Passenger() { Id = id };
			Context.Passengers.Attach(passenger);
			Context.Passengers.Remove(passenger);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
