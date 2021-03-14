using net.malevy;
using NuGet.Frameworks;
using NUnit.Framework;


namespace LRUCache.UnitTests
{
    [TestFixture]
    public class LRUTests
    {

        [Test]
        public void MaxSizeIsSet()
        {
            var cache = new LRUCache<int, string>(2);
            Assert.That(cache.MaxSize, Is.EqualTo(2));            
        }
        
        [Test]
        public void CacheWillMaintainMaxSize()
        {
            var cache = new LRUCache<int, string>(2);
            cache.Put(1, "A");
            cache.Put(2, "B");
            cache.Put(3, "C");
            Assert.That(cache.Count(), Is.EqualTo(2));
        }

        [Test]
        public void LRUIsEvicted()
        {
            var cache = new LRUCache<int, string>(2);
            cache.Put(1, "A");
            cache.Put(2, "B");
            cache.Put(3, "C");

            var keys = cache.Keys();
            CollectionAssert.AreEqual(new[]{3,2}, keys);

        }

        [Test]
        public void FetchingUpdatesHistory()
        {
            var cache = new LRUCache<int, string>(2);
            cache.Put(2, "B");
            cache.Put(3, "C");
            cache.Get(2);
            
            var keys = cache.Keys();
            CollectionAssert.AreEqual(new[]{2, 3}, keys);
            
        }

        [Test]
        public void ValuesEnumeratedInHistoryOrder()
        {
            var cache = new LRUCache<int, string>(2);
            cache.Put(1, "A");
            cache.Put(2, "B");
            cache.Put(3, "C");

            var keys = cache.Values();
            CollectionAssert.AreEqual(new[]{"C","B"}, keys);
        }
        
        [Test]
        public void IfTheMostRecentIsUpdated_ItStaysMostRecent()
        {
            var cache = new LRUCache<int, string>(2);
            cache.Put(2, "B");
            cache.Put(3, "C");
            cache.Put(3, "C");
            
            var keys = cache.Keys();
            CollectionAssert.AreEqual(new[]{3, 2}, keys);
            
        }
        
    }
}