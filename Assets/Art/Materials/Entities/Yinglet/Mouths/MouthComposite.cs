using UnityEngine;

/// <summary>
/// Scriptable object used for combining 1x3 mouth textures together into a single 3x3 texture
/// This is done for two reasons:
///  1) Many mouth variations share certain expressions, especially the muse shape
///  2) It's hard to export the 3x3 textures raw from inkscape, since many clip off the left side
/// </summary>
[CreateAssetMenu(fileName = "MouthComposite", menuName = "Scriptable Objects/Character Compositor/MouthComposite")]
public class MouthComposite : ScriptableObject
{
	public Texture2D Grin;
	public Texture2D Frown;
	public Texture2D Muse;
}

