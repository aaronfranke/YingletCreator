using Character.Data;
using UnityEngine;


[CreateAssetMenu(fileName = "ModDefinition", menuName = "Scriptable Objects/ModDefinition")]
public class ModDefinition : ScriptableObject, IHasUniqueAssetId
{
	[SerializeField, HideInInspector] string _uniqueAssetId;
	public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

	[SerializeField] string _modDisplayTitle = "Replace Me";
	public string ModDisplayTitle => _modDisplayTitle;

	[SerializeField] CharacterToggleId[] _toggles;
	public CharacterToggleId[] Toggles => _toggles;

}
