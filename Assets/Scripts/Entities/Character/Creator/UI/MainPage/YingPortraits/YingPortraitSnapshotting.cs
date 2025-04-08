using Character.Creator;
using Character.Creator.UI;
using Reactivity;
using Snapshotter;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class YingPortraitSnapshotting : ReactiveBehaviour
{
    [SerializeField] SnapshotterReferences _references;
    [SerializeField] SnapshotterCameraPosition _cameraPosition;
    private RawImage _image;
    private IYingPortraitReference _reference;
    private RenderTexture _rt;

    private void Awake()
    {
        _image = this.GetComponent<RawImage>();
        _reference = GetComponentInParent<IYingPortraitReference>();
    }
    void Start()
    {
        _image.texture = null;
        _image.enabled = false;

        AddReflector(Reflect);
    }

    private void Reflect()
    {
        var cachedData = _reference.Reference.CachedData; // Not actually used; just for reflection
        RunThrottled(Snapshot);
    }

    void Snapshot()
    {
        var observableData = new ObservableCustomizationData(_reference.Reference.CachedData);
        _rt = SnapshotterUtils.Snapshot(_references, new SnapshotterParams(_cameraPosition, observableData));
        _image.texture = _rt;
        _image.enabled = true;

        // Encode to PNG
        //string OutputPath = $"Assets/Scripts/Entities/Snapshotter/_TestGenerated{this.GetInstanceID()}.png";
        //RenderTexture.active = _rt;
        //Texture2D tex = new Texture2D(_rt.width, _rt.height, TextureFormat.RGBA32, false);
        //tex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0);
        //tex.Apply();
        //RenderTexture.active = null;
        //byte[] pngData = tex.EncodeToPNG();
        //System.IO.File.WriteAllBytes(OutputPath, pngData);
        //Debug.Log("Saved PNG to: " + OutputPath);
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        _image.texture = null;
        _rt?.Release();
    }

    static Coroutine currentChain;
    void RunThrottled(Action action)
    {
        IEnumerator Chain()
        {
            // Wait until the current chain is done
            if (currentChain != null)
                yield return currentChain;

            yield return null;

            action();

            currentChain = null;
        }

        currentChain = this.StartCoroutine(Chain());
    }
}
