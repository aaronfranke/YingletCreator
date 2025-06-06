using Reactivity;
using UnityEngine;

public interface IHoverable
{
	IReadOnlyObservable<bool> Hovered { get; }

	Transform transform { get; }
}
