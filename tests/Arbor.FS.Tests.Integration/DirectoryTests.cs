using Xunit;
using Zio;
using Zio.FileSystems;

namespace Arbor.FS.Tests.Integration
{
    public class DirectoryTests
    {
        [Fact]
        public void PathDoesExist()
        {
            using var fs = new MemoryFileSystem();

            UPath path = "/mnt/c/test";
            fs.CreateDirectory(path);

            var entry = fs.EnsureExists(path);

            Assert.True(entry.Exists);
        }

        [Fact]
        public void EntryDoesExist()
        {
            using var fs = new MemoryFileSystem();

            UPath path = "/mnt/c/test";
            fs.CreateDirectory(path);
            var directoryEntry = fs.GetDirectoryEntry(path);

            var entry = directoryEntry.EnsureExists();

            Assert.True(entry.Exists);
        }

        [Fact]
        public void PathDoesNotExist()
        {
            using var fs = new MemoryFileSystem();

            UPath path = "/mnt/c/test";

            var entry = fs.EnsureExists(path);

            Assert.True(entry.Exists);
        }

        [Fact]
        public void EntryDoesNotExist()
        {
            using var fs = new MemoryFileSystem();

            UPath path = "/mnt/c/test";
            var directoryEntry = new DirectoryEntry(fs, path);

            var entry = directoryEntry.EnsureExists();

            Assert.True(entry.Exists);
        }
    }
}