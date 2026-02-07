using UnityEngine;

namespace LizardKit.Settings
{
    public class MasterUiScaleBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            MasterUiScaleSlider.Preload();
        }
    }
}