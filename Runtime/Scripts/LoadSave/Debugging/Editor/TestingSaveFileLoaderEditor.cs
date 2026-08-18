#if UNITY_EDITOR
using LizardKit.LoadSave;
using UnityEditor;
using UnityEngine;

namespace LizardKit.LoadSave
{
[CustomEditor(typeof(TestingSaveFileLoader))]
public class TestingSaveFileLoaderEditor : Editor
{
    private SerializedProperty saveFilesProp;

    private void OnEnable()
    {
        saveFilesProp = serializedObject.FindProperty("saveFiles");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var loader = (TestingSaveFileLoader)target;

        // Draw default list normally at the top
        EditorGUILayout.PropertyField(saveFilesProp, true);

        EditorGUILayout.Space(15);

        EditorGUILayout.LabelField("Save File Actions", EditorStyles.boldLabel);

        for (var i = 0; i < loader.saveFiles.Count; i++)
        {
            var save = loader.saveFiles[i];

            if (!save)
                continue;

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                string.IsNullOrWhiteSpace(save.name)
                    ? $"Save File {i}"
                    : save.name,
                EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Load"))
            {
                loader.LoadFromSaveFile(save);
            }

            if (GUILayout.Button("Save"))
            {
                Undo.RecordObject(loader, "Save Testing Save File");

                loader.SaveToSaveFile(save);

                EditorUtility.SetDirty(loader);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
}
#endif