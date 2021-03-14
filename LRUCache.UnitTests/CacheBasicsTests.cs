using net.malevy;
using NUnit.Framework;

namespace LRUCache.UnitTests
{
    public class CacheBasicsTests
    {

        [Test]
        public void CanAddAndFetchWithStringKey()
        {
            var cache = new LRUCache<string,string>(10);
            cache.Put("1","A");
            Assert.That(cache.Get("1"), Is.EqualTo("A"));
        }

        [Test]
        public void CanAddAndFetchWithValueKey()
        {
            var cache = new LRUCache<int, string>(10);
            cache.Put(1, "A");
            Assert.That(cache.Get(1), Is.EqualTo("A"));
        }

        [Test]
        public void CacheMissReturnsDefault()
        {
            var cache = new LRUCache<int, string>(10);
            Assert.That(cache.Get(1), Is.EqualTo(default(string)));            
        }

        [Test]
        public void CacheCanCount()
        {
            var cache = new LRUCache<int, string>(10);
            cache.Put(1, "A");
            cache.Put(2, "B");
            Assert.That(cache.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanClear()
        {
            var cache = new LRUCache<int, string>(10);
            cache.Put(1, "A");
            cache.Clear();                    
            Assert.That(cache.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanSetMissingValueValue()
        {
            var missingValueValue = "NOT FOUND";
            var cache = new LRUCache<int, string>(10, missingValueValue);
            Assert.That(cache.Get(1), Is.EqualTo(missingValueValue));
        }
    }
}