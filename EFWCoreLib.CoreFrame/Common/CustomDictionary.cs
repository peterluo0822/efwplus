using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// 支持线程安全的字典结构
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CustomDictionary<TKey, TValue>
    {
        
        private ConcurrentDictionary<TKey, TValue> _dic;
        public CustomDictionary()
        {
            int numProcs = Environment.ProcessorCount;
            int concurrencyLevel = numProcs * 2;
            _dic = new ConcurrentDictionary<TKey, TValue>(numProcs, concurrencyLevel);
        }
        #region IDictionary<string,ResultType> 成员

        public void Add(TKey key, TValue value)
        {
            _dic.TryAdd(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dic.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _dic.Keys; }
        }

        public bool Remove(TKey key)
        {
            TValue val;
            return _dic.TryRemove(key, out val);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dic.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _dic.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _dic[key];
            }
            set
            {
                _dic[key] = value;
            }
        }

        public void Clear()
        {
            _dic.Clear();
        }

        public IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderBy(Func<KeyValuePair<TKey, TValue>, TKey> keySelector)
        {
            return _dic.OrderBy(keySelector);
        }
        #endregion
    }
}
