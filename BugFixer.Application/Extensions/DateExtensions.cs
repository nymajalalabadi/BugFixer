using System.Globalization;

namespace BugFixer.Application.Extensions
{
    public static class DateExtensions
    {
        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;

            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} ثانیه پیش", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("حدود {0} دقیقه پیش", timeSpan.Minutes) :
                    "حدود 1 دقیقه پیش";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("حدود {0} ساعت پیش", timeSpan.Hours) :
                    "حدود 1 ساعت پیش";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("حدود {0} روز پیش", timeSpan.Days) :
                    "دیروز";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("حدود {0} ماه پیش", timeSpan.Days / 30) :
                    "حدود یک ماه پیش";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("حدود {0} سال پیش", timeSpan.Days / 365) :
                    "حدود یک سال پیش";
            }

            return result;
        }

        public static string ToShamsiDate(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }

        public static DateTime ToShamsiDateTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return new DateTime(pc.GetYear(value), pc.GetMonth(value), pc.GetDayOfMonth(value), 0, 0, 0);
        }

        public static DateTime ToMiladi(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, new PersianCalendar());
        }

        public static DateTime ToMiladi(this string date)
        {
            var splitedDate = date.Split("/");

            var year = Convert.ToInt32(splitedDate[0]);
            var month = Convert.ToInt32(splitedDate[1]);
            var day = Convert.ToInt32(splitedDate[2]);

            return new DateTime(year, month, day, new PersianCalendar());
        }

        public static DateTime GetDateNow()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        public static string GetHourAndMinutesFormat(this DateTime time)
        {
            return time.ToString("HH:mm");
        }

        public static string ToStringShamsiDate(this DateTime dt)
        {
            System.Globalization.PersianCalendar PC = new PersianCalendar();
            int intYear = PC.GetYear(dt);
            int intMonth = PC.GetMonth(dt);
            int intDayOfMonth = PC.GetDayOfMonth(dt);
            DayOfWeek enDayOfWeek = PC.GetDayOfWeek(dt);
            string strMonthName = "";
            string strDayName = "";
            switch (intMonth)
            {
                case 1:
                    strMonthName = "فروردین";
                    break;
                case 2:
                    strMonthName = "اردیبهشت";
                    break;
                case 3:
                    strMonthName = "خرداد";
                    break;
                case 4:
                    strMonthName = "تیر";
                    break;
                case 5:
                    strMonthName = "مرداد";
                    break;
                case 6:
                    strMonthName = "شهریور";
                    break;
                case 7:
                    strMonthName = "مهر";
                    break;
                case 8:
                    strMonthName = "آبان";
                    break;
                case 9:
                    strMonthName = "آذر";
                    break;
                case 10:
                    strMonthName = "دی";
                    break;
                case 11:
                    strMonthName = "بهمن";
                    break;
                case 12:
                    strMonthName = "اسفند";
                    break;
                default:
                    strMonthName = "";
                    break;
            }

            switch (enDayOfWeek)
            {
                case DayOfWeek.Friday:
                    strDayName = "جمعه";
                    break;
                case DayOfWeek.Monday:
                    strDayName = "دوشنبه";
                    break;
                case DayOfWeek.Saturday:
                    strDayName = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    strDayName = "یکشنبه";
                    break;
                case DayOfWeek.Thursday:
                    strDayName = "پنجشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    strDayName = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    strDayName = "چهارشنبه";
                    break;
                default:
                    strDayName = "";
                    break;
            }

            return (string.Format("{0} {1} {2} {3}", strDayName, intDayOfMonth, strMonthName, intYear));
        }

        public static string ToShortTime(this TimeSpan ts)
        {
            return ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00");
        }
    }
}
