using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for doing Resource.Load operations
/// Ideally I shouldn't even be using the resource folder.
/// I should be using Unity's fancy new "Addressables" thing
/// But IDK, that's a little overkill for a low-poly game with 64x64 textures
/// </summary>
public static class ResourceLoader
{
    static Dictionary<Type, Dictionary<string, UnityEngine.Object>> _caches = new();

    public static T Load<T>(string id) where T : UnityEngine.Object, IHasUniqueAssetId
    {
        var type = typeof(T);
        if (!_caches.TryGetValue(type, out var cache))
        {
            // Load everything
            cache = new Dictionary<string, UnityEngine.Object>();
            string folder = type.Name;
            var all = Resources.LoadAll<T>(folder);
            foreach (var item in all)
            {
                cache[item.UniqueAssetID] = item;
            }
            _caches[type] = cache;
        }

        if (cache.TryGetValue(id, out UnityEngine.Object obj))
        {
            return (T)obj;
        }
        else
        {
            throw new ArgumentException($"Failed to load id {id} for {typeof(T)}");
        }
    }
}
