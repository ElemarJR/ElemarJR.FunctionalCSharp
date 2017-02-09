
using System;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.TrySamples
{
    using static Helpers;
    public class ParsingJson
    {
        [Fact]
        public void OldSchoolParsingIsBoring()
        {
            var source = "{'Site': 'ElemarJR's blog', 'Uri': 'http://elemarjr.com'";

            try
            {
                var parsed = JObject.Parse(source);
                try
                {
                    var stringUri = (string) parsed["Uri"];

                    try
                    {
                        var uri = new Uri(stringUri);
                        Assert.True(true);
                    }
                    catch (Exception)
                    {
                        // failed to parse Uri
                    }

                }
                catch (Exception)
                {
                    // failed to get attribute
                }
            }
            catch (Exception)
            {
                // failed to parse
            }
        }

        public static void OldSchoolParsingNotSoBoring()
        {
            var source = "{'Site': 'ElemarJR's blog', 'Uri': 'http://elemarjr.com'";

            try
            {
                var parsed = JObject.Parse(source);
                var stringUri = (string)parsed["Uri"];
                var uri = new Uri(stringUri);
                Assert.True(true);
            }
            catch (Exception e)
            {
                // failed
            }
        }

        [Fact]
        public void UsingFunctionalStyle_1()
        {
            var source = "{'Site': 'ElemarJR's blog', 'Uri': 'http://elemarjr.com'";

            Try
                .Run(() => JObject.Parse(source))
                .Bind(parsed => Try.Run(() => (string) parsed["Uri"]))
                .Bind(stringUri => Try.Run(() => new Uri(stringUri)))
                ;
        }

        [Fact]
        public void UsingFunctionalStyle_2()
        {
            var source = "{'Site': 'ElemarJR's blog', 'Uri': 'http://elemarjr.com'";

            Try
                .Run(() => JObject.Parse(source))
                .Map(parsed => (string)parsed["Uri"])
                .Map(stringUri => new Uri(stringUri))
                ;
        }

        [Fact]
        public void UsingFunctionalStyleLazy()
        {
            var source = "{'Site': 'ElemarJR's blog', 'Uri': 'http://elemarjr.com'";

            var parse = PromiseOfTry(() => JObject.Parse(source));
        }
    }
}
