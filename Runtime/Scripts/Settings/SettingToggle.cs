using System;
using LizardKit.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public abstract class SettingToggle : MonoBehaviour
    {
        private Toggle _toggle;
        private bool _preloaded;
        private string _settingsKey;

        protected virtual bool ShouldStartEnabled() => false;

        protected virtual string SettingKey()
        {
            return GetType().Name;
        }

        public virtual void Preload()
        {
            if (_preloaded) return;
            _preloaded = true;
            _toggle = GetComponent<Toggle>();
            _settingsKey = SettingKey(); 
            _toggle.isOn = PlayerPrefs.GetInt(_settingsKey, (ShouldStartEnabled() ? 1:0)) == 1;
        }

        private void Start()
        {
            Preload();
            if (!_toggle)
            {
                Debug.LogError($"Toggle not found in {gameObject.name}");
                return;
            }

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
            PlayerPrefs.SetInt(_settingsKey,on ? 1 : 0);
            SoundEffectHandler.Play("WoodClick"+(on ? "" : "Off"));
        }
    
        private void OnDestroy()
        {
            if (_toggle) _toggle.onValueChanged.RemoveListener(ShiftState);
        }
    }
}