using UnityEditor;

namespace Bg.StateMachine.Editor
{
    public static class MenuItems
    {
        [MenuItem("Window/BG StateMachine/State Machine Graph")]
        private static void OpenStateMachineGraphWindow()
        {
            EditorWindow window = (EditorWindow) UnityEditor.EditorWindow.GetWindow(typeof(EditorWindow), false, "State Machine Graph", true);
            window.Show();
        }
    }
}