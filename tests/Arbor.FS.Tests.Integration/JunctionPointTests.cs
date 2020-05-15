using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zio;
using Zio.FileSystems;
using static Arbor.FS.PathExtensions;

namespace Arbor.FS.Tests.Integration
{
    public class JunctionPointTests
    {
        [Fact]
        public async Task CreateJunctionPointWindowsPhysical()
        {
            IFileSystem fs = new PhysicalJunctionFs(new WindowsFs(new PhysicalFileSystem()));

            var target = FsPath("C:/temp/sub1/sub2");

            var junctionPoint = FsPath("C:/temp/virtualtest");

            fs.CreateDirectory(target);

            var entry = new DirectoryEntry(fs, target);

            var filePath = UPath.Combine(entry.Path, "test.txt");
            await using var testFile = fs.CreateFile(filePath);

            await testFile.WriteAsync(Encoding.UTF8.GetBytes("123"));

            fs.CreateJunctionPoint(junctionPoint, target, overwrite: true);

            var virtualFilePath = UPath.Combine(junctionPoint, "test.txt");

            var virtualFile = fs.GetFileEntry(virtualFilePath);
            var physicalFile = fs.GetFileEntry(filePath);

            Assert.True(virtualFile.Exists);
            Assert.True(physicalFile.Exists);
        }

        [Fact]
        public async Task CreateJunctionPointWithJunctionFs()
        {
            IFileSystem fs = new JunctionFs(new MemoryFileSystem());

            UPath target = "/test1";
            UPath junctionPoint = "/virtual";

            fs.CreateDirectory(target);

            var entry = new DirectoryEntry(fs, target);

            var filePath = UPath.Combine(entry.Path, "test.txt");
            await using var testFile = fs.CreateFile(filePath);

            await testFile.WriteAsync(Encoding.UTF8.GetBytes("123"));

            fs.CreateJunctionPoint(junctionPoint, target, overwrite: true);

            var virtualFilePath = UPath.Combine(junctionPoint, "test.txt");

            var virtualFile = fs.GetFileEntry(virtualFilePath);
            var physicalFile = fs.GetFileEntry(filePath);

            Assert.True(virtualFile.Exists);
            Assert.True(physicalFile.Exists);
        }

        [Fact]
        public async Task CreateJunctionPointWithJunctionFsForDeepPath()
        {
            IFileSystem fs = new JunctionFs(new MemoryFileSystem());

            UPath target = "/test1/sub1/sub2";
            UPath junctionPoint = "/virtual";

            fs.CreateDirectory(target);

            var entry = new DirectoryEntry(fs, target);

            var filePath = UPath.Combine(entry.Path, "test.txt");
            await using var testFile = fs.CreateFile(filePath);

            await testFile.WriteAsync(Encoding.UTF8.GetBytes("123"));

            fs.CreateJunctionPoint(junctionPoint, target, overwrite: true);

            var virtualFilePath = UPath.Combine(junctionPoint, "test.txt");

            var virtualFile = fs.GetFileEntry(virtualFilePath);
            var physicalFile = fs.GetFileEntry(filePath);

            Assert.True(virtualFile.Exists);
            Assert.True(physicalFile.Exists);
        }

        [Fact]
        public void DoubleConvert()
        {
            var original = new UPath("/mnt/c/123");

            string windowsPath = original.WindowsPath();

            var asUPath = windowsPath.NormalizePath();

            Assert.Equal(original, asUPath);
        }
    }
}