using System.Text.RegularExpressions;

namespace Booking.Extensions
{
    public static class IEnumerableExtensions
    {
        private static readonly Dictionary<string, int> AtcPositionOrder = new()
        {
                { "_DEL", 0 },
                { "_GND", 1 },
                { "_TWR", 2 },
                { "_APP", 3 },
                { "_CTR", 4 }
            };
        private static readonly Regex AtcRegex = new(string.Join("|", AtcPositionOrder.Keys), RegexOptions.Compiled);
        public static IOrderedEnumerable<T> OrderByAtcPosition<T>(this IEnumerable<T> source, Func<T, string> selector)
        {
            return source.OrderBy(item =>
            {
                var value = selector(item);
                if (value is not null && value.Length >= 4)
                {
                    return value[0..4];
                }
                return value;
            }).ThenBy(item =>
            {
                var value = selector(item);
                var match = AtcRegex.Match(value);
                if (match.Success && AtcPositionOrder.TryGetValue(match.Value, out int order))
                {
                    return order;
                }
                // If no match, sort after known positions
                return int.MaxValue;
            });
        }
    }
}
