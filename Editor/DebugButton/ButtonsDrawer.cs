using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

#if UNITY_EDITOR
namespace LizardKit.DebugButton
{
    public class ButtonsDrawer
    {
        private readonly List<IGrouping<string, DebugButton>> _buttonGroups;

        public ButtonsDrawer(object target)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var methods = target.GetType().GetMethods(flags);
            var buttons = new List<DebugButton>();
            var rowNumber = 0;

            foreach (MethodInfo method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

                if (buttonAttribute == null)
                    continue;

                buttons.Add(new DebugButton(method, buttonAttribute));
            }

            _buttonGroups = buttons.GroupBy(button =>
                {
                    var attribute = button.ButtonAttribute;
                    var hasRow = attribute.HasRow;
                    return hasRow ? attribute.Row : $"__{rowNumber++}";
                }).ToList();
        }

        public void DrawButtons(IEnumerable<object> targets)
        {
            foreach (var buttonGroup in _buttonGroups)
            {
                if(buttonGroup.Any())
                {
                    var space = buttonGroup.First().ButtonAttribute.Space;
                    if(space != 0) EditorGUILayout.Space(space);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    foreach (var button in buttonGroup)
                    {
                        button.Draw(targets);
                    }
                }
            }
        }
    }
}

#endif