using System;
using UnityEngine;

namespace Character.Creator.UI
{

	internal interface IPoseYingletClicking
	{
		event Action<PoseYing> OnPoseYingChanged;
	}
	internal class PoseYingClicking : MonoBehaviour, IPoseYingletClicking
	{
		private IPoseData _poseData;
		private IHoveredPoseYingProvider _hoveredProvider;
		private IUiHoverManager _uiHoverManager;
		private IColliderHoverManager _colliderHoverManager;

		public event Action<PoseYing> OnPoseYingChanged;

		private void Awake()
		{
			_poseData = this.GetComponent<IPoseData>();
			_hoveredProvider = this.GetComponent<IHoveredPoseYingProvider>();
			_uiHoverManager = Singletons.GetSingleton<IUiHoverManager>();
			_colliderHoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		}
		private void Update()
		{
			bool lmbClicked = Input.GetMouseButtonDown(0);
			if (!lmbClicked) return;

			// Are we hovering over a ying
			var currentlyHoveredYing = _hoveredProvider.HoveredPoseYing;
			if (currentlyHoveredYing != null)
			{
				if (currentlyHoveredYing.Reference == _poseData.CurrentlyEditing?.Reference)
				{
					// Ying is already selected, so let's toggle it off
					SetEditing(null);
				}
				else
				{
					// Ying isn't selected, so let's select it
					SetEditing(currentlyHoveredYing);
				}
				return;
			}

			bool hoveringAnything = _uiHoverManager.HoveringUi || _colliderHoverManager.CurrentlyHovered != null;
			if (!hoveringAnything)
			{
				// Hovering over nothing, so let's clear out the selected ying
				SetEditing(null);
			}
		}

		void SetEditing(PoseYing poseYing)
		{
			_poseData.CurrentlyEditing = poseYing;
			OnPoseYingChanged?.Invoke(poseYing);
		}

	}
}
