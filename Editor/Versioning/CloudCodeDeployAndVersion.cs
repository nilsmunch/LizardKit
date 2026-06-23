#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace LizardKit.Versioning
{
    public static class CloudCodeDeployAndVersion
    {
        [MenuItem("Tools/LizardKit/Deploy And Increment Version")]
        public static void DeployAndIncrement()
        {
            // Adjust args to your project/environment setup.
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ugs",
                    Arguments = "deploy",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Debug.Log(output);

            if (process.ExitCode != 0)
            {
                Debug.LogError($"Cloud Code deploy failed:\n{error}");
                return;
            }

            IncrementPatchVersion();
        }

        [MenuItem("Tools/LizardKit/Increment Version Only")]
        private static void IncrementPatchVersion()
        {
            var version = PlayerSettings.bundleVersion;
            var parts = version.Split('.');

            if (parts.Length == 3 && int.TryParse(parts[2], out var patch))
            {
                patch++;
                var newVersion = $"{parts[0]}.{parts[1]}.{patch}";
                PlayerSettings.bundleVersion = newVersion;

                EditorUtility.SetDirty(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
                AssetDatabase.SaveAssets();

                Debug.Log($"[CloudCodeDeployAndVersion] Updated version: {version} → {newVersion}");
            }
            else
            {
                Debug.LogWarning("Could not parse version string. Expected X.Y.Z");
            }
        }
    }
}
#endif