using UnityEngine;

namespace Character.Creator.UI
{
	internal class PoseYingClicking : MonoBehaviour
	{
		private IPoseData _poseData;
		private IHoveredPoseYingProvider _hoveredProvider;

		private void Awake()
		{
			_poseData = this.GetComponent<IPoseData>();
			_hoveredProvider = this.GetComponent<IHoveredPoseYingProvider>();
		}
		private void Update()
		{
			bool lmbClicked = Input.GetMouseButtonDown(0);
			if (!lmbClicked) return;

			var currentlyHovered = _hoveredProvider.HoveredPoseYing;
			if (currentlyHovered == null) return;

			_poseData.CurrentlyEditing = currentlyHovered.Reference;
		}
	}
}
