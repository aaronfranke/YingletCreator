using System;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class ExpressionToggle : MonoBehaviour, IUserToggleEvents
	{
		[SerializeField] Image _icon;

		public event Action<bool> UserToggled;

		public void Setup(Sprite sprite)
		{
			_icon.sprite = sprite;
		}
	}
}