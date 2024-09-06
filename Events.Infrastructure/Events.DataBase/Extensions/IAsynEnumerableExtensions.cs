using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


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
