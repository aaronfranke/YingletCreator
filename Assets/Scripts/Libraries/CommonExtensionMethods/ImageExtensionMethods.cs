using UnityEngine.UI;

public static class ImageExtensionMethods
{
	public static void SuperDirtyMaterial(this Image image)
	{
		// Neither _image.SetMaterialDirty() nor _image.SetAllDirty(); seem to force this to update, so do this hack
		image.enabled = false;
		image.enabled = true;
	}
}
