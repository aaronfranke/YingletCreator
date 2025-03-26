namespace Reactivity.Implementation
{
	public interface IDependent
	{
		void AddNotifier(Notifier notifier);
		void SetDirty();
	}
}