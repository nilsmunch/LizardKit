#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
}
#endif