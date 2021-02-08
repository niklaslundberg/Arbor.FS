using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Arbor.FS.Tests.Integration
{
    public class JunctionPointTests
    {
        [Fact]
        public async Task CreateJunctionPointWindowsPhysical()
        {
            IFileSystem fs = new PhysicalJunctionFs(new WindowsFs(new PhysicalFileSystem()));

            var target = "C:/temp/sub1/sub2".ParseAsPath();

            var junctionPointVirtualPath = "C:/temp/virtualtest".ParseAsPath();

            fs.CreateDirectory(target);

            var junctionPoint = new JunctionPoint(junctionPointVirtualPath, target);

            var entry = new DirectoryEntry(fs, target);

            var filePath = UPath.Combine(entry.Path, "test.txt");
            await using var testFile = fs.CreateFile(filePath);

            await testFile.WriteAsync(Encoding.UTF8.GetBytes("123"));

            fs.CreateJunctionPoint(junctionPoint, true);

            var virtualFilePath = UPath.Combine(junctionPointVirtualPath, "test.txt");

            var virtualFile = fs.GetFileEntry(virtualFilePath);
            var physicalFile = fs.GetFileEntry(filePath);

            Assert.True(virtualFile.Exists);
            Assert.True(physicalFile.Exists);

            bool junctionPointExists = fs.JunctionPointExists(junctionPointVirtualPath);

            Assert.True(junctionPointExists);

            fs.DeleteJunctionPoint(junctionPointVirtualPath);

            bool existsAfter = fs.JunctionPointExists(junctionPointVirtualPath);

            Assert.False(existsAfter);

            await testFile.DisposeAsync();
            fs.DeleteDirectory(target, true);
        }

        [Fact]
        public async Task CreateJunctionPointWithJunctionFs()
        {
            IFileSystem fs = new JunctionFs(new MemoryFileSystem());

            UPath target = "/test1";
            UPath junctionPointVirtualPath = "/virtual";

            var junctionPoint = new JunctionPoint(junctionPointVirtualPath, target);

            fs.CreateDirectory(target);

            var entry = new DirectoryEntry(fs, target);

            var filePath = UPath.Combine(entry.Path, "test.txt");
            await using var testFile = fs.CreateFile(filePath);

            await testFile.WriteAsync(Encoding.UTF8.GetBytes("123"));

            fs.CreateJunctionPoint(junctionPoint, true);

            var virtualFilePath = UPath.Combine(junctionPointVirtualPath, "test.txt");

            var virtualFile = fs.GetFileEntry(virtualFilePath);
            var physicalFile = fs.GetFileEntry(filePath);

            Assert.True(virtualFile.Exists);
            Assert.True(physicalFile.Exists);

            bool junctionPointExists = fs.JunctionPointExists(junctionPointVirtualPath);

            Assert.True(junctionPointExists);

            fs.DeleteJunctionPoint(junctionPointVirtualPath);

            bool existsAfter = fs.JunctionPointExists(junctionPointVirtualPath);

            Assert.False(existsAfter);
        }

        [Fact]
        public async Task CreateJunctionPointWithJunctionFsForDeepPath()
        {
            IFileSystem fs = new JunctionFs(new MemoryFileSystem());

            UPath target = "/test1/sub1/sub2";
            UPath junctionPointVirtualPath = "/virtual";

            var junctionPoint = new JunctionPoint(junctionPointVirtualPath, target);

            fs.CreateDirectory(target);

            var entry = new DirectoryEntry(fs, target);

            var filePath = UPath.Combine(entry.Path, "test.txt");
            await using var testFile = fs.CreateFile(filePath);

            await testFile.WriteAsync(Encoding.UTF8.GetBytes("123"));

            fs.CreateJunctionPoint(junctionPoint, true);

            var virtualFilePath = UPath.Combine(junctionPointVirtualPath, "test.txt");

            var virtualFile = fs.GetFileEntry(virtualFilePath);
            var physicalFile = fs.GetFileEntry(filePath);

            Assert.True(virtualFile.Exists);
            Assert.True(physicalFile.Exists);

            bool junctionPointExists = fs.JunctionPointExists(virtualFilePath);

            Assert.True(junctionPointExists);
        }

        [Fact]
        public void DoubleConvert()
        {
            var original = new UPath("/mnt/c/123");

            string windowsPath = original.WindowsPath();

            var asUPath = windowsPath.ParseAsPath();

            Assert.Equal(original, asUPath);
        }
    }
}