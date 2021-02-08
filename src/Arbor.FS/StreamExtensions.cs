using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zio;

namespace Arbor.FS
{
    public static class StreamExtensions
    {
        public static async Task WriteAllTextAsync(this Stream stream,
            string content,
            Encoding? encoding = null,
            bool leaveOpen = false,
            CancellationToken cancellationToken = default)
        {
            await using StreamWriter writer = new(stream, encoding ?? Encoding.UTF8, leaveOpen: leaveOpen);

            await writer.WriteAsync(content.AsMemory(), cancellationToken);
        }

        public static async Task WriteAllTextAsync(this FileEntry fileEntry,
            string content,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default)
        {
            fileEntry.Directory.EnsureExists();
            await using var stream = fileEntry.Open(FileMode.OpenOrCreate, FileAccess.Write);

            await WriteAllTextAsync(stream, content, encoding, true, cancellationToken);
        }
        public static async Task WriteAllTextAsync(this IFileSystem fileSystem,
            UPath path,
            string content,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default)
        {
            fileSystem.EnsureExists(path.GetDirectory()).EnsureExists();
            await using var stream = fileSystem.OpenFile(path, FileMode.OpenOrCreate, FileAccess.Write);

            await WriteAllTextAsync(stream, content, encoding, true, cancellationToken);
        }

        public static async Task WriteAllLinesAsync(this Stream stream,
            IReadOnlyCollection<string> lines,
            Encoding? encoding = null,
            bool leaveOpen = false,
            CancellationToken cancellationToken = default)
        {
            await using StreamWriter writer = new(stream, encoding ?? Encoding.UTF8, leaveOpen: leaveOpen);

            foreach (string line in lines)
            {
                await writer.WriteLineAsync(line.AsMemory(), cancellationToken);
            }
        }

        public static async Task WriteAllLinesAsync(this IFileSystem fileSystem,
            UPath path,
            IReadOnlyCollection<string> lines,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default)
        {
            fileSystem.EnsureExists(path.GetDirectory()).EnsureExists();
            await using var stream = fileSystem.OpenFile(path, FileMode.OpenOrCreate, FileAccess.Write);

            await WriteAllLinesAsync(stream, lines, encoding, true, cancellationToken);
        }

        public static async Task<string> ReadAllTextAsync(this Stream stream,
            Encoding? encoding = null,
            bool leaveOpen = false,
            CancellationToken cancellationToken = default)
        {
            using StreamReader reader = new(stream, encoding ?? Encoding.UTF8, leaveOpen: leaveOpen);

            return await reader.ReadToEndAsync();
        }

        public static async Task<string> ReadAllTextAsync(this IFileSystem fileSystem,
            UPath path,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default)
        {
            await using var stream = fileSystem.OpenFile(path, FileMode.Open, FileAccess.Read);

            return await ReadAllTextAsync(stream, encoding, true, cancellationToken);
        }

        public static async Task<IReadOnlyCollection<string>> ReadAllLinesAsync(this Stream stream,
            Encoding? encoding = null,
            bool leaveOpen = false,
            CancellationToken cancellationToken = default)
        {
            using StreamReader reader = new(stream, encoding ?? Encoding.UTF8, leaveOpen: leaveOpen);

            List<string> lines = new();
            string? line;

            do
            {
                line = await reader.ReadLineAsync();

                if (line != null)
                {
                    lines.Add(line);
                }
            } while (line != null && !cancellationToken.IsCancellationRequested);

            return lines;
        }

        public static async Task<IReadOnlyCollection<string>> ReadAllLinesAsync(this IFileSystem fileSystem,
            UPath path,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default)
        {
            await using var stream = fileSystem.OpenFile(path, FileMode.OpenOrCreate, FileAccess.Read);

            return await ReadAllLinesAsync(stream, encoding, true, cancellationToken);
        }
    }
}