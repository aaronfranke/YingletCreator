using CharacterCompositor;
using NUnit.Framework;
using UnityEngine;

namespace CharacterCompositor
{
    public class TexturingTest : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer _bodyMeshRenderer;
        [SerializeField] Material _originalMaterial;

        [SerializeField] Material _compositeColorizeAndMix;

        [SerializeField] Texture2D _bodyTexture;
        [SerializeField] ColorGroup _testColorGroup;

        Material _editorMaterial;

        public void UpdateMaterial()
        {
            Assert.IsNotNull(_bodyMeshRenderer);

            var renderTextures = new DoubleBufferedRenderTexture(256);

            if (_editorMaterial == null)
            {
                _editorMaterial = new Material(_originalMaterial);
            }

            var blitMaterial = new Material(_compositeColorizeAndMix);
            blitMaterial.SetTexture("_MixTex", _bodyTexture);
            blitMaterial.Colorize(_testColorGroup.DefaultColorValues);

            renderTextures.Blit(blitMaterial);

            _editorMaterial.mainTexture = renderTextures.Finalize();
            _bodyMeshRenderer.sharedMaterial = _editorMaterial;
        }

        public void RevertMaterial()
        {
            _bodyMeshRenderer.sharedMaterial = _originalMaterial;
        }
    }
}
