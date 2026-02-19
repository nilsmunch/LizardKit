using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LizardKit.Populator;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
namespace LizardKit.DebugButton
{
    [CustomEditor(typeof(Object), true, isFallback = true)]
    internal class ObjectEditor : UnityEditor.Editor
    {
        private ButtonsDrawer _buttonsDrawer;

        private void OnEnable()
        {
            _buttonsDrawer = new ButtonsDrawer(target);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            _buttonsDrawer.DrawButtons(targets);

            DrawPopulatorButtons();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPopulatorButtons()
        {
            var targetObject = target;

            var fields = targetObject.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(PopulatorButtonAttribute)))
                    continue;

                if (!typeof(IList).IsAssignableFrom(field.FieldType))
                    continue;

                EditorGUILayout.Space();

                if (GUILayout.Button($"Populate {field.Name}"))
                {
                    var elementType = field.FieldType.GetGenericArguments()[0];

                    var serializedObj = new SerializedObject(targetObject);
                    var prop = serializedObj.FindProperty(field.Name);

                    if (prop != null && prop.isArray)
                    {
                        prop.ClearArray();

                        var populatedList = PopulatorUtility.Populate(elementType);

                        int index = 0;
                        foreach (var item in (IList)populatedList)
                        {
                            prop.InsertArrayElementAtIndex(index);
                            prop.GetArrayElementAtIndex(index).objectReferenceValue = (UnityEngine.Object)item;
                            index++;
                        }

                        serializedObj.ApplyModifiedProperties();
                    }


                    EditorUtility.SetDirty(targetObject);
                }
            }
        }
    }
}
#endif