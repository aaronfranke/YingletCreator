namespace Reactivity
{

	public delegate void ObservableElementDestroyed();

	/// Completely unrelated to IDestroyable
	/// This is for items that may be observed on that can be "Destroyed"
	/// We need to update the Observable value to contain null when this happens 
	/// Currently this doesn't support any arrays, should it
	public interface IDestroyableObservableElement
	{
		event ObservableElementDestroyed Destroyed;
	}
}