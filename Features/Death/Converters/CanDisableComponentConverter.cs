﻿namespace Game.Ecs.Core.Death.Converters
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;

    [Serializable]
    public class CanDisableComponentConverter : EcsComponentConverter
    {
        [SerializeField] 
        public bool addDisabledOnAwake = false;
        
        public override void Apply(ProtoWorld world, ProtoEntity entity)
        {
            world.AddComponent<CanDisableComponent>(entity);

            if (addDisabledOnAwake)
                world.AddComponent<DisabledComponent>(entity);
        }
    }
}