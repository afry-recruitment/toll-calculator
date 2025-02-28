namespace TollFeeCalculator.Extensions
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Create groups a specific criteria where base value is determined by first item in group ex: Times are in between 60mins of first item. 
        /// </summary>
        internal static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> continueCriteria)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    yield break;
                }

                var currentGroup = new List<T> { enumerator.Current };
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    if (continueCriteria(currentGroup.First(), current))
                    {
                        currentGroup.Add(current);
                    }
                    else
                    {
                        yield return currentGroup;
                        currentGroup = new List<T> { current };
                    }
                }

                yield return currentGroup;
            }
        }
    }
}
