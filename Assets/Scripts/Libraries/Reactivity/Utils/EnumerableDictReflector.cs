using Reactivity.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reactivity
{
	/// <summary>
	/// This class helps when reflecting off an IEnumerable to some other currency
	/// It simplifies the task of determining what items need to be created / removed when mapping from something
	/// </summary>
	/// <typeparam name="TSource">The keyable type of the IEnumerable. This should really be unique</typeparam>
	/// <typeparam name="TObj">The create portion can return something of this type. This helps it get cleaned up in the delete portion</typeparam>
	internal sealed class EnumerableDictReflector<TSource, TObj>
	{
		private readonly Func<TSource, TObj> _create = null;
		private readonly Action<TObj> _delete = null;

		private IEnumerable<TSource> _lastItems;
		private readonly IDictionary<TSource, TObj> _currentObjs;
		private readonly Notifier notifier = new Notifier();


		public EnumerableDictReflector()
		{
			_lastItems = Enumerable.Empty<TSource>();
			_currentObjs = new Dictionary<TSource, TObj>();
		}
		public EnumerableDictReflector(Func<TSource, TObj> create, Action<TObj> delete)
			: this()
		{
			_create = create;
			_delete = delete;
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
				var newObj = _create != null ? _create(add) : default(TObj);
				_currentObjs[add] = newObj;
			}
			foreach (var remove in removes)
			{
				_currentObjs.Remove(remove, out var removedObj);
				if (_delete != null) _delete(removedObj);
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

		public IReadOnlyDictionary<TSource, TObj> Dict
		{
			get
			{
				notifier.Track();
				return (IReadOnlyDictionary<TSource, TObj>)_currentObjs;
			}
		}

		public IEnumerable<KeyValuePair<TSource, TObj>> KVP
		{
			get
			{
				notifier.Track();
				return _currentObjs.AsEnumerable();
			}

		}
	}
}
