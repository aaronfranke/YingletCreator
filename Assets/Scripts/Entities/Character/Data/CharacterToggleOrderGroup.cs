using UnityEngine;

namespace Character.Data
{
	/// <summary>
	/// This group determines how items are displayed among each other in the UI
	/// Note that individual toggles assign themselves to group and specify their order in it
	/// This is to make it easier to create new toggles without having to find some master object,
	/// as well as to potentially support moddability in the future
	/// </summary>
	[CreateAssetMenu(fileName = "Group", menuName = "Scriptable Objects/Character Data/CharacterToggleOrderGroup")]
	public class CharacterToggleOrderGroup : ScriptableObject
	{
	}
}