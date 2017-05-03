using FluentAssertions;
using GitHub.Unity;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class SimpleJsonTests
    {
        [Test]
        public void GitHubSimpleJsonTest()
        {
            var connectionCacheItems = new[] {
                new ConnectionCacheItem() { Host = "Host1", Username = "User1" },
                new ConnectionCacheItem() { Host = "Host2", Username = "User2" }
            };

            connectionCacheItems.ShouldAllBeEquivalentTo(new [] {
                                    new ConnectionCacheItem { Host = "Host1", Username = "User1" },
                                    new ConnectionCacheItem { Host = "Host2", Username = "User2" }
                                });

            var json = GitHub.SimpleJson.SerializeObject(connectionCacheItems);

            var deserializeObject = GitHub.SimpleJson.DeserializeObject<ConnectionCacheItem[]>(json);
            deserializeObject.ShouldAllBeEquivalentTo(new[] {
                                    new ConnectionCacheItem { Host = "Host1", Username = "User1" },
                                    new ConnectionCacheItem { Host = "Host2", Username = "User2" }
                                });
        }
    }
}