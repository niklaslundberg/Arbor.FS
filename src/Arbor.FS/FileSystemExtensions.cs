using System;
using System.IO;
using System.Linq;
using Zio;

namespace Arbor.FS
{
    public static class FileSystemExtensions
    {
        public static void CreateJunctionPoint(this IFileSystem fileSystem,
            JunctionPoint junctionPoint,
            bool overwrite)
        {
            if (fileSystem is IJunctionPointFeature junctionPointFeature)
            {
                junctionPointFeature.CreateJunctionPoint(junctionPoint, overwrite);
                return;
            }

            throw new NotSupportedException(
                $"Junction point is not supported in file system {fileSystem.GetType().Name}");
        }

        public static bool JunctionPointExists(this IFileSystem fileSystem,
            UPath virtualPath)
        {
            if (fileSystem is IJunctionPointFeature junctionPointFeature)
            {
                return junctionPointFeature.JunctionPointExists(virtualPath);
            }

            throw new NotSupportedException(
                $"Junction point is not supported in file system {fileSystem.GetType().Name}");
        }

        public static UPath GetTargetPath(this IFileSystem fileSystem,
            UPath virtualPath)
        {
            if (fileSystem is IJunctionPointFeature junctionPointFeature)
            {
                return junctionPointFeature.GetJunctionTargetPath(virtualPath);
            }

            throw new NotSupportedException(
                $"Junction point is not supported in file system {fileSystem.GetType().Name}");
        }

        public static void DeleteJunctionPoint(this IFileSystem fileSystem,
            UPath virtualPath)
        {
            if (fileSystem is IJunctionPointFeature junctionPointFeature)
            {
                junctionPointFeature.DeleteJunctionPoint(virtualPath);
                return;
            }

            throw new NotSupportedException(
                $"Junction point is not supported in file system {fileSystem.GetType().Name}");
        }

        public static void DeleteIfExists(this DirectoryEntry? directoryEntry, bool recursive = true)
        {
            if (directoryEntry is null)
            {
                return;
            }

            try
            {
                if (directoryEntry.Exists)
                {
                    FileEntry[] files;

                    try
                    {
                        files = directoryEntry.EnumerateFiles().ToArray();
                    }
                    catch (Exception ex)
                    {
                        if (ex.IsFatal())
                        {
                            throw;
                        }

                        throw new IOException(
                            $"Could not get files for directory '{directoryEntry.FullName}' for deletion",
                            ex);
                    }

                    foreach (var file in files)
                    {
                        file.Attributes = FileAttributes.Normal;

                        try
                        {
                            file.Delete();
                        }
                        catch (Exception ex)
                        {
                            if (ex.IsFatal())
                            {
                                throw;
                            }

                            throw new IOException($"Could not delete file '{file.FullName}'", ex);
                        }
                    }

                    foreach (var subDirectory in directoryEntry.EnumerateDirectories())
                    {
                        subDirectory.DeleteIfExists(recursive);
                    }
                }

                if (directoryEntry.Exists)
                {
                    directoryEntry.Delete(recursive);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidOperationException($"Could not delete directory '{directoryEntry.FullName}'", ex);
            }
        }

        public static bool DeleteIfExists(this FileEntry? file)
        {
            if (file is null)
            {
                return false;
            }

            try
            {
                file.Delete();
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new IOException($"Unauthorized to delete file '{file.FullName}'", ex);
            }
        }

        public static DirectoryEntry EnsureExists(this IFileSystem fileSystem, UPath path) =>
            fileSystem.DirectoryExists(path)
                ? fileSystem.GetDirectoryEntry(path)
                : new DirectoryEntry(fileSystem, path).EnsureExists();

        public static DirectoryEntry EnsureExists(this DirectoryEntry entry)
        {
            if (!entry.Exists)
            {
                entry.Create();
            }

            return entry;
        }

        public static string ConvertPathToInternal(this FileEntry file) =>
            file.FileSystem.ConvertPathToInternal(file.Path);

        public static string ConvertPathToInternal(this DirectoryEntry directoryEntry) =>
            directoryEntry.FileSystem.ConvertPathToInternal(directoryEntry.Path);
    }
}