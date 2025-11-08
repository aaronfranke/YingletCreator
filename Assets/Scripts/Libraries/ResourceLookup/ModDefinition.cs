using UnityEngine;


[CreateAssetMenu(fileName = "ModDefinition", menuName = "Scriptable Objects/ModDefinition")]
public class ModDefinition : ScriptableObject, IHasUniqueAssetId
{
	public const string ModExtension = ".yingmod";

	[SerializeField, HideInInspector] string _uniqueAssetId;
	public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

	[SerializeField] string _modDisplayTitle = "Replace Me";
	public string ModDisplayTitle => _modDisplayTitle;

	[SerializeField, HideInInspector] ResourceLookupTable _table;

#if UNITY_EDITOR

	public void EditorSetTable(ResourceLookupTable table)
	{
		_table = table;
		UnityEditor.EditorUtility.SetDirty(this);
		UnityEditor.AssetDatabase.SaveAssets();
	}
#endif

}
