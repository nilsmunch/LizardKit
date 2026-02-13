using UnityEngine;

namespace LizardKit.BaseExtensions
{
    public static class GameObjectExtensions
    {
        public static void ClearChildren(this GameObject go, bool immediate = false)
        {
            if (!go) return;
            
            go.transform.ClearChildren(immediate);
        }

        public static void ClearChildren(this Transform transform, bool immediate = false)
        {
            if (!transform) return;

            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);

#if UNITY_EDITOR
                if (!Application.isPlaying && immediate)
                    Object.DestroyImmediate(child.gameObject);
                else
#endif
                    Object.Destroy(child.gameObject);
            }
        }
    }
}