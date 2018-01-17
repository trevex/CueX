using System.Numerics;
using Orleans;


namespace CueX.API
{
    public interface ISpatialGrain<T> : IGrainWithIntegerKey where T : struct
    {
        Vector<T> GetPosition();
    }
    
}