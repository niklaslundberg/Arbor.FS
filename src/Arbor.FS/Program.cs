using System;
using Zio;
using Zio.FileSystems;

namespace Arbor.FS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IFileSystem fs = new MemoryFileSystem();

            var uPath = new UPath("/test.txt");
            var stream = fs.CreateFile(uPath);

            stream.WriteByte(value: 1);

            stream.Flush();

            stream.Dispose();

            var bytes = fs.ReadAllBytes(uPath);

            Console.WriteLine(bytes.Length);

            Console.WriteLine(bytes.Length == 1 && bytes[0] == 1);
        }
    }
}