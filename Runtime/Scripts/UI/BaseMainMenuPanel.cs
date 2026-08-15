using UnityEngine;

namespace LizardKit.UI
{
    public class BaseMainMenuPanel : MonoBehaviour
    {
        public string key;
        public bool rootPanel;

        public virtual void PrepareView()
        {
        }
    }
}