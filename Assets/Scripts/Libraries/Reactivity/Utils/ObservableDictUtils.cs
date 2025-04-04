namespace Reactivity
{
    public static class ObservableDictUtils<K, V>
    {
        public static void SetOrUpdate(ObservableDict<K, Observable<V>> dict, K key, V value)
        {
            if (dict.TryGetValue(key, out var existingObsVal))
            {
                existingObsVal.Val = value;
            }
            else
            {
                dict[key] = new Observable<V>(value);
            }
        }
    }
}
