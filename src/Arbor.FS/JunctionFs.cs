using System.Collections.Concurrent;
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

        public void CreateJunctionPoint(UPath junctionPoint, UPath target, bool overwrite)
        {
            _map.TryRemove(junctionPoint, out _);
            _map.TryAdd(junctionPoint, target);
        }

        protected override UPath ConvertPathToDelegate(UPath path)
        {
            if (_map.Keys.FirstOrDefault(key => path.IsInDirectory(key, recursive: true)) is {} foundPath &&
                foundPath.FullName?.Length > 0)
            {
                string newPath = path.FullName.Replace(foundPath.FullName, _map[foundPath].FullName);

                return new UPath(newPath);
            }

            if (_map.TryGetValue(path, out var mappedPath))
            {
                return mappedPath;
            }

            return path;
        }

        protected override UPath ConvertPathFromDelegate(UPath path) => path;
    }
}