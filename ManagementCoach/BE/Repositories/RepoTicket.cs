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
	public class RepoTicket : Repository
	{
		public bool TicketExists(int id) => Context.Tickets.Any(d => d.Id == id);

		public Result<ModelTicket> InsertTicket(InputTicket input)
		{

			if (!new RepoPassenger().PassengerExists(input.PassengerId))
			{
				return new Result<ModelTicket>() { Success = false, ErrorMessage = "Passenger of this Id does not exists" };
			}

			if (!new RepoTrip().TripExists(input.TripId))
			{
				return new Result<ModelTicket>() { Success = false, ErrorMessage = "Trip of this Id does not exists" };
			}

			var coachSeat = Context.CoachSeats.Where(cs => cs.Id == input.CoachSeatId).FirstOrDefault();
			if (coachSeat == null)
			{
				return new Result<ModelTicket>() { Success = false, ErrorMessage = "Coach seat of this Id does not exists" };
			}

			if (Context.Tickets.Any(t => t.TripId == input.TripId && t.CoachSeatId == input.CoachSeatId))
			{
				return new Result<ModelTicket>() { Success = false, ErrorMessage = "This coach seat already taken" };
			}
				
			var ticket = Map.To<Ticket>(input);
			Context.Tickets.Add(ticket);
			Context.SaveChanges();

			return new Result<ModelTicket>() { Success = true, Payload = Map.To<ModelTicket>(ticket) };
		}

		public ModelTicket GetTicket(int id)
		{
			return Map.To<ModelTicket>(Context.Tickets.Where(c => c.Id == id).FirstOrDefault());
		}

		///// <summary>
		///// Lấy thông tin các ticket theo khoảng thời gian
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelTicket> GetTickets(DateTimeOffset dateFrom, DateTimeOffset dateTo, int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelTicket>(limit, pageNum,
				() => Context.Tickets
							 .Where(c => c.DateBought >= dateFrom && c.DateBought <= dateTo)
							 .OrderByDescending(t => t.DateBought)
			);
		}

		///// <summary>
		///// Lấy thông tin các ticket từ ngày nào đó về sau
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelTicket> GetTicketsFromThisOnward(DateTimeOffset dateFrom, int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelTicket>(limit, pageNum,
				() => Context.Tickets
							 .Where(c => c.DateBought >= dateFrom)
							 .OrderByDescending(t => t.DateBought)
			);
		}

		///// <summary>
		///// Lấy thông tin các ticket mới nhất
		///// </summary>
		///// <param name="pageNum">trang muốn lấy</param>
		///// <param name="limit">số lượng kết quả trên một trang</param>
		public Page<ModelTicket> GetTickets(int pageNum = 1, int limit = 20)
		{
			return PaginationFactory.Create<ModelTicket>(limit, pageNum,
				() => Context.Tickets.OrderByDescending(t => t.DateBought)
			);
		}

		public Result<ModelTicket> UpdateTicket(int id, InputTicket input)
		{
			if (!TicketExists(id))
				return new Result<ModelTicket> { Success = false, ErrorMessage = "Ticket with this Id do not exist" };

			var ticket = Context.Tickets.Where(c => c.Id == id).FirstOrDefault();

			ticket = Map.To(input, ticket);
			Context.SaveChanges();

			return new Result<ModelTicket> { Success = true, Payload = Map.To<ModelTicket>(ticket) };
		}

		public Result DeleteTicket(int id)
		{
			if (!TicketExists(id))
				return new Result { Success = false, ErrorMessage = "Ticket with this Id do not exist" };

			var ticket = new Ticket() { Id = id };
			Context.Tickets.Attach(ticket);
			Context.Tickets.Remove(ticket);
			Context.SaveChanges();

			return new Result { Success = true };
		}
	}
}
