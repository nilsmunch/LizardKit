using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LizardKit.Populator
{
    #if UNITY_EDITOR
    public static class PopulatorUtility
    {
        public static void Populate<T>(List<T> targetList) where T : ScriptableObject
        {
            if (targetList == null) return;

            targetList.Clear();

            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);

                if (asset) targetList.Add(asset);
            }
        }
        
        public static IList Populate(Type type)
        {
            var listType = typeof(List<>).MakeGenericType(type);
            var targetList = (IList)Activator.CreateInstance(listType);

            var guids = AssetDatabase.FindAssets($"t:{type.Name}");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, type);

                if (asset != null)
                    targetList.Add(asset);
            }

            return targetList;
        }
    }
    #endif
}