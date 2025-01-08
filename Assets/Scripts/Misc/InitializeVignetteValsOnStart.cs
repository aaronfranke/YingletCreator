using UnityEngine;

public class InitializeVignetteValsOnStart : MonoBehaviour
{
    [SerializeField] float DistanceVisible;
    [SerializeField] float DistanceBlackStart;
    [SerializeField] float DistanceBlackEnd;

    void Start()
    {
        Apply();
    }

    void OnValidate() // Applies it in editor
    {
        Apply();
    }

    void Apply()
    {
        // y = mx + b
        // 1 = m(DistanceVisible) + b
        // 0 = m(DistanceBlackStart) + b
        // (nerd shit math to solve this, thanks chatgpt)

        float diff = DistanceVisible - DistanceBlackStart;
        float m = 1 / diff;
        float b = -DistanceBlackStart / diff;

        Shader.SetGlobalFloat("_VignetteSlope", m);
        Shader.SetGlobalFloat("_VignetteOffset", b);
        Shader.SetGlobalFloat("_VignetteRepeatDistance", DistanceBlackEnd);
    }
}
