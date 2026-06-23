using UnityEngine;

namespace LizardKit.UI
{
    public class HideFromMobile : MonoBehaviour
        {
            private void Awake()
            {
#if UNITY_ANDROID || UNITY_IOS          
            gameObject.SetActive(false);
#endif
            }
    }
}