using System;
using UnityEngine;

namespace LizardKit.Settings
{
    public class CommandLineHandler : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void CheckCommandLineFlags()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.Equals("--resetscale", StringComparison.OrdinalIgnoreCase))
                {
                    ResetResolution();
                    break;
                }
            }
        }

        static void ResetResolution()
        {
            Debug.Log("Resetting resolution and UI scale to defaults.");

            MasterUiScaleSlider.UiScale = 1;
            PlayerPrefs.SetInt("ui_scale",1);
            MasterUiScaleSlider.UpdateScale();
            PlayerPrefs.Save();
        }
    }
}