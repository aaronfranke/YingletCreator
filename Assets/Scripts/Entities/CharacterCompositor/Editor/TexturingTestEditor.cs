using UnityEngine;
using UnityEditor;

namespace CharacterCompositor
{

    [CustomEditor(typeof(TexturingTest))]
    public class TexturingTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TexturingTest myScript = (TexturingTest)target;
            if (GUILayout.Button("Update Material"))
            {
                myScript.UpdateMaterial();
            }

            if (GUILayout.Button("Revert Material"))
            {
                myScript.RevertMaterial();
            }
        }
    }

}