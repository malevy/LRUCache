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
            var missingValue = "NOT FOUND";
            var cache = new LRUCache<int, string>(10);
            Assert.That(cache.Get(1, missingValue), Is.EqualTo(missingValue));
        }
        
        [Test]
        public void CanSetMissingValueValueWithFactory()
        {
            var missingValue = "NOT FOUND";
            var cache = new LRUCache<int, string>(10);
            Assert.That(cache.Get(1, () => missingValue), Is.EqualTo(missingValue));
        }

        [Test]
        public void WhenKeyIsNotPresent_UseFactoryAndAdd()
        {
            var cache = new LRUCache<int, int>(10);
            var factoryCalled = false;
            var expected = 100;
            var actual = cache.GetOrAdd(1, k =>
            {
                factoryCalled = true;
                return expected;
            });
            
            Assert.That(factoryCalled, "factory was not called");
            Assert.That(actual, Is.EqualTo(expected));
            
        }

        [Test]
        public void WhenKeyIsPresent_FactoryNotCalled()
        {
            var cache = new LRUCache<int, int>(10);
            var expected = 100;
            cache.Put(1, expected);
            var factoryCalled = false;
            var actual = cache.GetOrAdd(1, k =>
            {
                factoryCalled = true;
                return expected;
            });
            
            Assert.That(factoryCalled, Is.False, "factory should not have been called");
            Assert.That(actual, Is.EqualTo(expected));
            
        }

    }
}