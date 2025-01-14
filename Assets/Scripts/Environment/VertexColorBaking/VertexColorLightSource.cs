using UnityEngine;

public class VertexColorLightSource : MonoBehaviour
{
    [ColorUsage(showAlpha: true, hdr: true)]
    public Color Color = Color.white;
    public float Range = 5;
    public float Intensity = 1;

    Color LowAlphaColor
    {
        get
        {
            var color = Color;
            color.a = .3f;
            return color;
        }
    }

    // This is called every frame in the Scene View
    void OnDrawGizmos()
    {
        Gizmos.color = Color; // Set the gizmo color
        Gizmos.DrawSphere(transform.position, .2f); // Draw a filled sphere
    }

    // Optional: Only draw when the GameObject is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = LowAlphaColor;
        Gizmos.DrawWireSphere(transform.position, Range * .75f); // Draw a wireframe sphere
    }
}
