using Character.Creator;
using Character.Creator.UI;
using Snapshotter;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class YingPortraitSnapshotting : MonoBehaviour
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
        RunThrottled(Snapshot);
    }

    void Snapshot()
    {
        // Optimization opportunity; could re-use selected if it's the same reference
        var observableData = new ObservableCustomizationData(_reference.Reference.CachedData);
        _rt = SnapshotterUtils.Snapshot(_references, new SnapshotterParams(_cameraPosition, observableData));
        _image.texture = _rt;
        _image.enabled = true;
    }

    private void OnDestroy()
    {
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
