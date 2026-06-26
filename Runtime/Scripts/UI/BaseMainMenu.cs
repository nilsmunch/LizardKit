using System.Collections.Generic;
using System.Linq;
using LizardKit.Settings;
using UnityEngine;

namespace LizardKit.UI
{
    public class BaseMainMenu : MonoBehaviour
    {
        private List<BaseMainMenuPanel> _menuPages;

        protected virtual void Awake()
        {
            _menuPages = FindObjectsByType<BaseMainMenuPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            MasterVolumeSlider.GlobalPreload();
        }

        public virtual void ShiftPage(string pageRequest)
        {
            foreach (var page in _menuPages)
            {
                page.gameObject.SetActive(page.key == pageRequest);
            }
        }
        public virtual void GoToRoot()
        {
            foreach (var page in _menuPages)
            {
                page.gameObject.SetActive(page.rootPanel);
            }
        }
        
        public void QuitToDesktop()
        {
            Application.Quit();
        }
    }
}