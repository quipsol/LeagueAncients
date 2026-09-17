namespace LeagueAncients.Extensions;

public static class DictionaryExtensions
{


    extension<TKey, TValue>(IDictionary<TKey, TValue> dict) where TValue : new()
    {
        /// <summary>
        /// Returns the value associated with the given key. <br/>
        /// If the key does not exist, add it with a new object instance of the value type. <br/>
        /// TValue must have a parameterless constructor!
        /// </summary>
        /// <returns>The value associated with the specified key</returns>
        public TValue GetOrCreate(TKey key)
        {
            if (dict.TryGetValue(key, out var val)) return val;
            val = new TValue();
            dict.Add(key, val);
            return val;
        }
    }
    
    extension<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        /// <summary>
        /// Returns the value associated with the given key. <br/>
        /// If the key does not exist, add it with the provided value.
        /// </summary>
        /// <param name="key">The key to look up</param>
        /// <param name="val">A default value if the key does not exist</param>
        /// <returns>The value associated with the specified key</returns>
        public TValue GetOrCreate(TKey key, TValue val)
        {
            if (dict.TryGetValue(key, out var val2)) return val2;
            dict.Add(key, val);
            return val;
        }

        /// <summary>
        /// Returns the value associated with the given key. <br/>
        /// If the key does not exist, add it with a value calculated from the passed function.
        /// </summary>
        /// /// <param name="key">The key to look up</param>
        /// <param name="factory">If the key does not exist yet, this will create the value for the new key</param>
        /// <returns>The value associated with the specified key</returns>
        public TValue GetOrCreate(TKey key, Func<TKey, TValue> factory)
        {
            if (dict.TryGetValue(key, out var value)) return value;
            value = factory(key);
            dict.Add(key, value);
            return value;
        }
    }
}