using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Controller;
using CueX.Core.Subscription;
using CueX.Geometry;

namespace CueX.Test.Grains
{
    public class SpatialGrainStub : ISpatialGrain
    {
        private IController _controller;
        private Vector3d _position;
        
        public Task SetController(IController controller)
        {
            _controller = controller;
            return Task.CompletedTask;
        }

        public Task SetPosition(Vector3d newPosition)
        {
            _position = newPosition;
            return Task.CompletedTask;
        }

        public Task<Vector3d> GetPosition()
        {
            return Task.FromResult(_position);
        }

        public Task<bool> Destroy()
        {
            return Task.FromResult(true);
        }

        public Task ReceiveSpatialEvent<T>(T spatialEvent) where T : SpatialEvent
        {
            return Task.CompletedTask;
        }

        public Task ReceiveControlEvent<T>(T controlEvent) where T : ControlEvent
        {
            return Task.CompletedTask;
        }
    }
}