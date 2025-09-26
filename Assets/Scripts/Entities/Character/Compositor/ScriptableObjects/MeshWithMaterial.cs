using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{

    [CreateAssetMenu(fileName = "MeshWithMaterial", menuName = "Scriptable Objects/Character Compositor/MeshWithMaterial")]
    public class MeshWithMaterial : ScriptableObject, ITaggableCharacterElement
    {
        [SerializeField] GameObject _skinnedMeshRendererPrefab;
        [SerializeField] MaterialDescription _materialWithDescription;
        [SerializeField] CharacterElementTag[] _tags;
        [SerializeField] CharacterBone _boneToAttachTo;

        public GameObject SkinnedMeshRendererPrefab => _skinnedMeshRendererPrefab;
        public MaterialDescription MaterialDescription => _materialWithDescription;
        public IEnumerable<CharacterElementTag> Tags => _tags;
        public CharacterBone BoneToAttachTo => _boneToAttachTo;
    }
}
