﻿namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Components;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.Runtime.ObjectPool.Extensions;
    using UnityEngine;
    using Object = UnityEngine.Object;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ProcessDestroySilentSystem : IProtoRunSystem, IProtoInitSystem
    {
        private ProtoWorld _world;
        private EcsFilter _requestFilter;
        
        private ProtoPool<TransformComponent> _transformPool;
        private ProtoPool<GameObjectComponent> _gameObjectPool;
        private ProtoPool<DestroySelfRequest> _destroyRequestPool;
        private ProtoPool<PoolingComponent> _pooledPool;

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();

            _requestFilter = _world.Filter<DestroySelfRequest>().End();
        }

        public void Run()
        {
            foreach (var entity in _requestFilter)
            {
                var packedEntity = _world.PackEntity(entity);
                
                if(!packedEntity.Unpack(_world,out var _)) continue;
                
                ref var request = ref _destroyRequestPool.Get(entity);
                var isTransform = _transformPool.Has(entity);
                var isGameObject = _gameObjectPool.Has(entity);
                
                GameObject gameObject = null;

                var usePooling = false && _pooledPool.Has(entity) && request.ForceDestroy == false;
                
                if (isGameObject)
                {
                    ref var gameObjectComponent = ref _gameObjectPool.Get(entity);
                    gameObject = gameObjectComponent.Value;
                }
                else if (isTransform)
                {
                    ref var transformComponent = ref _transformPool.Get(entity);
                    var transform = transformComponent.Value;
                    gameObject = transform?.gameObject;
                }
                
                if (gameObject == null)
                {
                    _world.DelEntity(entity);
                    continue;
                }

                if (usePooling)
                {
                    gameObject.Despawn();
                    continue;
                }
                
                _world.DelEntity(entity);
                gameObject.SetActive(false);
                Object.Destroy(gameObject);
            }
        }
    }
}