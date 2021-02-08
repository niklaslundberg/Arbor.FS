using System;
using Zio;

namespace Arbor.FS
{
    public static class PathExtensions
    {
        public static UPath NormalizePath(this UPath path) => AsNormalizePath(path.FullName);

        public static bool TryParseAsPath(this string path, out UPath? resultPath)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                resultPath = default;
                return false;
            }

            if (!UPath.TryParse(path, out var parsed))
            {
                resultPath = default;
                return false;
            }

            var normalized = parsed.NormalizePath();

            if (!normalized.IsAbsolute)
            {
                resultPath = default;
                return false;
            }

            resultPath = normalized;
            return true;
        }

        public static UPath ParseAsPath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            if (!UPath.TryParse(path, out var parsed))
            {
                throw new FormatException($"Could not parse '{path}' as a full path");
            }

            var normalized = parsed.NormalizePath();

            if (!normalized.IsAbsolute)
            {
                throw new FormatException($"Path {parsed.FullName} is not a full path");
            }

            return normalized;
        }

        private static UPath AsNormalizePath(this string path)
        {
            while (true)
            {
                switch (path.Length)
                {
                    case 2 when path[1] == ':':
                        path = $"{path}/";
                        continue;
                    case > 2 when path[1] == ':':
                        return new UPath($"/mnt/{path[0]}{path.Substring(2).Replace('\\', UPath.DirectorySeparator)}");
                    default:
                        return path;
                }
            }
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