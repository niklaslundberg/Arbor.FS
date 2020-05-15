using System;
using Zio;

namespace Arbor.FS
{
    public static class PathExtensions
    {
        public static UPath FsPath(string value) => NormalizePath(value);

        public static UPath NormalizePath(this UPath path) => NormalizePath(path.FullName);

        public static UPath NormalizePath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            if (path.Length > 2 && path[index: 1] == ':')
            {
                return new UPath("/mnt/" + path[index: 0] +
                                 path.Substring(startIndex: 2).Replace(oldChar: '\\', UPath.DirectorySeparator));
            }

            return path;
        }

        public static string WindowsPath(this UPath path)
        {
            string returnPath;

            if (path.FullName.Length > 5 && path.FullName.StartsWith("/mnt/"))
            {
                returnPath = path.FullName[index: 5] + ":" + path.FullName.Substring(startIndex: 6);
            }
            else
            {
                returnPath = path.FullName;
            }

            return returnPath.Replace(UPath.DirectorySeparator, newChar: '\\');
        }
    }
}