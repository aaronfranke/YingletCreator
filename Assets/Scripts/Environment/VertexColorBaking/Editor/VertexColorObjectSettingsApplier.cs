using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Ensures that all objects are ready for baking based on the settings on individual objects
/// Settings are applied for the lifetime of this object
/// </summary>
public class VertexColorObjectSettingsApplier : IDisposable
{
    private readonly IEnumerable<MeshRenderer> _disabledMeshRenderers;

    public VertexColorObjectSettingsApplier(IEnumerable<Transform> objects)
    {
        var settings = objects
            .Select(o => o.GetComponent<VertexColorObjectSettings>())
            .Where(c => c != null)
            .ToArray();

        var disabledMeshRenders = new List<MeshRenderer>();
        foreach (var setting in settings)
        {
            if (setting.Obfuscate == false)
            {
                var meshRender = setting.GetComponent<MeshRenderer>();
                if (meshRender == null)
                {
                    Debug.LogWarning("Settings called for no obfuscation, but the object didn't even have a MeshRenderer to begin with");
                    continue;
                }
                if (meshRender.enabled == false)
                {
                    Debug.LogWarning("Settings called for no obfuscation, but the object didn't even have its mesh render enabled to begin with");
                    continue;
                }
                meshRender.enabled = false;
                disabledMeshRenders.Add(meshRender);
            }
        }
        _disabledMeshRenderers = disabledMeshRenders;

    }

    public void Dispose()
    {
        foreach (var meshRender in _disabledMeshRenderers)
        {
            meshRender.enabled = true;
        }
    }
}
