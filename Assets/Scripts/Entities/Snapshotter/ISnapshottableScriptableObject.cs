using Character.Data;

namespace Snapshotter
{
	/// <summary>
	/// A scriptable object that can be applied to a ying and generate an icon sheet for
	/// </summary>
	public interface ISnapshottableScriptableObject : IHasUniqueAssetId
	{
		string name { get; }
		string DisplayName { get; }
		public CharacterTogglePreviewData Preview { get; }
	}
}