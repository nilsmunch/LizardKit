using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GeckoKit.LoadSave;
using LizardKit.DebugButton;
using LizardKit.Scaffolding;
using UnityEngine;

namespace LizardKit.LoadSave
{
    public abstract class BaseCloudSaveHandler<THandler, TSaveFile> 
        : BaseLoadSaveHandler<THandler, TSaveFile>
        where THandler : BaseManager<THandler>
        where TSaveFile : BaseSaveFile, new()
    {
        private const string Filename = "/SteamCloudSave.sav";
        private static string FilePos => Application.persistentDataPath + Filename;
        
        public override List<TSaveFile> ListSaveFiles()
        {
            var list = new List<TSaveFile>();
            if (!File.Exists(FilePos)) return list;
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
            return list;
        }

        [Button]
        protected void LoadFirstFile()
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
        
        public string GetSaveFileAsString(TSaveFile saveFile)
        {
            foreach (var dataHandler in Handlers)
            {
                dataHandler.SaveToFile(saveFile);
            }

            using var stream = new MemoryStream();
            var bf = new BinaryFormatter();

            bf.Serialize(stream, saveFile);

            var bytes = stream.ToArray();
            return System.Convert.ToBase64String(bytes);
        }

        public abstract TSaveFile CompareAndMergeSaves(TSaveFile localSave, TSaveFile remoteSave);

        public TSaveFile GetSaveFileFromString(string saveString)
        {
            var bytes = System.Convert.FromBase64String(saveString);

            using var stream = new MemoryStream(bytes);
            var bf = new BinaryFormatter();

            return bf.Deserialize(stream) as TSaveFile;
        }
    }
}