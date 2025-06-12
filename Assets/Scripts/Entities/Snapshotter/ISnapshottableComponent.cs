namespace Snapshotter
{
	/// <summary>
	/// With the snapshotter, we spawn up and immediately remove an object
	/// This means that operations like applying bones which happen per-frame won't be setup
	/// This gives us a hook to run those operations at any time
	/// </summary>
	public interface ISnapshottableComponent
	{
		void PrepareForSnapshot();
	}
}