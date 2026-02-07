using System;
using UnityEngine;

namespace LizardKit.Scaffolding
{
    public abstract class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        [SerializeField] protected bool debugLog;
        protected bool Persist;

        public static T Instance => _instance != null ? _instance : null;

        protected static void ForceIn(T forced)
        {
            _instance = forced;
        }

        protected virtual void Awake()
        {
            if (Persist && _instance)
            {
                Destroy(this);
                return;
            }

            if (_instance != null) return;
            _instance = this as T;
            if (Persist) DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnEnable()
        {
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        #region LOGGING

        protected void Log(string log)
        {
            #if UNITY_EDITOR
            if (debugLog) Debug.Log("<color=#00FFFF><b>"+typeof(T)+"</b></color>: "+log);
            #endif
        }

        protected void LogError(string log)
        {
            #if UNITY_EDITOR
            if (debugLog) Debug.LogError("<color=#00FFFF><b>"+typeof(T)+"</b></color>: "+log);
            #endif
        }
        

        #endregion
    }
}
