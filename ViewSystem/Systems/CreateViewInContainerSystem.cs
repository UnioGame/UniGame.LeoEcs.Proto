﻿namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Components;
    using Shared.Extensions;

    /// <summary>
    /// listen request to create view in container and find container by id
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CreateViewInContainerSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;
        private EcsFilter _requestFilter;
        private EcsFilter _allContainersFilter;
        private EcsFilter _freeContainersFilter;
        
        private ProtoPool<CreateViewInContainerRequest> _createViewInContainerRequestPool;
        private ProtoPool<ViewContainerBusyComponent> _busyContainerPool;
        private ProtoPool<TransformComponent> _transformPool;
        private ProtoPool<ViewContainerComponent> _containerPool;
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();

            _requestFilter = _world
                .Filter<CreateViewInContainerRequest>()
                .End();

            _allContainersFilter = _world
                .Filter<ViewContainerComponent>()
                .Inc<TransformComponent>()
                .End();
            
            _freeContainersFilter = _world
                .Filter<ViewContainerComponent>()
                .Inc<TransformComponent>()
                .Exc<ViewContainerBusyComponent>()
                .End();
        }

        public void Run()
        {
            foreach (var requestEntity in _requestFilter)
            {
                ref var request = ref _createViewInContainerRequestPool.Get(requestEntity);
                var canUseBusyContainer = request.UseBusyContainer;
                
                var containerFilter = canUseBusyContainer 
                    ? _allContainersFilter 
                    : _freeContainersFilter;

                foreach (var containerEntity in containerFilter)
                {
                    ref var containerComponent = ref _containerPool.Get(containerEntity);
                    
                    //is container for target view
                    if (containerComponent.ViewId != request.View) continue;
      
                    ref var transformComponent = ref _transformPool.Get(containerEntity);
                    
                    //create view in container
                    ref var createViewRequest = ref _world.AddComponent<CreateViewRequest>(requestEntity);
                    
                    createViewRequest.ViewId = request.View;
                    createViewRequest.ViewName = request.ViewName;
                    createViewRequest.Tag = request.Tag;
                    createViewRequest.Owner = request.Owner;
                    createViewRequest.StayWorld = request.StayWorld;
                    createViewRequest.Parent = transformComponent.Value;
                    createViewRequest.LayoutType =string.Empty;
                    createViewRequest.Target = requestEntity.PackEntity(_world);
              
                    //mark container as busy
                    _busyContainerPool.GetOrAddComponent(containerEntity);
                    
                    //remove request only when container found
                    _createViewInContainerRequestPool.TryRemove(requestEntity);
                    
                    break;
                }
            }
        }
    }
}