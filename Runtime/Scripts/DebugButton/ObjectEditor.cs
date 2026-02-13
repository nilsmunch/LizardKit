using UnityEditor;

#if UNITY_EDITOR
namespace LizardKit.DebugButton.Editor
{
    using Object = UnityEngine.Object;

    [CustomEditor(typeof(Object), true), CanEditMultipleObjects]
    internal class ObjectEditor : UnityEditor.Editor
    {
        private ButtonsDrawer _buttonsDrawer;

        private void OnEnable()
        {
            _buttonsDrawer = new ButtonsDrawer(target);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            _buttonsDrawer.DrawButtons(targets);
        }
    }
}
#endif