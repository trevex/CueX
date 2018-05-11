// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core.Subscription;
using CueX.Geometry;
using Orleans;

namespace CueX.Core
{
    /// <summary>
    /// Interace that needs to be fulfilled by any spatial objects, that should be inserted into the PubSub-System.
    /// </summary>
    public interface ISpatialGrain : IGrain
    {
        Task SetPosition(Vector3d newPosition);
        Task<Vector3d> GetPosition();
        Task SetParent<T>(T parent) where T : Core.IPartitionGrain;
        Task<bool> RemoveSelfFromParent();
        Task ReceiveEvent(string eventType, SpatialEvent eventValue);
    }

}