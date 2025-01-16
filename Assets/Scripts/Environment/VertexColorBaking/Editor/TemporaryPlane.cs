using System;
using UnityEngine;

public class TemporaryPlane : IDisposable
{
    private GameObject _plane;

    public TemporaryPlane(Vector3 rootPos)
    {
        _plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _plane.transform.position = rootPos + Vector3.down * 1;
        _plane.transform.localScale = Vector3.one * 100;

    }
    public void Dispose()
    {
        GameObject.DestroyImmediate(_plane);
    }
}
