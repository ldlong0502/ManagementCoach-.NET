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
    public class RepoCoachSeat : Repository
    {
        public bool CoachSeatExists(int id)
            => Context.CoachSeats.Any(c => c.Id == id);

        public bool CoachSeatNameExists(string name)
            => Context.CoachSeats.Any(c => c.Name == name);

        public Result<ModelCoachSeat> InsertCoachSeat(InputCoachSeat input)
        {
            throw new NotImplementedException();
            if (CoachSeatNameExists(input.Name))
                return new Result<ModelCoachSeat> { Success = false, ErrorMessage = "Coach seat with this name already exist." };

            var coachSeat = Map.To<CoachSeat>(input);
            Context.CoachSeats.Add(coachSeat);
            Context.SaveChanges();
            return new Result<ModelCoachSeat> { Success = true, Payload = Map.To<ModelCoachSeat>(coachSeat) };
        }

        public List<ModelCoachSeat> GetCoachSeats(int coachId)
        {
            return Context.CoachSeats
						  .Where(c => c.CoachId == coachId)
					      .Select(cs => Map.To<ModelCoachSeat>(cs))
						  .ToList();
        }

    }
}