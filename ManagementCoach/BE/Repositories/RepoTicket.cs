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
			var ticket = Map.To<Ticket>(input);
			Context.Tickets.Add(ticket);
			Context.SaveChanges();
			return new Result<ModelTicket>() { Success = true, Payload = Map.To<ModelTicket>(ticket) };
		}

		public ModelTicket GetTicket(int id)
		{
			return Map.To<ModelTicket>(Context.Tickets.Where(c => c.Id == id).FirstOrDefault());
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
