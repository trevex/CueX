// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.MathExt.LinearAlgebra;
using Orleans;


namespace CueX.API
{
    public interface ISpatialGrain : IGrainWithIntegerKey
    {
        Task<Vector3d> GetPosition();
    }

}