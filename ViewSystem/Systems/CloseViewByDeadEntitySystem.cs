﻿namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using global::UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using global::UniModules.UniGame.UISystem.Runtime;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;

    /// <summary>
    /// close View if entity is dead
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CloseViewByDeadEntitySystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;
        private EcsFilter _filter;

        private ProtoPool<ViewEntityLifeTimeComponent> _lifeTimePool;

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();

            _filter = _world
                .Filter<ViewEntityLifeTimeComponent>()
                .End();
        }

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var lifeTimeComponent = ref _lifeTimePool.Get(entity);
                if(lifeTimeComponent.Value.Unpack(_world,out _))
                    continue;
                
                var view = lifeTimeComponent.View;
                if(!view.IsTerminated && view.Status.Value != ViewStatus.Closed)
                    view.Close();
    
                if(_lifeTimePool.Has(entity))
                    _lifeTimePool.Del(entity);
            }
        }
    }
}