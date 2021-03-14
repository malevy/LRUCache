using System;
using System.Collections.Generic;
using System.Linq;

namespace net.malevy
{
    public class LRUCache<TKey, TValue> where TKey:IComparable
    {
        private readonly int _maxSize;
        private readonly TValue _missingValue;
        private readonly IDictionary<TKey, TValue> _store =  new Dictionary<TKey, TValue>();
        private readonly LinkedList<TKey> _history = new LinkedList<TKey>();
        
        public LRUCache(int maxSize = 10, TValue missingValue = default(TValue))
        {
            if (maxSize <= 0) throw new ArgumentOutOfRangeException(nameof(maxSize));
            _maxSize = maxSize;
            _missingValue = missingValue;
        }

        public int MaxSize => _maxSize;

        public TValue Get(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!_store.ContainsKey(key)) return _missingValue;
            
            var node =_store[key];
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
                .Select(k => k)
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
            if (head != null && key.Equals(head.Value)) return;

            var keyNode = _history.Find(key);
            if (null != keyNode) _history.Remove(keyNode);
            _history.AddFirst(key);
        }

    }
}