namespace Booking.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDisplayDate(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy.");
        }
        public static string ToDisplayTime(this DateTime dateTime)
        {
            return $"{dateTime.ToString("HHmm")}z";
        }
    }
}
