using UnityEngine.UI;

public class InvisibleRaycastTarget : Graphic
{
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear(); // Don't render anything
	}
}