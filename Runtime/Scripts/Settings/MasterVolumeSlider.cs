using LizardKit.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public class MasterVolumeSlider : SliderSetting
    {
        public static void GlobalPreload()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("master_volume", 0.5f);
        }

        public override void Preload()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("master_volume", 0.5f);
            base.Preload();
        }
        
        private void OnEnable()
        {
            Slider ??= GetComponentInChildren<Slider>();
            if (Slider) Slider.value = AudioListener.volume;
        }

        protected override float StartingValue() => AudioListener.volume;


        protected override void ValueChanged(float val)
        {
            AudioListener.volume = val;
            SoundEffectHandler.Play("WoodClick");
            PlayerPrefs.SetFloat("master_volume",val);
            PlayerPrefs.Save();
        }
    }
}
