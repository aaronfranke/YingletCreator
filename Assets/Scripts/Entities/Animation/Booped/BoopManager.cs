using System;
using UnityEngine;

public interface IBoopManager
{
    event Action OnBoop;
}
public class BoopManager : MonoBehaviour, IBoopManager
{
    public event Action OnBoop;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnBoop?.Invoke();
        }
    }
}
