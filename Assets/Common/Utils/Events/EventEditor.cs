namespace Common.Utils.Events
{
    // Editor namespace breaks compilation for WebGL
    #if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
    public class EventEditor : Editor
    {
        /// <summary>
        /// When inspecting an GameEvent, shows a button to raise the event for easy testing of events without triggering
        /// their conditions.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var eventToInspect = target as GameEvent;
            if (GUILayout.Button("Raise"))
            {
                eventToInspect.Raise();
            }
        }
    }
    #endif
}