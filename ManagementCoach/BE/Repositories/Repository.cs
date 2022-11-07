using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Repositories
{
    public abstract class Repository
    {
		protected CoachManContext Context { get; } = new CoachManContext();
    }
}
