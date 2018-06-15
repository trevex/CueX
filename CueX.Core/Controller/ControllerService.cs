// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace CueX.Core.Controller
{
    public class ControllerService : IControllerService
    {
        private IController _controller;
        
        public Task<IController> GetController()
        {
            return Task.FromResult(_controller);
        }

        public Task SetController(IController controller)
        {
            _controller = controller;
            return Task.CompletedTask;
        }
    }
}