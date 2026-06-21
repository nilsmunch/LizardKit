using System;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public abstract class SliderSetting : MonoBehaviour
    {
        protected Slider Slider;
        private bool _preloaded;

        protected abstract float StartingValue();

        public virtual void Preload()
        {
            if (_preloaded) return;
            _preloaded = true;
            SecureSlider();
            Slider.value = StartingValue();
        }

        protected virtual void Awake()
        {
            SecureSlider();
            Slider.onValueChanged.AddListener(ValueChanged);
        }

        protected void SecureSlider()
        {
            if (Slider) return;
            if (!Slider) Slider = GetComponentInChildren<Slider>(true);
            if (!Slider) Slider = GetComponent<Slider>();
        }

        protected virtual void ValueChanged(float val)
        {
            throw new NotImplementedException();
        }
    }
}
