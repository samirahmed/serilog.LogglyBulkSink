using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Serilog.LogglyBulkSink.Tests
{
    [TestClass]
    public class SerilogSinkTests
    {
        [TestMethod]
        public void TestAddIfContains()
        {
            var dictionary = new Dictionary<string, string>()
            {
                {"hello", "world"}
            };
            LogglySink.AddIfNotContains(dictionary, "hello", "another world");
            dictionary.ContainsKey("hello").Should().BeTrue();
            dictionary["hello"].Should().Be("world");


            LogglySink.AddIfNotContains(dictionary, "newkey", "orange");
            dictionary.ContainsKey("newkey").Should().BeTrue();
            dictionary["newkey"].Should().Be("orange");
        }

        [TestMethod]
        public void PackageContentsTest()
        {
            var jsons = new[]
            {
                "{'fruit': 'orange'}",
                "{'fruit': 'apple'}",
                "{'fruit': 'banana'}",
            }.ToList();

            var stringContent = LogglySink.PackageContent(jsons, Encoding.UTF8.GetByteCount(string.Join("\n", jsons)), 0);
            stringContent.Should().NotBeNull();
            var result = stringContent.ReadAsStringAsync().GetAwaiter().GetResult();
            result.Split('\n').Count().Should().Be(4);
        }
    }
}
