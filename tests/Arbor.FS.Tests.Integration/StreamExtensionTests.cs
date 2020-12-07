using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Zio.FileSystems;

namespace Arbor.FS.Tests.Integration
{
    public class StreamExtensionTests
    {
        [Fact]
        public async Task WriteAllText()
        {
            using var fs = new MemoryFileSystem();

            string writeContent = "content";

            await fs.WriteAllTextAsync("/a", writeContent);

            string content = await fs.ReadAllTextAsync("/a");

            content.Should().Be(writeContent);
        }

        [Fact]
        public async Task WriteAllLines()
        {
            using var fs = new MemoryFileSystem();

            var writeContent = new List<string>{"test1", "test2"};

            await fs.WriteAllLinesAsync("/a", writeContent);

            var content = await fs.ReadAllLinesAsync("/a");

            content.Should().ContainInOrder(writeContent);
        }
    }
}