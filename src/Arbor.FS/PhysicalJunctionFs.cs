using System.IO;
using Arbor.FS.Ntfs;
using Zio;
using Zio.FileSystems;

namespace Arbor.FS
{
    public class PhysicalJunctionFs : ComposeFileSystem, IJunctionPointFeature
    {
        public PhysicalJunctionFs(WindowsFs fileSystem, bool owned = true) : base(fileSystem, owned)
        {
        }

        public void CreateJunctionPoint(JunctionPoint junctionPoint, bool overwrite)
        {
            string virtualPath = junctionPoint.VirtualPath.WindowsPath();
            string targetPath = junctionPoint.TargetPath.WindowsPath();
            FileSystemJunctionPoint.Create(virtualPath, targetPath, overwrite);
        }

        public UPath GetJunctionTargetPath(UPath virtualPath)
        {
            string windowsVirtualPath = virtualPath.WindowsPath();

            if (!JunctionPointExists(virtualPath))
            {
                throw new IOException("There is no junction path for " + virtualPath.FullName);
            }

            string fullPath = FileSystemJunctionPoint.GetTarget(windowsVirtualPath);

            return new UPath(fullPath);
        }

        public bool JunctionPointExists(UPath virtualPath) => FileSystemJunctionPoint.Exists(virtualPath.WindowsPath());

        public void DeleteJunctionPoint(UPath virtualPath) => FileSystemJunctionPoint.Delete(virtualPath.WindowsPath());

        protected override UPath ConvertPathToDelegate(UPath path) => path;

        protected override UPath ConvertPathFromDelegate(UPath path) => path;
    }
}