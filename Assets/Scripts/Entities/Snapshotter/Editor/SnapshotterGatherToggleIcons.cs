using UnityEditor;

namespace Snapshotter
{
	public sealed class SnapshotterGatherToggleIcons
	{
		[MenuItem("Custom/Snapshotter/Generate Built-In Toggle Icons")]
		public static void GenerateToggleIcons()
		{
			SnapshotToSpriteSheetUtils.GenerateToggleIcons(ModDefinitionUtils.GetBuiltinModDefinition());
			SnapshotToSpriteSheetUtils.UpdateIconsInScene();
		}
	}
}