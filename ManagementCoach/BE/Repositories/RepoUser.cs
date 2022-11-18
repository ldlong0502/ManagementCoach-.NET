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
	public class RepoUser : Repository
	{
		public bool UsernameExists(string username) => Context.Users.Any(d => d.Username == username);
		public bool EmailExists(string email) => Context.Users.Any(d => d.Email == email);
		public bool UserExists(int id) => Context.Users.Any(d => d.Id == id);

		public Result<ModelUser> InsertUser(InputUser input)
		{
			if (UsernameExists(input.Username))
				return new Result<ModelUser>() { Success = false, ErrorMessage = "User with this username already exist." };

			if (EmailExists(input.Email))
				return new Result<ModelUser> { Success = false, ErrorMessage = "User with this email already exist." };

			var user = Map.To<User>(input);
			Context.Users.Add(user);
			Context.SaveChanges();
			return new Result<ModelUser>() { Success = true, Payload = Map.To<ModelUser>(user) };
		}

		public ModelUser GetUser(int id)
		{
			return Map.To<ModelUser>(Context.Users.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelUser> UpdateUser(int coachId, InputUser input)
		{
			var user = Context.Users.Where(c => c.Id == coachId).FirstOrDefault();

			if (user.Username != input.Username && UsernameExists(input.Username))
				return new Result<ModelUser> { Success = false, ErrorMessage = "User with this username already exist." };

			if (user.Email != input.Email && EmailExists(input.Email))
				return new Result<ModelUser> { Success = false, ErrorMessage = "User with this email already exist." };

			user = Map.To(input, user);
			Context.SaveChanges();

			return new Result<ModelUser> { Success = true, Payload = Map.To<ModelUser>(user) };
		}

		public Result DeleteUser(int id)
		{
			if (!UserExists(id))
				return new Result { Success = false, ErrorMessage = "User with this Id do not exist" };

			var user = new User() { Id = id };
			Context.Users.Attach(user);
			Context.Users.Remove(user);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
