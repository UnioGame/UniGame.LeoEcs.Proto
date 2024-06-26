﻿namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class CloseAllViewsSystem : IProtoInitSystem,IProtoRunSystem
    {
        private readonly IGameViewSystem _gameViewSystem;
        private ProtoWorld _world;

        private EcsFilter _closeAllFilter;

        public CloseAllViewsSystem(IGameViewSystem gameViewSystem)
        {
            _gameViewSystem = gameViewSystem;
        }

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _closeAllFilter = _world.Filter<CloseAllViewsRequest>().End();
        }
        
        public void Run()
        {
            foreach (var entity in _closeAllFilter)
            {
                _gameViewSystem.CloseAll();
                break;
            }
        }
    }
}