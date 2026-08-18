using UnityEngine;

namespace LizardKit.LoadSave
{
    [CreateAssetMenu]
    public class TestingSaveFiles : ScriptableObject
    {
        [TextArea(2,20)]
        public string ForceSaveFile;
    }
}