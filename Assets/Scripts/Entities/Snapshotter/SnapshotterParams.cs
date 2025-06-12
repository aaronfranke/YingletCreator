using Character.Creator;
using UnityEngine;

namespace Snapshotter
{
	public sealed class SnapshotterParams
	{

		public SnapshotterParams(SnapshotterCameraPosition camPos, ObservableCustomizationData data)
		{
			CamPos = camPos; ;
			Data = data;
		}

		public SnapshotterCameraPosition CamPos { get; }
		public ObservableCustomizationData Data { get; }

		/// <summary>
		/// Optional override for the pose the animator will play the first frame of
		/// </summary>
		public AnimationClip Pose { get; set; } = null;
	}
}
