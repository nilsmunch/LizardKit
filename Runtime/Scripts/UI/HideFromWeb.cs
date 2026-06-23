using System;
using UnityEngine;

namespace LizardKit.UI
{
    public class HideFromWeb : MonoBehaviour
    {
        private void Awake()
        {
            #if UNITY_WEBGL            
            gameObject.SetActive(false);
#endif
        }
    }
}