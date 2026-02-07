using System;
using LizardKit.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public abstract class SettingToggle : MonoBehaviour
    {
        private Toggle _toggle;

        protected virtual bool ShouldStartEnabled() => false;

        protected virtual string SettingKey()
        {
            return GetType().Name;
        }

        private void Start()
        {
            var key = SettingKey(); 
            _toggle = GetComponent<Toggle>();
            _toggle.isOn = PlayerPrefs.GetInt(key, (ShouldStartEnabled() ? 1:0)) == 1;
            _toggle.onValueChanged.AddListener(ShiftState);
        }

        public static bool IsActive<T>() where T : SettingToggle
        {
            var key = typeof(T).Name;
            if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key) == 1;
            var defaultValue = Activator.CreateInstance<T>().ShouldStartEnabled();
            PlayerPrefs.SetInt(key, (defaultValue ? 1 : 0));
            return PlayerPrefs.GetInt(key,(defaultValue ? 1:0)) == 1;
        }

        protected virtual void ShiftState(bool on)
        {
            var key = SettingKey(); 
            PlayerPrefs.SetInt(key,on ? 1 : 0);
            SoundEffectHandler.Play("WoodClick"+(on ? "" : "Off"));
        }
    
        private void OnDestroy()
        {
            if (_toggle) _toggle.onValueChanged.RemoveListener(ShiftState);
        }
    }
}