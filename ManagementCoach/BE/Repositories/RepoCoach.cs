using ManagementCoach.BE.Data;
using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagementCoach.BE.Repositories
{
	public class RepoCoach : Repository
	{
		public bool CoachExists(int id)
			=> Context.Coaches.Any(c => c.Id == id);

		public bool RegNoExists(string regNo)
			=> Context.Coaches.Any(c => c.RegNo == regNo);

		public Result<ModelCoach> InsertCoach(InputCoach input)
		{
			if (input.Capacity < 2)
			{
				return new Result<ModelCoach>
				{
					Success = false,
					ErrorMessage = "Capacity must be at least 2"
				};
			}

			if (input.Capacity % 2 == 1)
			{
				return new Result<ModelCoach> { 
					Success = false, ErrorMessage = "Capacity must be an even number"
				};
			}

			if (RegNoExists(input.RegNo))
			{
				return new Result<ModelCoach> { 
					Success = false, ErrorMessage = "Coach with this registration already exist"
				};
			}

			var coach = Map.To<Coach>(input);
			Context.Coaches.Add(coach);
			Context.SaveChanges();

			var payload = Map.To<ModelCoach>(coach);
			payload.CoachSeats = InsertCoachSeats(coach.Id, input.Capacity);

			return new Result<ModelCoach> { Success = true, Payload = payload };
		}

		private List<ModelCoachSeat> InsertCoachSeats(int coachId, int capacity)
		{
			var coachSeats = new List<CoachSeat>();
			
			for (int i = 1; i <= capacity / 2; i++)
			{
				coachSeats.Add(new CoachSeat() { Name = $"A{i}", CoachId = coachId });
				coachSeats.Add(new CoachSeat() { Name = $"B{i}", CoachId = coachId });
			}

			Context.CoachSeats.AddRange(coachSeats);
			Context.SaveChanges();

			return coachSeats.Select(cs => Map.To<ModelCoachSeat>(cs)).ToList();
		}

		///// <summary>
		///// Lấy thông tin các xe theo tên hoặc biển số xe
		///// </summary>
		///// <param name="keyword">theo từ khóa, nếu từ khóa trống thì lấy thông tin mới nhất</param>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelCoach> GetCoaches(string keyword, int pageNum = 1, int limit = 20)
		{
			var page = PaginationFactory.Create<ModelCoach>(limit, pageNum, 
				() => Context.Coaches
							 .Where(c => c.Name.Contains(keyword) || c.RegNo.Contains(keyword))
							 .OrderByDescending(c => c.DateAdded)
			);

            var coachSeatRepo = new RepoCoachSeat();
            page.Items.ForEach(c => c.CoachSeats = coachSeatRepo.GetCoachSeats(c.Id));

            return page;
		}
	
		public ModelCoach GetCoach(int id) { 
			return Map.To<ModelCoach>(Context.Coaches.Where(c => c.Id == id).FirstOrDefault());
		}
		public ModelCoach GetCoachByRegNo(string regNo)
		{
			return Map.To<ModelCoach>(Context.Coaches.Where(c => c.RegNo == regNo).FirstOrDefault());
		}

		public Result<ModelCoach> UpdateCoach(int coachId, InputCoach input) {
			if (input.Capacity < 2)
			{
				return new Result<ModelCoach>
				{
					Success = false,
					ErrorMessage = "Capacity must be at least 2"
				};
			}

			if (input.Capacity % 2 == 1)
			{
				return new Result<ModelCoach>
				{
					Success = false, ErrorMessage = "Capacity must be an even number"
				};
			}

			var coach = Context.Coaches.Where(c => c.Id == coachId).FirstOrDefault();
			
			if (coach.RegNo != input.RegNo && RegNoExists(input.RegNo))
				return new Result<ModelCoach> { Success = false, ErrorMessage = "Coach with this registration already exist." };

			coach.Name = input.Name;
			coach.RegNo = input.RegNo;
			coach.Status = input.Status;
			coach.Notes = input.Notes;
			Context.SaveChanges();

			var payload = Map.To<ModelCoach>(coach);

			var oldCoachSeats = Context.CoachSeats.Where(cs => cs.Id == coach.Id);
			Context.CoachSeats.RemoveRange(oldCoachSeats);
			Context.SaveChanges();
			payload.CoachSeats = InsertCoachSeats(coach.Id, input.Capacity);

			return new Result<ModelCoach> { Success = true, Payload = payload };
		}

		public Result DeleteCoach(int coachId)
		{
			if (!CoachExists(coachId))
				return new Result { Success = false, ErrorMessage = "Coach with this Id do not exist" };

			var coach = new Coach() { Id = coachId };
			Context.Coaches.Attach(coach);
			Context.Coaches.Remove(coach);

			var coachSeats = Context.CoachSeats.Where(cs => cs.CoachId == coachId);
			Context.CoachSeats.RemoveRange(coachSeats);
			
			Context.SaveChanges();

			return new Result { Success = true };
		} 
	}
}
