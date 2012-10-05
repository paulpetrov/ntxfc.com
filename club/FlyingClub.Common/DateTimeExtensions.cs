using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Common
{
    public static class DateTimeExtensions
    {
        public static List<DateTime> GetListFromRange(this List<DateTime> value, DateTime from, DateTime to, int years, int months, int days)
        {
            return GetListFromRange(value, from, to, years, months, days, 0, 0, 0);
        }

        public static List<DateTime> GetListFromRange(this List<DateTime> value, DateTime from, DateTime to, int years, int months, int days, int hours, int minutes, int seconds)
        {
            List<DateTime> list = new List<DateTime>();

            for (DateTime dt = from; dt < to; dt = dt.AddYears(years).AddMonths(months).AddDays(days).AddHours(hours).AddMinutes(minutes).AddSeconds(seconds))
            {
                list.Add(dt);
            }

            return list;
        }

        public static List<DateTime> GetListFromRange(this List<DateTime> value, DateTime from, DateTime to, TimeSpan increment)
        {
            List<DateTime> list = new List<DateTime>();

            for (DateTime dt = from; dt <= to; dt += increment)
            {
                list.Add(dt);
            }

            return list;
        }
    }
}
