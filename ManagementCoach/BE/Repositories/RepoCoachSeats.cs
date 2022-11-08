using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Repositories
{
    public class RepoCoachSeats : Repository
    {
        public bool CoachSeatExists(int id)
            => Context.CoachSeats.Any(c => c.Id == id);

        public bool CoachSeatNameExists(string regNo)
            => Context.CoachSeats.Any(c => c.RegNo == regNo);

        public Result<ModelCoachSeat> InsertCoachSeat(InputCoachSeat input)
        {
            throw new NotImplementedException();
            if (CoachSeatNameExists(input.name))
                return new Result<ModelCoachSeat> { Success = false, ErrorMessage = "Coach seat with this name already exist." };

            var coachSeat = Map.To<CoachSeat>(input);
            Context.CoachSeats.Add(coachSeat);
            Context.SaveChanges();
            return new Result<ModelCoachSeat> { Success = true, Payload = Map.To<ModelCoachSeat>(coachSeat) };
        }

        public ModelCoach GetCoachSeats(int coachId)
        {
            return Context.CoachSeats.Where(c => c.CoachId == coachId).FirstOfDefault();
        }
    }
}
