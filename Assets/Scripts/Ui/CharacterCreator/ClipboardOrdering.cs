using UnityEngine;

public interface IClipboardOrdering
{
    void SendToFront(Transform transform);
    void SendToBack(Transform transform);
}
public class ClipboardOrdering : MonoBehaviour, IClipboardOrdering
{
    [SerializeField] Transform[] _alwaysOnTopItems;
    public void SendToFront(Transform transform)
    {
        transform.SetSiblingIndex(this.transform.childCount - _alwaysOnTopItems.Length - 1);
    }

    public void SendToBack(Transform transform)
    {
        transform.SetAsFirstSibling();
    }
}
