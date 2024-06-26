﻿namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ProcessKillRequestSystem : IProtoRunSystem, IProtoInitSystem
    {
        private EcsFilter _filter;
        private ProtoWorld _world;

        private ProtoPool<DestroyComponent> _deadPool;
        private ProtoPool<PoolingComponent> _poolingPool;
        private ProtoPool<DeadEvent> _deadEventPool;
        private ProtoPool<KillRequest> _killRequestPool;
        private ProtoPool<KillEvent> _killEventPool;

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<KillRequest>()
                .Exc<DestroyComponent>()
                .Exc<DontKillComponent>()
                .End();
        }

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var killRequest = ref _killRequestPool.Get(entity);

                var killEventEntity = _world.NewEntity();
                ref var killEvent = ref _killEventPool.Add(killEventEntity);
                killEvent.Source = killRequest.Source;
                killEvent.Destination = _world.PackEntity(entity);

                if (_poolingPool.Has(entity))
                {
                    _deadEventPool.TryAdd(entity);
                    continue;
                }

                _deadPool.TryAdd(entity);
                _deadEventPool.TryAdd(entity);
            }
        }
    }
}