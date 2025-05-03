using UnityEditor;
using UnityEngine;

namespace Character.Compositor
{
	[CustomPropertyDrawer(typeof(MixTextureOrderGroup))]
	public class MixTextureOrderGroupDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty nameProp = property.FindPropertyRelative("_name");

			if (nameProp != null && !string.IsNullOrEmpty(nameProp.stringValue))
			{
				label.text = nameProp.stringValue;
			}
			else
			{
				label.text = "Unnamed Group";
			}

			EditorGUI.PropertyField(position, property, label, true);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
	}
}