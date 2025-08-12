namespace Booking.Tests.Extensions
{
    internal static class ListExtensions
    {
        public static void Update<T>(this List<T> list, T obj)
        {
            int index = list.FindIndex(o => EqualityComparer<T>.Default.Equals(o, obj));
            if (index >= 0)
            {
                list[index] = obj;
            }
        }
    }
}
