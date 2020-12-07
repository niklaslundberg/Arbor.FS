using Xunit;
using Zio;
using Zio.FileSystems;

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
        public void WindowsPath()
        {
            string mntPath = new UPath("/mnt/c/123").WindowsPath();

            Assert.Equal("c:\\123", mntPath);
        }
    }
}