using ManagementCoach.BE.Models;
using ManagementCoach.Views.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach
{
    public class CurrentUser
    {
        public static ModelUser currentUser = new ModelUser();

        public static DateTime GetDateNow()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day);
        }

        public static DashBoard dashBoard = new DashBoard();
        public static int invoiceId = 1;
    }
}
