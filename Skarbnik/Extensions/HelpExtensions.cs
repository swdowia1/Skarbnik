using System.Globalization;
using System.Security.Claims;

namespace Skarbnik.Extensions
{
    public static class HelpExtensions
    {
        private static readonly CultureInfo PolishCulture =
           CultureInfo.GetCultureInfo("pl-PL");

        public static string ToPln(this decimal value)
        {
            return value.ToString("C", PolishCulture);
        }
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
        public static bool IsLoggedIn(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated == true;
        }
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(
                "dddd, dd.MM.yyyy, 'godz.' HH:mm",
                PolishCulture);
        }

        public static string ToTimeFull(this DateTime dateTime)
        {
            return dateTime.ToWarsawTime().ToTimeString();
        }


    }
}
