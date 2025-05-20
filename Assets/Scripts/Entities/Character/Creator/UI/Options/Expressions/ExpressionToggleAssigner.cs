using Character.Data;
using UnityEngine;

namespace Character.Creator.UI
{
	internal interface IExpressionToggleAssigner
	{
		public int Value { get; set; }
	}

	internal class ExpressionToggleAssigner : MonoBehaviour, IExpressionToggleAssigner
	{
		[SerializeField] CharacterIntId _intId;
		private ICustomizationSelectedDataRepository _dataRepo;

		void Awake()
		{
			_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
		}

		public int Value
		{
			get
			{
				return _dataRepo.GetInt(_intId);

			}
			set
			{
				_dataRepo.SetInt(_intId, value);
			}
		}
	}
}
