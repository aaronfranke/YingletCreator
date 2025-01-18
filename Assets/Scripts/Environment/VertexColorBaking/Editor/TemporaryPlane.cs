using System;
using UnityEngine;

public class TemporaryPlane : IDisposable
{
    private GameObject _plane;

    public TemporaryPlane(Transform root)
    {
        _plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _plane.transform.parent = root;
        _plane.transform.position = root.transform.position + Vector3.down * 1;
        _plane.transform.localScale = Vector3.one * 100;

    }
    public void Dispose()
    {
        GameObject.DestroyImmediate(_plane);
    }
}
