using Reactivity;
using TMPro;
using UnityEngine;

public class YingPortraitGrid : ReactiveBehaviour
{
    [SerializeField] GameObject _yingPortraitPrefab;
    string[] _debugYingNames = new[] { "Kass", "Vizlet", "Pekkit" };
    EnumerableReflector<string, GameObject> _yingEnumerableReflector;

    private void Start()
    {
        _yingEnumerableReflector = new(CreatePortrait, RemovePortrait);
        AddReflector(ReflectYings);
    }

    private GameObject CreatePortrait(string name)
    {
        var go = Instantiate(_yingPortraitPrefab, this.transform);
        go.GetComponentInChildren<TextMeshProUGUI>().text = name;
        return go;
    }
    private void RemovePortrait(GameObject obj)
    {
        Destroy(obj);
    }

    void ReflectYings()
    {
        _yingEnumerableReflector.Enumerate(_debugYingNames);
    }
}
