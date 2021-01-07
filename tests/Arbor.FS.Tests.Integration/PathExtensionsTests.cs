using FluentAssertions;
using Xunit;
using Zio;

namespace Arbor.FS.Tests.Integration
{
    public class PathExtensionsTests
    {
        [Fact]
        public void NormalizePath()
        {
            var path = new UPath("c:/123").NormalizePath();

            Assert.True(path.IsAbsolute);
            Assert.Equal(new UPath("/mnt/c/123"), path);
        }

        [Fact]
        public void NormalizeBackslashWindowsPath()
        {
            var path = new UPath("c:\\123").NormalizePath();

            Assert.True(path.IsAbsolute);
            Assert.Equal(new UPath("/mnt/c/123"), path);
        }

        [Fact]
        public void WindowsPath()
        {
            string mntPath = new UPath("/mnt/c/123").WindowsPath();

            Assert.Equal("c:\\123", mntPath);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-")]
        [InlineData("+")]
        [InlineData("1")]
        [InlineData("a")]
        [InlineData("a/")]
        public void TryParseInvalidPathShouldReturnFalse(string invalidPath)
        {
            bool parsed = invalidPath.TryParseAsPath(out var resultPath);

            resultPath.Should().BeNull();
            parsed.Should().BeFalse();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/mnt/c")]
        [InlineData("/a")]
        [InlineData("/a/")]
        [InlineData("/A")]
        [InlineData("c:\\")]
        [InlineData("c:\\abc")]
        [InlineData("c:\\abc\\")]
        [InlineData("c:/abc")]
        [InlineData("c:/abc/")]
        public void TryParseValidPathShouldReturnTrue(string path)
        {
            bool parsed = path.TryParseAsPath(out var resultPath);

            resultPath.Should().NotBeNull();
            parsed.Should().BeTrue();
        }
    }
}