using Snapshotter;
using UnityEngine;

namespace Character.Data
{
	[CreateAssetMenu(fileName = "Pose", menuName = "Scriptable Objects/Character Data/PoseId")]
	public class PoseId : ScriptableObject, IHasUniqueAssetId, ISnapshottableScriptableObject
	{
		[SerializeField, HideInInspector] string _uniqueAssetId;
		public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

		[SerializeField] AnimationClip _clip;
		public AnimationClip Clip => _clip;

		// Filenames can't contain special characters I want for some things
		[SerializeField] string _overrideName;
		public string OverrideName => _overrideName;

		[SerializeField] CharacterTogglePreviewData _preview;
		public CharacterTogglePreviewData Preview => _preview;

		public string DisplayName => string.IsNullOrWhiteSpace(_overrideName) ? name : _overrideName;
	}
}