using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reactivity
{
	/// <summary>
	/// Implementation class. Exists because it's otherwise hard to implement this for both monobehaviours and networkedbehaviours
	/// </summary>
	class ReactiveBehaviourImpl : IReactiveBehaviour
	{
		private List<IDestroyable> destroyableReactives = new List<IDestroyable>();

		public GameObject gameObject => null; // This piece isn't retrieved from the impl class

		public event ObservableElementDestroyed Destroyed = delegate { };

		public void AddReflector(Action action)
		{
			var reflector = new Reflector(action);
			destroyableReactives.Add(reflector);
		}

		public Computed<T> CreateComputed<T>(Func<T> func)
		{
			var computed = new Computed<T>(func);
			destroyableReactives.Add(computed);
			return computed;
		}

		public void OnDestroy()
		{
			foreach (var destroyableReactive in destroyableReactives)
			{
				destroyableReactive.Destroy();
			}

			Destroyed();
		}
	}

	// Extension to MonoBehaviour that gives an easier method to add reflectors
	public class ReactiveBehaviour : MonoBehaviour, IReactiveBehaviour
	{
		ReactiveBehaviourImpl _impl = new ReactiveBehaviourImpl();

		public event ObservableElementDestroyed Destroyed
		{
			add { _impl.Destroyed += value; }
			remove { _impl.Destroyed -= value; }
		}

		public void AddReflector(Action action)
		{
			_impl.AddReflector(action);
		}

		public Computed<T> CreateComputed<T>(Func<T> func)
		{
			return _impl.CreateComputed(func);
		}

		public void OnDestroy()
		{
			_impl.OnDestroy();
		}
	}
}