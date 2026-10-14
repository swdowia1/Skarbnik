using System.Globalization;

namespace Skarbnik.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToWarsawTime(this DateTime utcDateTime)
        {
            TimeZoneInfo warsawTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    OperatingSystem.IsWindows()
                        ? "Central European Standard Time"
                        : "Europe/Warsaw");

            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc),
                warsawTimeZone);
        }
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(
                "dddd, dd.MM.yyyy, 'godz.' HH:mm",
                new CultureInfo("pl-PL"));
        }

        public static string ToTimeFull(this DateTime dateTime)
        {
            return dateTime.ToWarsawTime().ToTimeString();
        }


    }
}
