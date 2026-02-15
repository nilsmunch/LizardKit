using LizardKit.Audio;
using UnityEngine;

namespace LizardKit.Settings
{
    public class MasterVolumeSlider : SliderSetting
    {
        public static void Preload()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("master_volume", 0.5f);
        }

        protected override void Awake()
        {
            base.Awake();
            Slider.value = AudioListener.volume;
        }

        protected override void ValueChanged(float val)
        {
            AudioListener.volume = val;
            SoundEffectHandler.Play("WoodClick");
            PlayerPrefs.SetFloat("master_volume",val);
            PlayerPrefs.Save();
        }
    }
}
