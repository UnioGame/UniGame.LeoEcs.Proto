﻿namespace UniGame.LeoEcs.ViewSystem.Layouts.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.ViewSystem.Runtime;

    /// <summary>
    /// register new layout into view system
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class RegisterNewViewLayoutSystem : IProtoInitSystem, IProtoRunSystem
    {
        private IGameViewSystem _viewSystem;
        private ProtoWorld _world;

        private ViewLayoutAspect _layout;
        private EcsFilter _layoutFilter;

        public RegisterNewViewLayoutSystem(IGameViewSystem viewSystem)
        {
            _viewSystem = viewSystem;
        }
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();

            _layoutFilter = _world
                .Filter<ViewLayoutComponent>()
                .Inc<RegisterViewLayoutSelfRequest>()
                .End();
        }

        public void Run()
        {
            foreach (var entity in _layoutFilter)
            {
                ref var layoutComponent = ref _layout.Layout.Get(entity);
                _viewSystem.RegisterLayout(layoutComponent.Id, layoutComponent.Layout);
            }
        }
    }
}