using Character.Creator.UI;
using Reactivity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{

	public class MeshGatherer_MaskOnCharacterCreatorScreen : ReactiveBehaviour, IMeshGathererMutator
	{
		[SerializeField] MeshTag _toRemove;
		[SerializeField] ClipboardSelectionType _clipboardType;

		IClipboardSelection _clipboardSelection;
		private Computed<bool> _onPage;

		private void Awake()
		{
			_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();
			_onPage = CreateComputed(ComputeOnPage);
		}

		bool ComputeOnPage()
		{
			if (_clipboardSelection == null)
			{
				return false;
			}
			return _clipboardSelection.Selection.Val == _clipboardType;
		}

		public void Mutate(ref ISet<MeshWithMaterial> meshes)
		{
			if (_onPage.Val)
			{
				var toRemove = meshes.Where(m => m.Tags.Contains(_toRemove)).ToList();
				foreach (var m in toRemove)
				{
					meshes.Remove(m);
				}
			}
		}
	}
}
