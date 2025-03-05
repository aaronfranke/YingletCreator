using UnityEngine;
using UnityEditor;

namespace CharacterCompositor
{

    [CustomEditor(typeof(CompositedYinglet))]
    public class CompositedYingletEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CompositedYinglet myScript = (CompositedYinglet)target;
            if (GUILayout.Button("Composite"))
            {
                myScript.Composite();
            }

            if (GUILayout.Button("Clear"))
            {
                myScript.Clear();
            }
        }
    }

}