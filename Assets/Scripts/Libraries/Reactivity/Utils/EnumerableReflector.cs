using Reactivity.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reactivity
{
	/// <summary>
	/// This class helps when reflecting off an IEnumerable
	/// It simplifies the task of determining what items need to be created / removed
	/// </summary>
	/// <typeparam name="TSource">The keyable type of the IEnumerable. This should really be unique</typeparam>
	/// <typeparam name="TObj">The create portion can return something of this type. This helps it get cleaned up in the delete portion</typeparam>
	internal sealed class EnumerableReflector<TSource, TObj>
	{
		private readonly Func<TSource, TObj> _create;
		private readonly Action<TObj> _delete;

		private IEnumerable<TSource> _lastItems;
		private readonly IDictionary<TSource, TObj> _currentObjs;
		private readonly Notifier notifier = new Notifier();


		public EnumerableReflector(Func<TSource, TObj> create, Action<TObj> delete)
		{
			_create = create;
			_delete = delete;
			_lastItems = Enumerable.Empty<TSource>();
			_currentObjs = new Dictionary<TSource, TObj>();
		}

		/// <summary>
		/// To be called within a reflect method
		/// Pass in the (observable) elements you want reflected
		/// It will cache the previous result to know what is added and removed
		/// Then the reflector will simply have to act on the individual item
		/// </summary>
		public void Enumerate(IEnumerable<TSource> items)
		{
			// We only want to iterate this once, so create a copy
			var itemsCopy = items.ToArray();

			var adds = itemsCopy.Except(_lastItems);
			var removes = _lastItems.Except(itemsCopy);
			foreach (var add in adds)
			{
				var newObj = _create(add);
				_currentObjs[add] = newObj;
			}
			foreach (var remove in removes)
			{
				_currentObjs.Remove(remove, out var removedObj);
				_delete(removedObj);
			}
			_lastItems = itemsCopy;
			if (adds.Any() || removes.Any())
			{
				notifier.Dirty();
			}
		}

		public IEnumerable<TSource> Keys
		{
			get
			{
				notifier.Track();
				return _currentObjs.Keys;
			}
		}
		public IEnumerable<TObj> Values
		{
			get
			{
				notifier.Track();
				return _currentObjs.Values;
			}
		}
	}
}
