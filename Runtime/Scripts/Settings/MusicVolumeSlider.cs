using LizardKit.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public class MusicVolumeSlider : SliderSetting
    {
        public static float GlobalPreload()
        {
            return PlayerPrefs.GetFloat("bgm_volume", 0.5f);
        }

        public override void Preload()
        {            
            MusicManager.Instance.Source.volume = PlayerPrefs.GetFloat("bgm_volume", 0.5f);
        }

        private void OnEnable()
        {
            Slider ??= GetComponentInChildren<Slider>();
            if (Slider) Slider.value = PlayerPrefs.GetFloat("bgm_volume", 0.5f);
        }

        protected override float StartingValue() => PlayerPrefs.GetFloat("bgm_volume", 0.5f);

        protected override void ValueChanged(float val)
        {
            if (!MusicManager.Instance) return;
            MusicManager.Instance.Source.volume = val;
            PlayerPrefs.SetFloat("bgm_volume", val);
            PlayerPrefs.Save();
        }
    }
}