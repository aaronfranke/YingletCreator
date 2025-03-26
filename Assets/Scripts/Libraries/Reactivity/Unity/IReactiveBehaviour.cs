using System;
using Reactivity;
using UnityEngine;

public interface IReactiveBehaviour : IDestroyableObservableElement
{
	void AddReflector(Action action);
	Computed<T> CreateComputed<T>(Func<T> func);
	// void Watch<T>(Observable<T> observable, Action<T,T> action);

	void OnDestroy();

	GameObject gameObject { get; }
}