using Character.Data;
using UnityEditor;
using UnityEngine;

namespace Snapshotter
{
	public sealed class SnapshotterGatherPoseIcons
	{
		public const string OutputPath = "Assets/Scripts/Entities/Snapshotter/Generated/PoseIcons.png";

		[MenuItem("Custom/Snapshotter/Generate Pose Icons")]
		public static void GeneratePoseIcons()
		{

			var poses = Resources.LoadAll<PoseId>(nameof(PoseId));
			SnapshotToSpriteSheetUtils.SnapshotToTexAndApply(poses, OutputPath);

		}
	}
}