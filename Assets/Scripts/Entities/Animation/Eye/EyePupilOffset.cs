using System.Linq;
using UnityEngine;

public interface IEyeOffsetProvider
{
	Vector2 Offset { get; }
	bool enabled { get; }
}

public class EyePupilOffset : MonoBehaviour
{
	private IEyeGatherer _eyeGatherer;
	private IEyeOffsetProvider[] _offsetProviders;
	static readonly int PUPIL_OFFSET_X_PROP_ID = Shader.PropertyToID("_PupilOffsetX");
	static readonly int PUPIL_OFFSET_Y_PROP_ID = Shader.PropertyToID("_PupilOffsetY");

	// The pupil stars centered so it can be scaled. Move it to the default eye pos
	static readonly Vector2 InherentOffset = new Vector2(.094f, 0.151f);

	void Awake()
	{
		_eyeGatherer = this.GetComponentInParent<IEyeGatherer>();
		_offsetProviders = this.GetComponents<IEyeOffsetProvider>().Where(c => c.enabled).ToArray();
		Update();
	}


	void Update()
	{
		Vector2 combinedOffset = Vector2.zero;

		foreach (var offsetProvider in _offsetProviders)
		{
			combinedOffset += offsetProvider.Offset;
		}
		var offsetLeft = new Vector2(combinedOffset.x, combinedOffset.y) + InherentOffset;
		var offsetRight = new Vector2(-combinedOffset.x, combinedOffset.y) + InherentOffset;
		SetMaterial(_eyeGatherer.EyeMaterials[0], offsetLeft);
		SetMaterial(_eyeGatherer.EyeMaterials[1], offsetRight);

		void SetMaterial(Material eyeMaterial, Vector2 offset)
		{
			eyeMaterial.SetFloat(PUPIL_OFFSET_X_PROP_ID, offset.x);
			eyeMaterial.SetFloat(PUPIL_OFFSET_Y_PROP_ID, offset.y);
		}
	}
}
