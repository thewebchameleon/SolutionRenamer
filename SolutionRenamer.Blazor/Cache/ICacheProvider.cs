using System;

namespace SolutionRenamer.Blazor.Cache
{
    public interface ICacheProvider
    {
        bool TryGet<T>(string key, out T value);

        T Set<T>(string key, T value);

        T Set<T>(string key, T value, DateTimeOffset expiryDate);

        void Remove(string key);
    }
}
