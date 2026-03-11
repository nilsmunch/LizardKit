using JetBrains.Annotations;

namespace GeckoKit.LoadSave.Handlers
{
    public abstract class BaseSaveDataHandler<TSaveFile>
        where TSaveFile : BaseSaveFile
    {
        public abstract void SaveToFile(TSaveFile saveFile);
        public abstract void LoadFromFile(TSaveFile saveFile);
    }
}