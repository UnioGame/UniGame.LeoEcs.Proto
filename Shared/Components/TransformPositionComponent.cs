﻿namespace UniGame.LeoEcs.Shared.Components
{
    using System;
    using Unity.Mathematics;

    /// <summary>
    /// Component with single transform data 
    /// </summary>
    [Serializable]
    public struct TransformPositionComponent
    {
        public float3 Position;
        public float3 LocalPosition;
    }
}