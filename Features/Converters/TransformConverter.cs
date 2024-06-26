﻿namespace Game.Ecs.Core.Converters
{
    using System;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Shared.Components;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;

    [Serializable]
    public class TransformConverter : GameObjectConverter
    {
        public bool addPosition = true;
        public bool addDirection = true;
        
        protected override void OnApply(
            GameObject target,
            ProtoWorld world,
            ProtoEntity entity)
        {
            ref var transformComponent = ref world.GetOrAddComponent<TransformComponent>(entity);
            
            if (addPosition)
            {
                ref var transformPositionComponent = ref world.GetOrAddComponent<TransformPositionComponent>(entity);
            }

            if (addDirection)
            {
                ref var transformDirectionComponent = ref world.GetOrAddComponent<TransformDirectionComponent>(entity);
            }
        }
    }
}