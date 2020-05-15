using Zio;

namespace Arbor.FS
{
    public interface IJunctionPointFeature
    {
        void CreateJunctionPoint(UPath junctionPoint, UPath target, bool overwrite);
    }
}