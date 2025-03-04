using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class TexturingTest : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer _bodyMeshRenderer;
    [SerializeField] Material _originalMaterial;
    Material _editorMaterial;

    public void UpdateMaterial()
    {
        Assert.IsNotNull(_bodyMeshRenderer);

        int textureSize = 256;
        var renderTexture = new RenderTexture(textureSize, textureSize, 0);
        renderTexture.Create();

        Texture2D whiteTexture = new Texture2D(1, 1);
        whiteTexture.SetPixel(0, 0, Color.white);
        whiteTexture.Apply();

        Graphics.Blit(whiteTexture, renderTexture);

        if (_editorMaterial == null)
        {
            _editorMaterial = new Material(_originalMaterial);
        }

        _bodyMeshRenderer.sharedMaterial = _editorMaterial;
        _editorMaterial.mainTexture = renderTexture;
    }

    public void RevertMaterial()
    {
        _bodyMeshRenderer.sharedMaterial = _originalMaterial;
    }
}
