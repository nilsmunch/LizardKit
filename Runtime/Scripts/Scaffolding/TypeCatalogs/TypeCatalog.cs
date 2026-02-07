using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace Scaffolding
{
    public static class TypeCatalog
    {
        private static readonly Dictionary<Type, object> _cache = new();
        private static readonly object _lock = new();

        public sealed class Entry<TInterface>
        {
            public TInterface Instance;
            public float Weight;
            public int Order;
            public Type Type;
            public bool Introduced;
        }

        public static IReadOnlyList<Entry<TInterface>> Get<TInterface>(
            bool createInstances = true,
            IEnumerable<Assembly> assemblies = null)
            where TInterface : class
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(typeof(TInterface), out var boxed) &&
                    boxed is List<Entry<TInterface>> cached)
                {
                    return cached;
                }

                var built = Discover<TInterface>(createInstances, assemblies)
                    // Ensure deterministic order: first by Order, then by name
                    .OrderBy(e => e.Order)
                    .ThenBy(e => e.Type.FullName, StringComparer.Ordinal)
                    .ToList();

                _cache[typeof(TInterface)] = built;
                return built;
            }
        }

        public static IReadOnlyList<TInterface> GetInstances<TInterface>(
            IEnumerable<Assembly> assemblies = null)
            where TInterface : class
            => Get<TInterface>(createInstances: true, assemblies)
               .Select(e => e.Instance)
               .ToList();

        public static void Clear<TInterface>() where TInterface : class
        {
            lock (_lock) { _cache.Remove(typeof(TInterface)); }
        }

        public static void ClearAll()
        {
            lock (_lock) { _cache.Clear(); }
        }

        private static IEnumerable<Entry<TInterface>> Discover<TInterface>(
            bool createInstances,
            IEnumerable<Assembly> assemblies)
            where TInterface : class
        {
            var asms = assemblies ?? AppDomain.CurrentDomain.GetAssemblies();

            var types = asms.SelectMany(asm =>
            {
                try { return asm.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null); }
                catch { return Array.Empty<Type>(); }
            });

            var concrete = types.Where(t =>
                t != null &&
                !t.IsAbstract &&
                !t.IsInterface &&
                typeof(TInterface).IsAssignableFrom(t) &&
                t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var t in concrete)
            {
                TInterface instance = default;
                if (createInstances)
                {
                    try
                    {
                        instance = (TInterface)Activator.CreateInstance(t);
                    }
                    catch (Exception e)
                    {
#if UNITY_5_3_OR_NEWER
                        Debug.LogWarning($"TypeCatalog: Failed to create {t.FullName}: {e.Message}");
#endif
                        continue;
                    }
                }

                // Weight: always from WeightedChanceAttribute (default 1 if missing)
                float weight = 1f;
                try
                {
                    var weightAttr = t.GetCustomAttribute<WeightedChanceAttribute>(inherit: false);
                    weight = Math.Max(0f, weightAttr?.Weight ?? 1f);
                }
                catch (Exception e)
                {
#if UNITY_5_3_OR_NEWER
                    Debug.LogWarning($"TypeCatalog: Failed to read WeightedChanceAttribute on {t.FullName}: {e.Message}");
#endif
                }

                // Order: from OrderKeyAttribute (default 0 if missing)
                int order = 0;
                try
                {
                    var orderAttr = t.GetCustomAttribute<OrderKeyAttribute>(inherit: false);
                    if (orderAttr != null) order = orderAttr.Order;
                }
                catch (Exception e)
                {
#if UNITY_5_3_OR_NEWER
                    Debug.LogWarning($"TypeCatalog: Failed to read OrderKeyAttribute on {t.FullName}: {e.Message}");
#endif
                }

                yield return new Entry<TInterface>
                {
                    Instance = instance,
                    Weight = weight,
                    Order = order,
                    Type = t
                };
            }
        }
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class WeightedChanceAttribute : Attribute
    {
        public float Weight { get; }
        public WeightedChanceAttribute(float weight = 1f) => Weight = Math.Max(0f, weight);
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class OrderKeyAttribute : Attribute
    {
        public int Order { get; }
        public OrderKeyAttribute(int order) => Order = order;
    }

}
