using System;
using System.Collections.Generic;
using System.Linq;

namespace net.malevy
{
    public class LRUCache<TKey, TValue> where TKey:IComparable
    {
        private readonly int _maxSize;
        private readonly IDictionary<TKey, TValue> _store =  new Dictionary<TKey, TValue>();
        private readonly LinkedList<TKey> _history = new LinkedList<TKey>();
        
        public LRUCache(int maxSize = 10)
        {
            if (maxSize <= 0) throw new ArgumentOutOfRangeException(nameof(maxSize));
            _maxSize = maxSize;
        }

        public int MaxSize => _maxSize;

        public TValue Get(TKey key, TValue defaultValue = default(TValue))
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!_store.ContainsKey(key)) return defaultValue;
            
            return GetInternal(key);
        }

        public TValue Get(TKey key, Func<TValue> defaultFactory) 
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (defaultFactory == null) throw new ArgumentNullException(nameof(defaultFactory));
            
            if (!_store.ContainsKey(key)) return defaultFactory.Invoke();

            return GetInternal(key);
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> missingValueFactory)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (missingValueFactory == null) throw new ArgumentNullException(nameof(missingValueFactory));

            if (!_store.ContainsKey(key))
            {
                Put(key, missingValueFactory.Invoke(key));
            }

            return GetInternal(key);
        }
        
        private TValue GetInternal(TKey key)
        {
            var node = _store[key];
            TouchKey(key);
            return node;
        }

        public void Put(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            _store[key] = value;
            TouchKey(key);
            EvictIfNeeded();
        }

        public int Count()
        {
            return _store.Count;
        }
        
        public void Clear()
        {
            _history.Clear();
            _store.Clear();
        }

        public ICollection<TKey> Keys()
        {
            var keys = new TKey[_history.Count];
            _history.CopyTo(keys, 0);
            return keys;
        }

        public ICollection<TValue> Values()
        {
            var values = _history
                .Where(k => _store.ContainsKey(k))
                .Select(k => _store[k])
                .ToArray();

            return values;
        }
        
        private void EvictIfNeeded()
        {
            while (_store.Count > _maxSize)
            {
                var victim = _history.Last;
                if (null == victim) return;
                _history.Remove(victim);
                _store.Remove(victim.Value);
            }
        }

        private void TouchKey(TKey key)
        {
            var head = _history.First;
            if (head != null && head.Value.CompareTo(key) == 0) return;

            var keyNode = _history.Find(key);
            if (null != keyNode) _history.Remove(keyNode);
            _history.AddFirst(key);
        }

    }
}