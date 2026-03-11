using UnityEngine;

namespace GeckoKit.LoadSave.Handlers
{
    public class GameVersionBaseSaveHandler : BaseSaveDataHandler<BaseSaveFile>
    {
        public override void SaveToFile(BaseSaveFile saveFile)
        {
            saveFile.GameVersion = Application.version;
        }

        public override void LoadFromFile(BaseSaveFile saveFile)
        {
            Debug.Log("Loading game version " + Application.version);
        }
    }
}