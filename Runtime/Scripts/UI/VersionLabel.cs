using TMPro;
using UnityEngine;

namespace LizardKit.UI
{
    public class VersionLabel : MonoBehaviour
    {
        private void Awake()
        {
            var label =  GetComponent<TMP_Text>();
            label.text = "v." + Application.version;
        }
    }
}