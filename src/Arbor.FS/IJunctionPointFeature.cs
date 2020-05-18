using Zio;

namespace Arbor.FS
{
    public interface IJunctionPointFeature
    {
        void CreateJunctionPoint(JunctionPoint junctionPoint, bool overwrite);
        UPath GetJunctionTargetPath(UPath virtualPath);
        bool JunctionPointExists(UPath virtualPath);
        void DeleteJunctionPoint(UPath virtualPath);
    }
}