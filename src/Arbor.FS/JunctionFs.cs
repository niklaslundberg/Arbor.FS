using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Zio;
using Zio.FileSystems;

namespace Arbor.FS
{
    public class JunctionFs : ComposeFileSystem, IJunctionPointFeature
    {
        private readonly ConcurrentDictionary<UPath, UPath> _map;

        public JunctionFs(IFileSystem fileSystem, bool owned = true) : base(fileSystem, owned) =>
            _map = new ConcurrentDictionary<UPath, UPath>();

        public void CreateJunctionPoint(JunctionPoint junctionPoint, bool overwrite)
        {
            _map.TryRemove(junctionPoint.VirtualPath, out _);
            _map.TryAdd(junctionPoint.VirtualPath, junctionPoint.TargetPath);
        }

        public UPath GetJunctionTargetPath(UPath virtualPath)
        {
            if (_map.TryGetValue(virtualPath, out var path))
            {
                return path;
            }

            throw new IOException("There is no junction point for " + virtualPath.FullName);
        }

        public bool JunctionPointExists(UPath virtualPath) => IsPathJunction(virtualPath, out _);

        public void DeleteJunctionPoint(UPath virtualPath)
        {
            if (IsPathJunction(virtualPath, out var path) && path.HasValue)
            {
                _map.TryRemove(path.Value, out _);
            }
        }

        protected override UPath ConvertPathToDelegate(UPath path)
        {
            if (IsPathJunction(path, out var foundPath) && foundPath.HasValue)
            {
                string newPath = path.FullName.Replace(foundPath.Value.FullName, _map[foundPath.Value].FullName);

                return new UPath(newPath);
            }

            if (_map.TryGetValue(path, out var mappedPath))
            {
                return mappedPath;
            }

            return path;
        }

        private bool IsPathJunction(UPath path, out UPath? resultPath)
        {
            bool found = _map.Keys.FirstOrDefault(key => path.IsInDirectory(key, recursive: true)) is { } foundPath &&
foundPath.FullName?.Length > 0;

            if (found)
            {
                resultPath = foundPath;
                return true;
            }

            resultPath = default;
            return false;
        }

        protected override UPath ConvertPathFromDelegate(UPath path) => path;
    }
}