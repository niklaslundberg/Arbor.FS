using Zio;

namespace Arbor.FS
{
    public static class FileSystemExtensions
    {
        public static void CreateJunctionPoint(this IFileSystem fileSystem,
            UPath junctionPoint,
            UPath target,
            bool overwrite)
        {
            if (fileSystem is IJunctionPointFeature junctionPointFeature)
            {
                junctionPointFeature.CreateJunctionPoint(junctionPoint, target, overwrite);
            }
        }
    }
}