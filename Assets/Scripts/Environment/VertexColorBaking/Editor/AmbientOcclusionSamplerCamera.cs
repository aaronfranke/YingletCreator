using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Generates a camera and render texture for the lifetime of this object which can be used to sample ambient occlusion values
/// </summary>
public sealed class AmbientOcclusionSamplerCamera : IDisposable
{
    private readonly RenderTexture _renderTexture;
    private readonly GameObject _cameraGO;
    private readonly Camera _camera;
    private readonly Texture2D _texture2D;

    public AmbientOcclusionSamplerCamera(VertexColorBakingSettings settings)
    {
        
        _texture2D = new Texture2D(VertexColorBakingSettings.SamplingTextureSize, VertexColorBakingSettings.SamplingTextureSize, TextureFormat.RGBA64, false);
        _renderTexture = new RenderTexture(VertexColorBakingSettings.SamplingTextureSize, VertexColorBakingSettings.SamplingTextureSize, 16, RenderTextureFormat.ARGB64);

        _cameraGO = new GameObject("VertexColorBakingCamera");
        _camera = _cameraGO.AddComponent<Camera>();
        _camera.targetTexture = _renderTexture;
        _camera.renderingPath = RenderingPath.Forward;
        // _camera.pixelRect = new Rect(0, 0, VertexColorBakingSettings.SamplingTextureSize, VertexColorBakingSettings.SamplingTextureSize);
        _camera.aspect = 1.0f;
        _camera.nearClipPlane = VertexColorBakingSettings.SamplingBias;
        _camera.farClipPlane = settings.SamplingDistance;
        _camera.fieldOfView = Mathf.Clamp(settings.SamplingCameraFOV, 5, 160);
        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = Color.white;
        _camera.cullingMask = Tools.visibleLayers; // May need to be adjusted in the future
        _camera.enabled = false;
        var shader = Shader.Find("Hidden/VertexColorBakingAmbientOcclusion");
        _camera.SetReplacementShader(shader, "");

    }
    public void Dispose()
    {
        _camera.targetTexture = null;
        GameObject.DestroyImmediate(_texture2D);
        GameObject.DestroyImmediate(_renderTexture);
        GameObject.DestroyImmediate(_cameraGO);
    }

    public float SampleAO(Vector3 position, Vector3 normal)
    {
        _cameraGO.transform.position = position + normal * VertexColorBakingSettings.SamplingBias;
        _cameraGO.transform.LookAt(position + normal);
        _camera.Render();

        RenderTexture.active = _renderTexture;

        _texture2D.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        _texture2D.Apply();
        var color = GetAverageColor(_texture2D);
        
        RenderTexture.active = null;
        return color.r;
    }

    static Color GetAverageColor(Texture2D texture)
    {
        // Get all pixels of the texture
        Color[] pixels = texture.GetPixels();

        // Initialize variables to store color components
        float totalR = 0f, totalG = 0f, totalB = 0f, totalA = 0f;

        // Sum up the color components
        foreach (Color pixel in pixels)
        {
            totalR += pixel.r;
            totalG += pixel.g;
            totalB += pixel.b;
            totalA += pixel.a;
        }

        // Calculate the average
        int totalPixels = pixels.Length;
        return new Color(totalR / totalPixels, totalG / totalPixels, totalB / totalPixels, totalA / totalPixels);
    }
}
