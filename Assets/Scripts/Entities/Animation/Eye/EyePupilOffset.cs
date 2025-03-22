using System;
using System.Collections;
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


    void Awake()
    {
        _eyeGatherer = this.GetComponent<IEyeGatherer>();
        _offsetProviders = this.GetComponents<IEyeOffsetProvider>().Where(c => c.enabled).ToArray();
    }


    void Update()
    {
        Vector2 combinedOffset = Vector2.zero;
        foreach (var offsetProvider in _offsetProviders)
        {
            combinedOffset += offsetProvider.Offset;
        }
        SetMaterial(_eyeGatherer.EyeMaterials[0], combinedOffset.x, combinedOffset.y);
        SetMaterial(_eyeGatherer.EyeMaterials[1], -combinedOffset.x, combinedOffset.y);

        void SetMaterial(Material eyeMaterial, float x, float y)
        {
            eyeMaterial.SetFloat(PUPIL_OFFSET_X_PROP_ID, x);
            eyeMaterial.SetFloat(PUPIL_OFFSET_Y_PROP_ID, y);
        }
    }
}
