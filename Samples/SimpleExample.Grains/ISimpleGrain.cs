using System.Threading.Tasks;
using CueX.API;
using CueX.Numerics;
using Orleans;

namespace SimpleExample.Grains
{
    public interface ISimpleGrain : ISpatialGrain, IGrainWithIntegerKey
    {
        Task SetPosition(Vector3d newPosition);
    }
}