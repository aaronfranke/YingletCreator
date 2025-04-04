using Character.Creator;
using Reactivity;
using TMPro;
using UnityEngine;

public class YingPortraitGrid : ReactiveBehaviour
{
    [SerializeField] GameObject _yingPortraitPrefab;
    [SerializeField] CustomizationYingletGroup _group;
    private ICustomizationYingletRepository _yingletRepository;
    private int _initialChildren;
    EnumerableReflector<CachedYingletReference, GameObject> _yingEnumerableReflector;

    private void Awake()
    {
        _yingletRepository = this.GetComponentInParent<ICustomizationYingletRepository>();
        _initialChildren = this.transform.childCount;
    }
    private void Start()
    {
        _yingEnumerableReflector = new(CreatePortrait, RemovePortrait);
        AddReflector(ReflectYings);
    }

    private GameObject CreatePortrait(CachedYingletReference yingReference)
    {
        var go = Instantiate(_yingPortraitPrefab, this.transform);

        // Place it right before the pre-existing "Create New" button
        if (_initialChildren != 0)
        {
            int siblingCount = this.transform.childCount;
            int targetIndex = Mathf.Max(0, siblingCount - 1 - _initialChildren);
            go.transform.SetSiblingIndex(targetIndex);
        }

        go.GetComponentInChildren<TextMeshProUGUI>().text = yingReference.Path;
        return go;
    }
    private void RemovePortrait(GameObject obj)
    {
        Destroy(obj);
    }

    void ReflectYings()
    {
        _yingEnumerableReflector.Enumerate(_yingletRepository.GetYinglets(_group));
    }
}
