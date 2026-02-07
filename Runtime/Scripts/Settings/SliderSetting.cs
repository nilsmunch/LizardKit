using System;
using UnityEngine;
using UnityEngine.UI;

namespace LizardKit.Settings
{
    public abstract class SliderSetting : MonoBehaviour
    {
        public Slider Slider;

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
