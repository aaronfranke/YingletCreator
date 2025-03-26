using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reactivity.Implementation;

namespace Reactivity
{
	class ArrayVal<T>
	{
		public T Val { get; set; }
		public Notifier Notifier { get; } = new Notifier();
	}

	public class ObservableArray<T> : IObservableEnumerable<T>
	{
		readonly ArrayVal<T>[] array;

		readonly Notifier enumerableNotifier = new Notifier();

		public ObservableArray(int size)
		{
			array = new ArrayVal<T>[size];
			for (int i = 0; i < size; i++)
			{
				array[i] = new ArrayVal<T>();
			}
		}
		public ObservableArray(T[] from)
		{
			array = new ArrayVal<T>[from.Length];
			for (int i = 0; i < from.Length; i++)
			{
				array[i] = new ArrayVal<T>();
				array[i].Val = from[i];
			}
		}

		#region IArray (if such a thing were to exist) 
		public T this [int index]
		{
			get
			{
				var arrayVal = array[index];
				arrayVal.Notifier.Track();
				return arrayVal.Val;
			}
			set
			{
				var arrayVal = array[index];
				if (EqualityComparer<T>.Default.Equals(arrayVal.Val, value)) return;
				arrayVal.Val = value;
				arrayVal.Notifier.Dirty();
				enumerableNotifier.Dirty();
			}
		}

		public int IndexOf(T value)
		{
			enumerableNotifier.Track();
			return Array.FindIndex(array, x => EqualityComparer<T>.Default.Equals(x.Val, value));
		}
		#endregion

		#region IEnumerable
		public IEnumerator<T> GetEnumerator()
		{
			enumerableNotifier.Track();

			return array
				.Select(val => val.Val)
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}

}