﻿namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using System;
    using Game.Ecs.Core.Components;
    using Leopotam.EcsProto;
    using Shared.Components;

    [Serializable]
    public class UnityAspect : EcsAspect
    {
        public ProtoPool<GameObjectComponent> GameObject;
        public ProtoPool<AssetComponent> Asset;
        public ProtoPool<RotationComponent> QuaternionRotation;
        public ProtoPool<TransformComponent> Transform;
        public ProtoPool<TransformPositionComponent> Position;
        public ProtoPool<TransformDirectionComponent> Direction;
        public ProtoPool<TransformScaleComponent> Scale;
        public ProtoPool<TransformRotationComponent> Rotation;
        public ProtoPool<SpriteComponent> Sprite;
    }
}