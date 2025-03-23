using System.Collections.Generic;
using UnityEngine;

public interface IEyeGatherer
{
    Material[] EyeMaterials { get; }
}

public class EyeGatherer : MonoBehaviour, IEyeGatherer
{
    Material[] _cachedEyes;

    const string EYE_NAME_LEFT = "YingletEye_Left";
    const string EYE_NAME_RIGHT = "YingletEye_Right";

    public Material[] EyeMaterials
    {
        get
        {
            if (_cachedEyes != null) return _cachedEyes;

            var leftEye = GetEyeMaterial(EYE_NAME_LEFT);
            var rightEye = GetEyeMaterial(EYE_NAME_RIGHT);

            if (leftEye != null && rightEye != null)
            {
                _cachedEyes = new[] { leftEye, rightEye };
            }
            return _cachedEyes;
        }
    }

    Material GetEyeMaterial(string name)
    {
        var eyeTransform = TransformUtils.FindChildRecursive(this.transform, name);
        if (eyeTransform == null)
        {
            Debug.LogError($"Failed to find eye {name}");
            return null;
        }
        return eyeTransform.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
    }


}
