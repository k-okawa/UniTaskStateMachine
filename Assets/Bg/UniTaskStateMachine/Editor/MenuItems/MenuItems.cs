using UnityEditor;

namespace Bg.UniTaskStateMachine.Editor
{
    public static class MenuItems
    {
        [MenuItem("Window/BG UniTaskStateMachine/State Machine Graph")]
        private static void OpenStateMachineGraphWindow()
        {
            EditorWindow window = (EditorWindow) UnityEditor.EditorWindow.GetWindow(typeof(EditorWindow), false, "State Machine Graph", true);
            window.Show();
        }
    }
}