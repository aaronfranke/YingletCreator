using System.Collections.Generic;
using System.Linq;
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
	public ResourceLookupTable Table => _table;

	public bool IsBuiltInMod => name == "BuiltInAssetsMod";

	[SerializeField, HideInInspector] string[] _presetYings;
	public IEnumerable<string> PresetYings => _presetYings ?? Enumerable.Empty<string>();

#if UNITY_EDITOR

	public void EditorSetPresetsAndTable(string[] presetYings, ResourceLookupTable table)
	{
		_presetYings = presetYings;
		_table = table;
		UnityEditor.EditorUtility.SetDirty(this);
		UnityEditor.AssetDatabase.SaveAssets();
	}
#endif

}
