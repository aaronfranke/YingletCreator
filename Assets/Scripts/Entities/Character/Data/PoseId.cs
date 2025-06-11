using UnityEngine;

namespace Character.Data
{
	[CreateAssetMenu(fileName = "Pose", menuName = "Scriptable Objects/Character Data/PoseId")]
	public class PoseId : ScriptableObject, IHasUniqueAssetId
	{
		[SerializeField, HideInInspector] string _uniqueAssetId;
		public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

		[SerializeField] AnimationClip _clip;
		public AnimationClip Clip => _clip;

		[SerializeField] CharacterTogglePreviewData _preview;
		public CharacterTogglePreviewData Preview => _preview;
	}
}