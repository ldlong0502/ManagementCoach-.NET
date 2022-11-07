using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE
{
	internal class PaginationFactory
	{
		public static Page<T> Create<T>(int limit, int page, Func<IQueryable<object>> getItems)
		{
			return new Page<T>()
			{
				Items = getItems().Skip((page - 1) * limit).Take(limit).Select(e => Map.To<T>(e)).ToList(),
				CurrentPage = page,
				PageCount = (int)Math.Ceiling(getItems().Count() / (double)limit)
			};
		}
	}
}
