using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LizardKit.BaseExtensions
{
    public static class GameObjectInterfaceExtensions
    {
        public static List<T> FindObjectsWithInterface<T>(
            FindObjectsInactive inactive = FindObjectsInactive.Include,
            FindObjectsSortMode sortMode = FindObjectsSortMode.InstanceID)
            where T : class
        {
            return Object
                .FindObjectsByType<MonoBehaviour>(inactive, sortMode)
                .OfType<T>()
                .ToList();
        }
        
        public static T GetInterface<T>(this GameObject go) where T : class
        {
            return go.GetComponents<MonoBehaviour>().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetInterfaces<T>(this GameObject go) where T : class
        {
            return go.GetComponents<MonoBehaviour>().OfType<T>();
        }

        public static T GetInterface<T>(this Component c) where T : class
        {
            return c.GetComponents<MonoBehaviour>().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetInterfaces<T>(this Component c) where T : class
        {
            return c.GetComponents<MonoBehaviour>().OfType<T>();
        }
    }
}