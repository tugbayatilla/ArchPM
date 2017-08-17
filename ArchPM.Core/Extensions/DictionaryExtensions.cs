using System;
using System.Collections.Generic;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Finds the key by value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">the value is not found in the dictionary</exception>
        public static TKey FindKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            ObjectExtension.NotNull(dictionary);

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                if (value.Equals(pair.Value))
                    return pair.Key;

            throw new Exception("the value is not found in the dictionary");
        }
    }
}
