// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using CueX.Core;

namespace CueX.GridSPS.Config
{
    public class GridConfiguration : Configuration
    {
        private double _partitionSize = 0d;
        public double PartitionHalfDiagonal { get; private set; } = 0d;
        public double PartitionSize
        {
            get => _partitionSize;
            set
            {
                _partitionSize = value;
                PartitionHalfDiagonal = Math.Sqrt(Math.Pow(_partitionSize * 0.5d, 2d) * 2d);
            }
        }

        public static GridConfiguration Default()
        {
            return new GridConfiguration();
        }
    }
}