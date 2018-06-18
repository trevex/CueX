// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Geometry
{
    public interface IArea
    {
        void SetOrigin(Vector3d origin);
        Vector3d GetOrigin();
        double GetHalfBoundingBoxWidth();
        bool IsPointInside(Vector3d point);
    }
}