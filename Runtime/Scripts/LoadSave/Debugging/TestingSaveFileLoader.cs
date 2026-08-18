using System.Collections.Generic;
using UnityEngine;

namespace LizardKit.LoadSave
{
    public class TestingSaveFileLoader : MonoBehaviour
    {
        public List<TestingSaveFiles> saveFiles = new();

        public void LoadFromSaveFile(TestingSaveFiles save)
        {
            GameLoadManager.LoadFromJson(save.ForceSaveFile);
        }

        public void SaveToSaveFile(TestingSaveFiles save)
        {
            var data = GameLoadManager.CurrentSaveData();
            save.ForceSaveFile = data;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif
        }
    }
}
