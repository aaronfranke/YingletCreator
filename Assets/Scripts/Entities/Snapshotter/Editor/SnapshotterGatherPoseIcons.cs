using Character.Data;
using System.Linq;
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
			poses = poses.Where(p => !p.name.StartsWith("(UNUSED)")).ToArray();
			SnapshotToSpriteSheetUtils.SnapshotToTexAndApply(poses, OutputPath);

		}
	}
}