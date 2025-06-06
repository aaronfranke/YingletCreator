using Reactivity;

public interface IHoverable
{
	IReadOnlyObservable<bool> Hovered { get; }
}
