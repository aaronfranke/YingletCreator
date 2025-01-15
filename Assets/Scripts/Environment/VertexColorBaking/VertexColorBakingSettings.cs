using UnityEngine;

[System.Serializable]
public class VertexColorBakingSettings
{
    public const int SamplingTextureSize = 16;

    public const float SamplingBias = 0.001f;
    [Range(0.01f, 100.0f)]
    public float SamplingDistance = 10.0f;
    [Range(45.0f, 145.0f)]
    public float SamplingCameraFOV = 135.0f;

    [ColorUsage(showAlpha: true, hdr: true)]
    public Color SkyColor = Color.white;
    public Color ShadowColor = Color.black;

    public AnimationCurve PointLightFalloff = AnimationCurve.Linear(0, 1, 1, 0);
}
