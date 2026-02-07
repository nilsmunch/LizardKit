using System.Linq;
using LizardKit.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public class MasterUiScaleSlider : SliderSetting
    {
        public ModalWindow KeepOrCancel;
        
        public static int UiScale = 1;
        public static void Preload()
        {
            UiScale = PlayerPrefs.GetInt("ui_scale",1);
            UpdateScale();
        }

        public static void UpdateScale()
        {
            FindObjectsByType<CanvasScaler>(FindObjectsInactive.Include, FindObjectsSortMode.None).First()
                .scaleFactor = UiScale;
        }

        public void UpdateScaleAction()
        {
            UpdateScale();
            KeepOrCancel.Show(
                "Would you like to keep these settings?",
                () => Debug.Log("Accepted!"),
                () =>
                {
                    UiScale = 1;
                    UpdateScale();
                },
                10
            );
        }

        private void OnEnable()
        {
            Slider.value = UiScale;
        }

        public void ResetScaleAction()
        {
            PlayerPrefs.SetInt("ui_scale",1);
            UiScale = 1;
            UpdateScale();
            Slider.value = UiScale;
        }

        protected override void Awake()
        {
            base.Awake();
            Slider.value = UiScale;
        }

        protected override void ValueChanged(float val)
        {
            UiScale = (int)val;
            SoundEffectHandler.Play("WoodClick");
            PlayerPrefs.SetInt("ui_scale",UiScale);
        }
    }
}