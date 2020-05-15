using Zio;
using Zio.FileSystems;

namespace Arbor.FS
{
    public class WindowsFs : ComposeFileSystem
    {
        public WindowsFs(PhysicalFileSystem fileSystem, bool owned = true) : base(fileSystem, owned)
        {
        }

        protected override UPath ConvertPathToDelegate(UPath path) => path;

        protected override UPath ConvertPathFromDelegate(UPath path) => path;
    }
}