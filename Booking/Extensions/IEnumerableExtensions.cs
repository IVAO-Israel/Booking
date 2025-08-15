using System.Text.RegularExpressions;

namespace Booking.Extensions
{
    public static class IEnumerableExtensions
    {
        private static readonly Dictionary<string, int> AtcPositionOrder = new Dictionary<string, int>
            {
                { "_DEL", 0 },
                { "_GND", 1 },
                { "_TWR", 2 },
                { "_APP", 3 },
                { "_CTR", 4 }
            };
        private static readonly Regex AtcRegex = new Regex(string.Join("|", AtcPositionOrder.Keys), RegexOptions.Compiled);
        public static IOrderedEnumerable<T> OrderByAtcPosition<T>(this IEnumerable<T> source, Func<T, string> selector)
        {
            return source.OrderBy(selector).ThenBy(item =>
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
