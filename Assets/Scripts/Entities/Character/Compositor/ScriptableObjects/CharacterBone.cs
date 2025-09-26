using UnityEngine;


namespace Character.Compositor
{
    [CreateAssetMenu(fileName = "CharacterBone", menuName = "Scriptable Objects/Character Compositor/CharacterBone")]
    public class CharacterBone : ScriptableObject
    {
        [SerializeField] string _boneName;
        public string BoneName => _boneName;
    }
}
