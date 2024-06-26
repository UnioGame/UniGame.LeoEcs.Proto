﻿namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Extensions;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class CloseViewSystem : IProtoInitSystem,IProtoRunSystem
    {
        private ProtoWorld _world;

        private EcsFilter _closeFilter;
        private ProtoPool<ViewComponent> _viewComponent;

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            
            _closeFilter = _world
                .Filter<CloseViewSelfRequest>()
                .Inc<ViewComponent>()
                .End();
            
            _viewComponent = _world.GetPool<ViewComponent>();
        }
        
        public void Run()
        {
            foreach (var entity in _closeFilter)
            {
                ref var viewComponent = ref _viewComponent.Get(entity);
                viewComponent.View.Close();
                break;
            }
        }
    }
}