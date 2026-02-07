using System;
using System.Collections.Generic;
using System.Linq;

namespace LizardKit.Scripts.BaseExtensions
{
        public static class ListExtensions
    {
        // Extension method to shuffle a list with an optional seed
        public static void Shuffle<T>(this IList<T> list, int? seed = null)
        {
            var rng = seed.HasValue ? new Random(seed.Value) : new Random();
            var n = list.Count;

            for (var i = n - 1; i > 0; i--)
            {
                var k = rng.Next(i + 1);
                (list[k], list[i]) = (list[i], list[k]);
            }
        }
    
        public static string ConcatenateWithComma<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return string.Empty;
            }

            return list.Aggregate("", (current, t) => current + t);
        }
        
        
        public static T RandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }
            var index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }
    
        public static T RandomElementButSkip<T>(this List<T> list, T skip)
        {
            if (list == null || list.Count == 0)
            {
                throw new InvalidOperationException("Cannot select a random element from an empty or null list.");
            }
            var index = UnityEngine.Random.Range(0, list.Count);
            if (Equals(list[index], skip)) index = UnityEngine.Random.Range(0, list.Count);
            if (Equals(list[index], skip)) index = UnityEngine.Random.Range(0, list.Count);
            if (Equals(list[index], skip)) index = UnityEngine.Random.Range(0, list.Count);

            return list[index];
        }

    
        public static T PopRandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new InvalidOperationException("Cannot pop a random element from an empty or null list.");
            }
            var index = UnityEngine.Random.Range(0, list.Count);
            var element = list[index];
            list.RemoveAt(index);
            return element;
        }
    }
}