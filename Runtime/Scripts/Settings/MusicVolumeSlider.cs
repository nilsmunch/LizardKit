using GeckoKit.AudioKit;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public class MusicVolumeSlider : SliderSetting
    {
        public static float Preload()
        {
            return PlayerPrefs.GetFloat("bgm_volume", 0.5f);
        }

        private void OnEnable()
        {
            Slider ??= GetComponentInChildren<Slider>();
            if (Slider) Slider.value = Preload();
        }

        protected override void ValueChanged(float val)
        {
            if (!MusicManager.Instance) return;
            MusicManager.Instance.Source.volume = val;
            PlayerPrefs.SetFloat("bgm_volume", val);
            PlayerPrefs.Save();
        }
    }
}