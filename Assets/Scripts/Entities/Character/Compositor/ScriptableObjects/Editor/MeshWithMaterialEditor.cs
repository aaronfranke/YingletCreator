using UnityEditor;
using UnityEngine;

namespace Character.Compositor
{
	[CustomEditor(typeof(MeshWithMaterial))]
	public class MeshWithMaterialEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Select in Project"))
			{
				Selection.activeObject = target;
				EditorGUIUtility.PingObject(target);
			}
		}
	}
}