using UnityEditor;

namespace Snapshotter
{
	public sealed class SnapshotterGatherPoseIcons
	{

		[MenuItem("Custom/Snapshotter/Generate Built-In Pose Icons")]
		public static void GeneratePoseIcons()
		{
			SnapshotToSpriteSheetUtils.GenerateToggleIcons(ModDefinitionUtils.GetBuiltinModDefinition());
		}
	}
}