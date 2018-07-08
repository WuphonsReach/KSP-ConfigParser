using System;
using System.IO;
using System.Reflection;
using parse.Models;
using parse.Tests.TestFileTests;
using Xunit;

namespace parse.Tests
{
    ///<summary>Helper methods and tests to validate that the xUnit test environment
    ///is as we expect it to be.</summary>
    public class ParserTests
    {
        protected const string TestFileDirectory = "TestFiles";

        protected Parser Sut = new Parser();

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

        protected Stream GetManifestResourceStream(string fileName)
        {
            var assembly = GetAssembly();
            var resource = GetResourceName(assembly, fileName);
            return assembly.GetManifestResourceStream(resource);
        }

        protected void AssertCanReadEmbeddedResource(string fileName)
        {
            using (Stream stream = GetManifestResourceStream(fileName))
            using (StreamReader reader = new StreamReader(stream))
            {
                var line = reader.ReadLine();
                Assert.True(
                    line != null,
                    $"Could not read anything from embedded resource '{fileName}'."
                    );
            }
        }

        protected void AssertFileNameIsInMetaData(ConfigFile configFile, string expectedFileName)
        {
            Assert.True(configFile.FilePath.IndexOf(expectedFileName) >= 0,
                $"Did not find '{expectedFileName}' in '{configFile.FilePath}'."
                );
        }

        protected ConfigFile GetParsedConfigFile(string fileName)
        {
            var assembly = GetAssembly();
            var resource = GetResourceName(assembly, fileName);
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                var result = Sut.ParseConfigFile(fileName, stream);
                return result;
            }
        }

        [Theory]
        [InlineData(TestFileNames.CurlyBraceOnSameLine)]
        [InlineData(TestFileNames.MMCacheSimpleExample)]
        [InlineData(TestFileNames.SimplePart)]
        public void CanReadTestFile(string fileName)
        {
            AssertCanReadEmbeddedResource(fileName);
        }

        [Theory]
        [InlineData(TestFileNames.CurlyBraceOnSameLine)]
        [InlineData(TestFileNames.MMCacheSimpleExample)]
        [InlineData(TestFileNames.SimplePart)]
        public void FileNameIsInMetaData(string fileName)
        {
            var result = GetParsedConfigFile(fileName);
            AssertFileNameIsInMetaData(result, fileName);
        }

        [Theory]
        [InlineData(TestFileNames.CurlyBraceOnSameLine)]
        [InlineData(TestFileNames.MMCacheSimpleExample)]
        [InlineData(TestFileNames.SimplePart)]
        public void FileCanBeParsed(string fileName)
        {
            var result = GetParsedConfigFile(fileName);
            Assert.NotNull(result);
        }
    }
}
