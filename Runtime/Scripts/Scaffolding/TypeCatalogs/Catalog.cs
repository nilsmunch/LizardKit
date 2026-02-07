using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scaffolding.TypeCatalogs
{
    public static class Catalog<TInterface> where TInterface : class
    {
        private static List<TypeCatalog.Entry<TInterface>> _cache;

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Get cached entries (discover on first call).
        /// You can override weight, skip instance creation, or limit assemblies if needed.
        /// </summary>
        public static IReadOnlyList<TypeCatalog.Entry<TInterface>> Get(
            bool createInstances = true,
            IEnumerable<Assembly> assemblies = null)
        {
            if (_cache != null) return _cache;
            _cache = TypeCatalog.Get<TInterface>(createInstances, assemblies).ToList();
            return _cache;
        }

        /// <summary>Clear only this interface's cache.</summary>
        public static void ClearCache()
        {
            _cache = null;
            TypeCatalog.Clear<TInterface>();
        }
    }
}