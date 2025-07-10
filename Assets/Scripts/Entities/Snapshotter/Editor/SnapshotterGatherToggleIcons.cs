using Character.Data;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Snapshotter
{
	public sealed class SnapshotterGatherToggleIcons
	{
		public const string OutputPath = "Assets/Scripts/Entities/Snapshotter/Generated/ToggleIcons.png";

		[MenuItem("Custom/Snapshotter/Generate Toggle Icons")]
		public static void GenerateToggleIcons()
		{

			var toggles = Resources.LoadAll<CharacterToggleId>(nameof(CharacterToggleId));
			// Filter out toggles that we don't want an icon for
			toggles = toggles.Where(t => !t.Components.Any(c => c is NoToggleIcon)).ToArray();
			SnapshotToSpriteSheetUtils.SnapshotToTexAndApply(toggles, OutputPath);

		}
	}
}