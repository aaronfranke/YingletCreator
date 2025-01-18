using UnityEngine;

public class VertexColorObjectSettings : MonoBehaviour
{
    [TooltipAttribute("Leave this enabled if you want the geometry to contribute to ambient occlusion. Worth disabling for certain things like plants")] 
    [SerializeField] bool _obfuscate = true;

    public bool Obfuscate => _obfuscate;
}
