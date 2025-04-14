using Reactivity.Implementation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reactivity
{
	public class ObservableHashSet<T> : IObservableEnumerable<T>, ICollection<T>
	{
		class DictValue
		{
			public bool Exists { get; set; }
			public Notifier Notifier { get; } = new Notifier();
		}

		readonly Dictionary<T, DictValue> dict = new Dictionary<T, DictValue>();

		readonly Notifier enumerableNotifier = new Notifier();
		#region Collection

		public void Add(T item)
		{
			if (!dict.TryGetValue(item, out var dictValue))
			{
				dictValue = new DictValue();
				dict.Add(item, dictValue);
			}
			if (dictValue.Exists) return;

			dictValue.Exists = true;
			dictValue.Notifier.Dirty();

			enumerableNotifier.Dirty();
		}

		public bool Remove(T item)
		{
			var result = RemoveNoEnumNotify(item);
			enumerableNotifier.Dirty();
			return result;
		}

		bool RemoveNoEnumNotify(T item)
		{
			if (!dict.TryGetValue(item, out var dictValue))
			{
				dictValue = new DictValue();
				dict.Add(item, dictValue);
			}
			var returnVal = dictValue.Exists;
			dictValue.Exists = false;
			dictValue.Notifier.Dirty();

			return returnVal;
		}

		public bool Contains(T item)
		{
			if (!dict.TryGetValue(item, out var dictValue))
			{
				dictValue = new DictValue();
				dict.Add(item, dictValue);
			}
			dictValue.Notifier.Track();

			return dictValue.Exists == true;
		}

		public int Count
		{
			get
			{
				enumerableNotifier.Track();
				return dict.Count(kvp => kvp.Value.Exists);
			}
		}

		public bool IsReadOnly =>
		throw new System.NotImplementedException();

		public void Clear()
		{
			var keys = dict.Keys.ToArray();
			foreach (var key in keys)
			{
				RemoveNoEnumNotify(key);
			}
			enumerableNotifier.Dirty();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			dict
				.Where(kvp => kvp.Value.Exists)
				.Select(kvp => kvp.Key)
				.ToArray()
				.CopyTo(array, arrayIndex);

			enumerableNotifier.Track();
		}
		#endregion

		#region IEnumerable
		public IEnumerator<T> GetEnumerator()
		{
			enumerableNotifier.Track();

			return dict
				.Where(kvp => kvp.Value.Exists)
				.Select(kvp => kvp.Key)
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}

}