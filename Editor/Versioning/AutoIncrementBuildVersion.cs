#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LizardKit.Editor.Versioning
{
    public class AutoIncrementBuildVersion : IPreprocessBuildWithReport
    {
        // Determines when this preprocessor runs (0 = very early)
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            string version = PlayerSettings.bundleVersion; // e.g. "0.3.9"
            string[] parts = version.Split('.');

            if (parts.Length == 3 &&
                int.TryParse(parts[2], out int patch))
            {
                patch++; // increment patch
                string newVersion = $"{parts[0]}.{parts[1]}.{patch}";
                PlayerSettings.bundleVersion = newVersion;

                // (Optional) Also bump build number for iOS / Android
#if UNITY_IOS
            PlayerSettings.iOS.buildNumber = newVersion;
#endif
#if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode++;
#endif

                Debug.Log($"[AutoIncrementBuildVersion] Updated version: {version} → {newVersion}");
            }
            else
            {
                Debug.LogWarning("[AutoIncrementBuildVersion] Could not parse version string. Expected format: X.Y.Z");
            }
        }
    }
}
#endif