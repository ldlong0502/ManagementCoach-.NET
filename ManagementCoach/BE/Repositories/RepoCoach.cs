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
			if (RegNoExists(input.RegNo))
				return new Result<ModelCoach> { Success = false, ErrorMessage = "Coach with this registration already exist." };

			var coach = Map.To<Coach>(input);
			Context.Coaches.Add(coach);
			Context.SaveChanges();
			return new Result<ModelCoach> { Success = true, Payload = Map.To<ModelCoach>(coach) };
		}


		///// <summary>
		///// Lấy thông tin các xe theo tên hoặc biển số xe
		///// </summary>
		///// <param name="keyword">theo từ khóa, nếu từ khóa trống thì lấy thông tin mới nhất</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		///// <param name="page">trang muốn lấy</param>
		public Page<ModelCoach> GetCoaches(string keyword, int pageNum = 1, int limit = 20)
		{
			var page = PaginationFactory.Create<ModelCoach>(limit, pageNum, 
				() => Context.Coaches
							 .OrderByDescending(c => c.DateAdded)
							 .Where(c => c.Name.Contains(keyword) || c.RegNo.Contains(keyword))
			);

            var coachSeatRepo = new RepoCoachSeat();
            page.Items.ForEach(c => c.CoachSeats = coachSeatRepo.GetCoachSeats(c.Id));

            return page;
		}


	
		public ModelCoach GetCoach(int id) { 
			return Map.To<ModelCoach>(Context.Coaches.Where(c => c.Id == id).FirstOrDefault());
		}

		public Result<ModelCoach> UpdateCoach(int coachId, InputCoach input) {
			var coach = Context.Coaches.Where(c => c.Id == coachId).FirstOrDefault();
			
			if (coach.RegNo != input.RegNo && RegNoExists(input.RegNo))
				return new Result<ModelCoach> { Success = false, ErrorMessage = "Coach with this registration already exist." };

			coach.Name = input.Name;
			coach.RegNo = input.RegNo;
			coach.Status = input.Status;
			coach.Notes = input.Notes;
			Context.SaveChanges();
			
			return new Result<ModelCoach> { Success = true, Payload = Map.To<ModelCoach>(coach) };
		}

		public Result DeleteCoach(int coachId)
		{
			if (!CoachExists(coachId))
				return new Result { Success = false, ErrorMessage = "Coach with this Id do not exist" };

			var coach = new Coach() { Id = coachId };
			Context.Coaches.Attach(coach);
			Context.Coaches.Remove(coach);
			Context.SaveChanges();

			return new Result { Success = true };
		} 
	}
}
