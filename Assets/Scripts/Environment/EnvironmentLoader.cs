using UnityEngine;


/// <summary>
/// Pretty small for now; will expand this later
/// </summary>
public class EnvironmentLoader : MonoBehaviour
{
    [SerializeField] GameObject _roomPrefab;

    void Start()
    {
        Instantiate(_roomPrefab, this.transform);
    }
}
