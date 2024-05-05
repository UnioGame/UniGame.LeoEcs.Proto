﻿namespace Game.Ecs.Core.Timer.Systems
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.Timer.Components;
    using UniGame.LeoEcs.Timer.Components.Events;

    /// <summary>
    /// update active timer
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class AutoRestartTimerSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoWorld _world;
        private EcsFilter _filter;
        
        private TimerAspect _timerAspect;

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            
            _filter = _world
                .Filter<CooldownComponent>()
                .Inc<CooldownAutoRestartComponent>()
                .Inc<CooldownFinishedSelfEvent>()
                .End();
        }

        public void Run()
        {
            foreach (var entity in _filter)
            {
                _timerAspect.Restart.GetOrAddComponent(entity);
            }
        }
    }
}