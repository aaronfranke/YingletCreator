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

        public bool IsInitializing { get; private set; } = false;

        public event ObservableElementDestroyed Destroyed = delegate { };

        public void AddReflector(Action action)
        {
            IsInitializing = true;
            var reflector = new Reflector(action);
            destroyableReactives.Add(reflector);
            IsInitializing = false;
        }

        public Computed<T> CreateComputed<T>(Func<T> func)
        {
            IsInitializing = true;
            var computed = new Computed<T>(func);
            destroyableReactives.Add(computed);
            IsInitializing = false;
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

        public bool IsInitializing => _impl.IsInitializing;

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

        public virtual void OnDestroy()
        {
            _impl.OnDestroy();
        }
    }
}