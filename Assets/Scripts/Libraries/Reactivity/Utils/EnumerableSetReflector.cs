using System;
using System.Collections.Generic;

namespace Reactivity
{
	/// <summary>
	/// This class helps when reflecting off changes from some IEnumerable
	/// If a mapping is preffered, use <see cref="EnumerableDictReflector{TSource, TObj}"/> instead
	/// (This is really just implemented with that tbh)
	/// </summary>
	/// <typeparam name="TSource">The keyable type of the IEnumerable. This should be unique</typeparam>
	internal sealed class EnumerableSetReflector<TSource>
	{
		EnumerableDictReflector<TSource, object> _impl;

		public EnumerableSetReflector()
		{
			_impl = new EnumerableDictReflector<TSource, object>();
		}

		public EnumerableSetReflector(Action<TSource> create)
		{
			Func<TSource, object> createFunc = null;
			if (create != null)
			{
				createFunc = (TSource item) => { create(item); return null; };
			}
			_impl = new EnumerableDictReflector<TSource, object>(createFunc, null);
		}


		/// <summary>
		/// To be called within a reflect method
		/// Pass in the (observable) elements you want reflected
		/// It will cache the previous result to know what is added and removed
		/// Then the reflector will simply have to act on the individual item
		/// </summary>
		public void Enumerate(IEnumerable<TSource> items)
		{
			_impl.Enumerate(items);
		}

		public IEnumerable<TSource> Items => _impl.Keys;
	}
}
