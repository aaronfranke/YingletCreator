using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reactivity
{
    /// <summary>
    /// We may frequently have an IDictionary<T, IObservable<K>>, but a consumer just wants a IDictionary<T, K>
    /// This class turns the former into the latter
    /// </summary>
    internal class DictWithObservableValuesAbstraction<T, K> : IDictionary<T, K>
    {
        private readonly IDictionary<T, Observable<K>> _source;

        public DictWithObservableValuesAbstraction(IDictionary<T, Observable<K>> sourceDict)
        {
            _source = sourceDict;
        }

        public K this[T key]
        {
            get
            {
                return _source[key].Val;
            }
            set
            {
                if (_source.TryGetValue(key, out var existingObsVal))
                {
                    existingObsVal.Val = value;
                }
                else
                {
                    _source[key] = new Observable<K>(value);

                }
            }
        }

        public ICollection<T> Keys => _source.Keys;

        public ICollection<K> Values => _source.Values.Select(v => v.Val).ToArray();

        public int Count => _source.Count;

        public bool IsReadOnly => _source.IsReadOnly;

        public void Add(T key, K value)
        {
            _source.Add(key, new Observable<K>(value));
        }

        public void Add(KeyValuePair<T, K> item)
        {
            _source.Add(item.Key, new Observable<K>(item.Value));
        }

        public void Clear()
        {
            _source.Clear();
        }

        public bool Contains(KeyValuePair<T, K> item)
        {
            if (_source.TryGetValue(item.Key, out Observable<K> value))
            {
                return EqualityComparer<K>.Default.Equals(value.Val, item.Value);
            }
            return false;
        }

        public bool ContainsKey(T key)
        {
            return _source.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<T, K>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T, K>> GetEnumerator()
        {
            foreach (var item in _source)
            {
                yield return new KeyValuePair<T, K>(item.Key, item.Value.Val);
            }
        }

        public bool Remove(T key)
        {
            return _source.Remove(key);
        }

        public bool Remove(KeyValuePair<T, K> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(T key, out K value)
        {
            if (_source.TryGetValue(key, out Observable<K> observableValue))
            {
                value = observableValue.Val;
                return true;
            }
            value = default(K);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
