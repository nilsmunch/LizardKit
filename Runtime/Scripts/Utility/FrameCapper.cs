using UnityEngine;

namespace LizardKit.Utility
{
    public class FrameCapper : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 90;
        }
    }
}
