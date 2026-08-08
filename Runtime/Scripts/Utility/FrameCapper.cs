using UnityEngine;

namespace LizardKit.Utility
{
    public class FrameCapper : MonoBehaviour
    {
        public int cap = 90;
        private void Start()
        {
            var target = cap;
            #if UNITY_WEBGL
                target = Mathf.FloorToInt(cap * 0.65f);
            #endif

            Application.targetFrameRate = target;
        }
    }
}
