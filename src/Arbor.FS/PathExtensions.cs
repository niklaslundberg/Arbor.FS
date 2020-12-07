using System;
using Zio;

namespace Arbor.FS
{
    public static class PathExtensions
    {

        public static UPath NormalizePath(this UPath path) => AsNormalizePath(path.FullName);

        public static UPath AsNormalizePath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            if (path.Length > 2 && path[1] == ':')
            {
                return new UPath("/mnt/" + path[0] +
                                 path.Substring(2).Replace('\\', UPath.DirectorySeparator));
            }

            return path;
        }

        public static string WindowsPath(this UPath path)
        {
            string returnPath;

            if (path.FullName.Length > 5 && path.FullName.StartsWith("/mnt/"))
            {
                returnPath = path.FullName[5] + ":" + path.FullName.Substring(6);
            }
            else
            {
                returnPath = path.FullName;
            }

            return returnPath.Replace(UPath.DirectorySeparator, '\\');
        }
    }
}