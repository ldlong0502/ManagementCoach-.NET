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
    public class RepoCoachSeat : Repository
    {
        public bool CoachSeatExists(int id)
            => Context.CoachSeats.Any(c => c.Id == id);

        public bool NameExists(string name)
            => Context.CoachSeats.Any(c => c.Name == name);

        public Result<ModelCoachSeat> InsertCoachSeat(InputCoachSeat input)
        {
            if (Context.CoachSeats.Any(c => c.Id == input.CoachId && c.Name == input.Name))
                return new Result<ModelCoachSeat> { Success = false, ErrorMessage = "Coach seat with this name already exist." };

            var coachSeat = Map.To<CoachSeat>(input);
            Context.CoachSeats.Add(coachSeat);
            Context.SaveChanges();
            return new Result<ModelCoachSeat> { Success = true, Payload = Map.To<ModelCoachSeat>(coachSeat) };
        }

		public ModelCoachSeat GetCoachSeat(int seatId)
		{
			return Map.To<ModelCoachSeat>(Context.CoachSeats.Where(c => c.Id == seatId).FirstOrDefault());
		}

		public List<ModelCoachSeat> GetCoachSeats(int coachId)
        {
            return Context.CoachSeats
						  .Where(c => c.CoachId == coachId)
						  .ToList()
					      .Select(cs => Map.To<ModelCoachSeat>(cs))
						  .ToList();
        }

		public Result<ModelCoachSeat> UpdateCoachSeat(int id, string seatName)
		{
			if (!CoachSeatExists(id))
				return new Result<ModelCoachSeat> { Success = false, ErrorMessage = "CoachSeat with this Id do not exist" };

			var coachSeat = Context.CoachSeats.Where(c => c.Id == id).FirstOrDefault();

			if (coachSeat.Name != seatName && NameExists(seatName))
				return new Result<ModelCoachSeat> { Success = false, ErrorMessage = "CoachSeat with this name already exist." };

			coachSeat.Name = seatName;
			Context.SaveChanges();

			return new Result<ModelCoachSeat> { Success = true, Payload = Map.To<ModelCoachSeat>(coachSeat) };
		}

		public Result DeleteCoachSeat(int id)
		{
			if (!CoachSeatExists(id))
				return new Result { Success = false, ErrorMessage = "CoachSeat with this Id do not exist" };

			var coachSeat = new CoachSeat() { Id = id };
			Context.CoachSeats.Attach(coachSeat);
			Context.CoachSeats.Remove(coachSeat);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
