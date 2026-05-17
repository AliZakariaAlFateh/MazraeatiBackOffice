using Microsoft.Extensions.Caching.Memory;
using System;

namespace MazraeatiBackOffice.Configuration
{
    public class TimeCacheService
    {
        private readonly IMemoryCache _cache;
        private const string PrevKey = "prev";

        public TimeCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public (DateTime? previous, DateTime next) GetNextAndPrevious()
        {
            DateTime? previous = null;

            // Check if cache has previous value
            if (_cache.TryGetValue<DateTime>(PrevKey, out var prevValue))
            {
                previous = prevValue;
            }

            // Set next value as current time
            var next = DateTime.Now;

            // Update cache with new prev value (store next)
            _cache.Set(PrevKey, next);

            return (previous, next);
        }
    }
}
