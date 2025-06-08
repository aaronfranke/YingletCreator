
using System;

public sealed class BasicActionDisposable : IDisposable
{
	private Action _undo;
	public BasicActionDisposable(Action undo)
	{
		_undo = undo;
	}
	public void Dispose()
	{
		_undo();
	}
}
