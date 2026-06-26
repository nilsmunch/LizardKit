using System;
using System.Collections.Generic;
using System.Linq;
using LizardKit.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LizardKit.UI
{
    public class BaseMainMenu : MonoBehaviour
    {
        private List<BaseMainMenuPanel> _menuPages;
        public string gameSceneName = "MainGame";
        private string _showingPanel;

        protected virtual void Awake()
        {
            MasterVolumeSlider.GlobalPreload();
            _menuPages = FindObjectsByType<BaseMainMenuPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            GoToRoot();
        }


        public virtual void StartGame()
        {
            SceneManager.LoadScene(gameSceneName);
        }
        
        public virtual void ShiftPage(string pageRequest)
        {
            if (_showingPanel == pageRequest)
            {
                GoToRoot();
                return;
            }

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

            _showingPanel = string.Empty;
        }
        
        public void QuitToDesktop()
        {
            Application.Quit();
        }
    }
}