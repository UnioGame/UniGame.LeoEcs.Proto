﻿namespace UniGame.LeoEcs.Shared.Components
{
    using System;
    using Converter.Runtime;
    using Extensions;
    using Leopotam.EcsProto;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    /// <summary>
    /// renderer converter
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class RendererConverter : GameObjectConverter
    {
        public Renderer renderer;
        
        protected override void OnApply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            var render = renderer != null ? renderer : target.GetComponent<Renderer>();
            if(render == null) return;

            ref var renderComponent = ref world.GetOrAddComponent<RendererComponent>(entity);
            ref var visibleComponent = ref world.GetOrAddComponent<RendererVisibleComponent>(entity);

            renderComponent.Value = render;
            visibleComponent.Value = render.isVisible;

            if (render.enabled)
            {
                ref var activeComponent = ref world.GetOrAddComponent<RendererEnabledComponent>(entity);
            }
        }

        
    }
}