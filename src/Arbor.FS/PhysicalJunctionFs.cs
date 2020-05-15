using Arbor.FS.CreateMaps;
using Zio;
using Zio.FileSystems;

namespace Arbor.FS
{
    public class PhysicalJunctionFs : ComposeFileSystem, IJunctionPointFeature
    {
        public PhysicalJunctionFs(WindowsFs fileSystem, bool owned = true) : base(fileSystem, owned)
        {
        }

        public void CreateJunctionPoint(UPath junctionPoint, UPath target, bool overwrite)
        {
            string actualPath = junctionPoint.WindowsPath();
            string targetPath = target.WindowsPath();
            JunctionPoint.Create(actualPath, targetPath, overwrite);
        }

        protected override UPath ConvertPathToDelegate(UPath path) => path;

        protected override UPath ConvertPathFromDelegate(UPath path) => path;
    }
}