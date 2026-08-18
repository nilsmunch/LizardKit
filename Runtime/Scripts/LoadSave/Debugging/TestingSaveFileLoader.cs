using System.Collections.Generic;
using UnityEngine;

namespace LizardKit.LoadSave
{
    public class TestingSaveFileLoader : MonoBehaviour
    {
        public IJsonSaveHandler handler;
        public List<TestingSaveFiles> saveFiles = new();

        public void LoadFromSaveFile(TestingSaveFiles save)
        {
            handler.LoadJson(save.ForceSaveFile);
        }

        public void SaveToSaveFile(TestingSaveFiles save)
        {
            var data = handler.CurrentSaveJson();
            save.ForceSaveFile = data;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif
        }
    }
}
