using System;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Caches the render settings, and then disables them for the lifetime of this object
/// </summary>
public sealed class RenderSettingDisabler : IDisposable
{
    private readonly RenderPipelineAsset _defaultRenderPipeline;
    private readonly RenderPipelineAsset _renderPipeline;

    public RenderSettingDisabler()
    {
        _defaultRenderPipeline = GraphicsSettings.defaultRenderPipeline;
        _renderPipeline = QualitySettings.renderPipeline;

        // 1 - Cache graphic and quality settings and then null them
        GraphicsSettings.defaultRenderPipeline = null;
        QualitySettings.renderPipeline = null;
    }
    public void Dispose()
    {
		GraphicsSettings.defaultRenderPipeline = _defaultRenderPipeline;
        QualitySettings.renderPipeline = _renderPipeline;
    }
}
