using Character.Creator;
using Snapshotter;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class YingPortraitSnapshotting : MonoBehaviour
{
    [SerializeField] SnapshotterReferences _references;
    [SerializeField] SnapshotterCameraPosition _cameraPosition;
    private RawImage _image;
    private ICustomizationSelectedDataRepository _dataRepository;
    private RenderTexture _rt;

    private void Awake()
    {
        _image = this.GetComponent<RawImage>();
        _dataRepository = GetComponentInParent<ICustomizationSelectedDataRepository>();
    }
    void Start()
    {
        _image.texture = null;
        _image.enabled = false;
        StartCoroutine(WaitAndThenSnapshot());
    }

    IEnumerator WaitAndThenSnapshot()
    {
        // TODO: Wait for longer based on some scheduler
        yield return null;
        yield return null;

        _rt = SnapshotterUtils.Snapshot(_references, new SnapshotterParams(_cameraPosition, _dataRepository.CustomizationData));
        _image.texture = _rt;
        _image.enabled = true;
    }

    private void OnDestroy()
    {
        _image.texture = null;
        _rt?.Release();
    }
}
