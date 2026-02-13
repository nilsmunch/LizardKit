using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace LizardKit.DebugButton
{
    public class DebugButton
    {
        private readonly string _displayName;
        private readonly MethodInfo _method;
        public readonly ButtonAttribute ButtonAttribute;

        public DebugButton(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            ButtonAttribute = buttonAttribute;
            _displayName = string.IsNullOrEmpty(buttonAttribute.Name)
                ? ObjectNames.NicifyVariableName(method.Name)
                : buttonAttribute.Name;

            _method = method;
        }

        internal void Draw(IEnumerable<object> targets)
        {
            if (!GUILayout.Button(_displayName)) return;

            foreach (object target in targets)
            {
                _method.Invoke(target, null);
            }
        }
    }
}
#endif