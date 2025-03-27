using UnityEngine;

public interface IClipboardOrdering
{
    void SendToFront(Transform transform);
    void SendToBack(Transform transform);
}
public class ClipboardOrdering : MonoBehaviour, IClipboardOrdering
{
    public void SendToFront(Transform transform)
    {
        transform.SetSiblingIndex(this.transform.childCount - 1);
    }

    public void SendToBack(Transform transform)
    {
        transform.SetAsFirstSibling();
    }
}
