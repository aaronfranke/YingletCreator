using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "ModDefinition", menuName = "Scriptable Objects/ModDefinition")]
public class ModDefinition : ScriptableObject, IHasUniqueAssetId
{
	public const string ModExtension = ".yingmod";

	[SerializeField, HideInInspector] string _uniqueAssetId;
	public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

	[SerializeField] string _title = "Replace Me";
	public string Title => _title;

	[SerializeField] string _shortDescription = "Replace Me";
	public string ShortDescription => _shortDescription;

	[SerializeField] string _author = "Replace Me";
	public string Author => _author;

	[SerializeField] Sprite _icon = null;
	public Sprite Icon => _icon;

	[SerializeField, HideInInspector] ResourceLookupTable _table;
	public ResourceLookupTable Table => _table;

	public bool IsBuiltInMod => name == "BuiltInAssetsMod";

	[SerializeField, HideInInspector] string[] _presetYings;
	public IEnumerable<string> PresetYings => _presetYings ?? Enumerable.Empty<string>();


	[SerializeField, HideInInspector] ulong _steamWorkshopId = 0;
	[SerializeField, HideInInspector] string _steamWorkshopUniqueId; // In-case someone copy-pasted this mod-definition, we need to know if the workshop ID was generated for this or not
	public ulong SteamWorkshopId
	{
		get
		{
			if (!_steamWorkshopUniqueId.Equals(_uniqueAssetId))
			{
				return 0;
			}
			return _steamWorkshopId;
		}
		set
		{
			_steamWorkshopUniqueId = _uniqueAssetId;
			_steamWorkshopId = value;
		}
	}



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
