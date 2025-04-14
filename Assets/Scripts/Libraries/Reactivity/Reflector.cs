using Reactivity.Implementation;
using System;
using System.Collections.Generic;

namespace Reactivity
{

	public class Reflector : IDependent, IDestroyable
	{
		Action _action;
		readonly IList<Notifier> notifiers = new List<Notifier>();

		public Reflector(Action action)
		{
			_action = action;
			RunAction();
		}

		public void Destroy()
		{
			foreach (var notifier in notifiers)
			{
				notifier.ClearDependent(this);
			}
			notifiers.Clear();
		}

		public void SetDirty()
		{
			foreach (var notifier in notifiers)
			{
				notifier.ClearDependent(this);
			}
			notifiers.Clear();

			// Reflectors should always force get the latest data
			RunAction();
		}

		// Runs the action with dependent adding
		void RunAction()
		{
			var lastDependent = Statics.CurrentDependent;
			Statics.CurrentDependent = this;
			_action();
			Statics.CurrentDependent = lastDependent;
		}

		public void AddNotifier(Notifier notifier)
		{
			if (!notifiers.Contains(notifier))
			{
				notifiers.Add(notifier);
			}
		}
	}
}