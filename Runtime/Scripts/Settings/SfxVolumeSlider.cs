using System.Collections.Generic;
using LizardKit.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public class SfxVolumeSlider : SliderSetting
    {
        public List<AudioClip> TestEffects;
        public static float Preload()
        {
            return PlayerPrefs.GetFloat("sfx_volume", 0.75f);
        }

        private void OnEnable()
        {
            Slider ??= GetComponentInChildren<Slider>();
            if (Slider) Slider.value = Preload();
        }

        protected override void ValueChanged(float val)
        {
            SoundEffectHandler.Volume = val;
            var test = TestEffects[Random.Range(0,TestEffects.Count)];
            SoundEffectHandler.PlayClip(test);
            PlayerPrefs.SetFloat("sfx_volume", val);
            PlayerPrefs.Save();
        }
    }
}