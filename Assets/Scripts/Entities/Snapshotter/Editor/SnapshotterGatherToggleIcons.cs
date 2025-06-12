using Character.Data;
using UnityEditor;
using UnityEngine;

namespace Snapshotter
{
	public sealed class SnapshotterGatherToggleIcons
	{
		public const string OutputPath = "Assets/Scripts/Entities/Snapshotter/Generated/ToggleIcons.png";

		[MenuItem("Custom/Snapshotter/Generate Toggle Icons")]
		public static void TestSnapshotter()
		{

			var toggles = GetAllToggles();
			SnapshotToSpriteSheetUtils.SnapshotToTexAndApply(toggles, OutputPath);

		}
		static CharacterToggleId[] GetAllToggles()
		{
			return Resources.LoadAll<CharacterToggleId>(nameof(CharacterToggleId));
		}
	}
}