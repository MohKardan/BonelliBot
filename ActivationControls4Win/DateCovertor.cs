using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace a.Utilities
{
    public static class DateCovertor
    {
        public static string ToShamsi(this DateTime time)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(time) + "/" + persianCalendar.GetMonth(time).ToString("00")
                   + "/" + persianCalendar.GetDayOfMonth(time).ToString("00");
        }

        public static string PersionDayOfWeek(this DateTime date)
    {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Saturday:
                return "شنبه";
            case DayOfWeek.Sunday:
                return "یکشنبه";
            case DayOfWeek.Monday:
                return "دوشنبه";
            case DayOfWeek.Tuesday:
                return "سه شنبه";
            case DayOfWeek.Wednesday:
                return "چهارشنبه";
            case DayOfWeek.Thursday:
                return "پنجشنبه";
            case DayOfWeek.Friday:
                return "جمعه";
            default:
                throw new Exception();
        }
    }

    }
}