using System;
using System.Collections.Generic;
using System.Linq;
using GeckoKit.LoadSave;
using GeckoKit.LoadSave.Handlers;
using LizardKit.Scaffolding;

namespace LizardKit.LoadSave
{
    public abstract class BaseLoadSaveHandler<T, TSaveFile> : BaseManager<T>
        where T : BaseManager<T>
        where TSaveFile : BaseSaveFile
    {
        protected bool _readyToSave;
        protected BaseSaveDataHandler<TSaveFile>[] Handlers;

        #region ABSTRACT FUNCTIONS
        public abstract List<TSaveFile> ListSaveFiles();
        public abstract void LoadFile(TSaveFile saveFile);
        public abstract void SaveFile(TSaveFile saveFile);
        #endregion

        #region CONSTRUCTORS
        protected override void Awake()
        {
            base.Awake();
            PreloadHandlers();
        }
        private void PreloadHandlers()
        {
            Handlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(type =>
                    !type.IsAbstract &&
                    !type.IsGenericTypeDefinition &&
                    typeof(BaseSaveDataHandler<TSaveFile>).IsAssignableFrom(type))
                .Select(type => (BaseSaveDataHandler<TSaveFile>)Activator.CreateInstance(type))
                .ToArray();
        }
        #endregion
    }
}