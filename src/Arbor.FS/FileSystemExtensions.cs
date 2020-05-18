using System;
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
    }
}