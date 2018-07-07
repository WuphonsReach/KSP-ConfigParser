using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace parse.Tests
{
    public class ParserTests
    {
        protected const string TestFileDirectory = "TestFiles";

        protected Assembly GetAssembly()
        {
            var assembly = typeof(ParserTests).GetTypeInfo().Assembly;
            return assembly;
        }

        protected string GetResourceName(Assembly assembly, string fileName)
        {
            var assemblyFullName = assembly.GetName().Name;
            var resource = $"{assemblyFullName}.{TestFileDirectory}.{fileName}";
            return resource;
        }

        protected void AssertCanReadEmbeddedResource(string fileName)
        {
            var assembly = GetAssembly();
            var resource = GetResourceName(assembly, fileName);
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            using (StreamReader reader = new StreamReader(stream))
            {
                var line = reader.ReadLine();
                Assert.True(
                    line != null,
                    $"Could not read anything from embedded resource '{resource}'."
                    );
            }
        }

        public class MMCacheSimpleExampleTests : ParserTests
        {
            private const string _fileName = "MMCacheSimpleExample.cfg";

            [Fact]
            public void CanReadTestFile()
            {
                AssertCanReadEmbeddedResource(_fileName);
            }
        }
    }
}
