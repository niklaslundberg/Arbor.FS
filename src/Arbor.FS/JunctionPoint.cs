using Zio;

namespace Arbor.FS
{
    public class JunctionPoint
    {
        public JunctionPoint(UPath virtualPath, UPath targetPath)
        {
            VirtualPath = virtualPath;
            TargetPath = targetPath;
        }

        public UPath VirtualPath { get; }

        public UPath TargetPath { get; }
    }
}