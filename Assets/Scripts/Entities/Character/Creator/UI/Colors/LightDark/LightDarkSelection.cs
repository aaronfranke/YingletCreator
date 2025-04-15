using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface ILightDarkSelection
	{
		bool Light { get; }
		void Toggle();
	}
	public class LightDarkSelection : MonoBehaviour, ILightDarkSelection
	{
		Observable<bool> _light = new(true);
		public bool Light => _light.Val;

		public void Toggle()
		{
			_light.Val = !_light.Val;
		}
	}
}