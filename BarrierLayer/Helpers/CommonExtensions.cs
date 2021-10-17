using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarrierLayer.Helpers
{
    public static class CommonExtensions
    {
        public static T ThrowIfNull<T>(this T entity, object id)
            => entity != null ? entity : throw new ArgumentNullException($"Не найден {typeof(T).Name} c ID: {id}");
        
        public static Task<T> ThrowIfNull<T>(this Task<T> entityTask, object id)
            => entityTask.ContinueWith(entity => entity.Result.ThrowIfNull(id));
    }
}