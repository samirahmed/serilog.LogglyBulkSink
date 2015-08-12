using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Events;
using Serilog.Parsing;

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

        [TestMethod]
        public void TestRender()
        {
            var logEvent = new LogEvent(DateTimeOffset.UtcNow,
                LogEventLevel.Debug, null, new MessageTemplate(Enumerable.Empty<MessageTemplateToken>()), new []
                {
                    new LogEventProperty("test1", new ScalarValue("answer1")),
                    new LogEventProperty("0", new ScalarValue("this should be missing")),
                    new LogEventProperty("key", new ScalarValue("value"))
                });
            var result = LogglySink.EventToJson(logEvent);
            var json = JsonConvert.DeserializeObject<dynamic>(result);
            (json["test1"].Value as string).Should().Be("answer1");
            bool hasZero = (json["0"] == null);
            hasZero.Should().Be(true);
            (json["key"].Value as string).Should().Be("value");
        }
    }
}
