namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.ViewSystem.Runtime;
    using UnityEngine;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class ViewServiceInitSystem : IProtoInitSystem
    {
        private readonly IGameViewSystem _gameViewSystem;
        private ProtoWorld _world;

        public ViewServiceInitSystem(IGameViewSystem gameViewSystem)
        {
            _gameViewSystem = gameViewSystem;
        }

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            var entity = _world.NewEntity();
            ref var component = ref _world.AddComponent<ViewServiceComponent>(entity);
            component.ViewSystem = _gameViewSystem;
            
            GameLog.Log($"{nameof(ViewServiceComponent)} Created",Color.green);
        }
    }
}
