using System;
using UnityEngine;

namespace LizardKit.Scaffolding
{
    public abstract class BaseManager<T> : MonoBehaviour
        where T : BaseManager<T>
    {
        private static T _instance;
        [SerializeField] protected bool debugLog;
        protected bool Persist;
        private bool _prepared;

        public static T Instance
        {
            get
            {
                if (_instance)
                    return _instance;

#if UNITY_2023_1_OR_NEWER
                _instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
#else
                _instance = FindObjectOfType<T>();
#endif

                if (_instance) _instance.EnsurePrepared();

                return _instance;
            }
        }

        protected virtual void Prepare()
        {
            if (Persist) DontDestroyOnLoad(gameObject);
        }

        private void EnsurePrepared()
        {
            if (_prepared) return;
            _prepared = true;
            Prepare();
        }


        protected virtual void Awake()
        {
            if (Persist && _instance)
            {
                Destroy(this);
                return;
            }

            if (_instance) return;
            _instance = this as T;
            EnsurePrepared();
        }
        
        

        protected virtual void OnEnable()
        {
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
        
        protected static void ForceIn(T forced)
        {
            _instance = forced;
        }

        #region LOGGING

        // ReSharper disable Unity.PerformanceAnalysis
        public void Log(string log)
        {
            #if UNITY_EDITOR
            if (debugLog) Debug.Log("<color=#00FFFF><b>"+typeof(T)+"</b></color>: "+log);
            #endif
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void LogError(string log)
        {
            #if UNITY_EDITOR
            if (debugLog) Debug.LogError("<color=#00FFFF><b>"+typeof(T)+"</b></color>: "+log);
            #endif
        }
        

        #endregion
    }
}
