using System.Numerics;

namespace CueX.Core
{ // TODO: refactor into SPS base class?
    public static class Setup
    {
        public static void InitializeCommon()
        {
            if (!Vector.IsHardwareAccelerated)
            {
                // TODO: throw warning
            }
        }
    }
}