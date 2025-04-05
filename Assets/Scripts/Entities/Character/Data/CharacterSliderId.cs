using UnityEngine;

namespace Character.Data
{
    // Pretty much just a stub to have a unique ID
    [CreateAssetMenu(fileName = "Slider", menuName = "Scriptable Objects/Character Data/CharacterSliderId")]
    public class CharacterSliderId : ScriptableObject, IHasUniqueAssetId
    {
        [SerializeField, HideInInspector] string _uniqueAssetId;
        public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }
    }
}