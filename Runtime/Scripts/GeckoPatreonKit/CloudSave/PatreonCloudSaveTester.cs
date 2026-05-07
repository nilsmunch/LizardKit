using System;
using LizardKit.DebugButton;
using LizardKit.GeckoPatreonKit.CloudSave;
using UnityEngine;

namespace GeckoPatreonKit.CloudSave
{
    public class PatreonCloudSaveTester : MonoBehaviour
    {
        [SerializeField] private PatreonCloudSaveClient cloudSave;

        [Button]
        public void Save()
        {
            var saveObj = new DemoSaveData
            {
                level = 5,
                gold = 1234,
                lastScene = "Dungeon01"
            };
            
            var saveDataJson = JsonUtility.ToJson(saveObj);
            cloudSave.StoreSave(saveDataJson);
        }

        [Button]
        public void Load()
        {

            cloudSave.RecoverSave(save =>
            {
                if (save == null)
                {
                    Debug.Log("No save found.");
                    return;
                }
                var result = JsonUtility.FromJson<DemoSaveData>(save);

                Debug.Log($"Loaded: Level {result.level}, Gold {result.gold}, Scene {result.lastScene}");
            });
        }
        
        
        [Serializable]
        public class DemoSaveData
        {
            public int level;
            public int gold;
            public string lastScene;
        }
    }
}