using System.Runtime.CompilerServices;


namespace Events.DataBase.Extensions;


public static class IAsynEnumerableExtensions
{
    public static async IAsyncEnumerable<T> WithEnforcedCancellation<T>(this IAsyncEnumerable<T> @this, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var item in @this)
        {
            yield return item;
        }
    }
}
