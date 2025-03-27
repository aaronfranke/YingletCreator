using UnityEngine;

public interface IClipboardOrdering
{
    void SendToFront(Transform transform, bool isFreeFall);
    void SendToBack(Transform transform);
}
public class ClipboardOrdering : MonoBehaviour, IClipboardOrdering
{
    [SerializeField] RectTransform _normalParent;
    [SerializeField] RectTransform _freefallParent;


    public void SendToFront(Transform transform, bool isFreefall)
    {
        var parent = isFreefall ? _freefallParent : _normalParent;
        transform.SetParent(parent);
        transform.SetSiblingIndex(parent.childCount - 1);
    }

    public void SendToBack(Transform transform)
    {
        transform.SetAsFirstSibling();
    }
}
