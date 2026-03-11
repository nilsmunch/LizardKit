using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using LizardKit.DebugButton;
using UnityEngine;

namespace GeckoKit.LoadSave
{
    public abstract class BaseCloudSaveHandler<THandler, TSaveFile> 
        : BaseLoadSaveHandler<THandler, TSaveFile>
        where THandler : MonoBehaviour
        where TSaveFile : BaseSaveFile, new()
    {
        private const string FILENAME = "/SteamCloudSave.sav";
        private static string FilePos => Application.persistentDataPath + FILENAME;
        
        public override List<TSaveFile> ListSaveFiles()
        {
            var list = new List<TSaveFile>();
            if (File.Exists(FilePos))
            {
                var bf = new BinaryFormatter();
                var stream = new FileStream(FilePos, FileMode.Open);
                TSaveFile data = null;
                try
                {
                    data = bf.Deserialize(stream) as TSaveFile;
                }
                catch
                {
                    // ignored
                };

                if (data == null)
                {
                    LogError("Broken file at "+FilePos);
                    return list;
                }

                list.Add(data);
                stream.Close();
            }
            return list;
        }

        [Button]
        private void LoadFirstFile()
        {
            var list = ListSaveFiles();
            var saveFile = list.FirstOrDefault();
            if (saveFile == null)
            {
                LogError("No save files found");
                return;
            }
            LoadFile(saveFile);
        }


        public override void LoadFile(TSaveFile saveFile)
        {
            Log(saveFile.ToString());
            foreach (var dataHandler in Handlers)
            {
                dataHandler.LoadFromFile(saveFile);
            }
        }
        
        [Button]
        private void SaveDummyFile()
        {
            var saveFile = new TSaveFile();
            SaveFile(saveFile);
        }

        public override void SaveFile(TSaveFile saveFile)
        {
            foreach (var dataHandler in Handlers)
            {
                dataHandler.SaveToFile(saveFile);
            }

            #region DISK IO
            var stream = new FileStream(FilePos, FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(stream, saveFile);
            stream.Close();
            #endregion
            
            Log($"{saveFile} Saved to {FilePos}");
        }
    }
}