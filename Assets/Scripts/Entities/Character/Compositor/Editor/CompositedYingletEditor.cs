using UnityEngine;
using UnityEditor;

namespace Character.Compositor
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
                EditorUtility.SetDirty(myScript.gameObject);
            }

            if (GUILayout.Button("Clear"))
            {
                myScript.Clear();
                EditorUtility.SetDirty(myScript.gameObject);
            }
        }
    }

}