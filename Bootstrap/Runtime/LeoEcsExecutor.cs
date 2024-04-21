﻿namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UnityEngine;

    [Serializable]
    public class LeoEcsExecutor : ILeoEcsExecutor
    {
        private List<IProtoSystems> _systems = new();
        private ProtoWorld _world;
        private LeoEcsPlayerUpdateType _loopTiming = LeoEcsPlayerUpdateType.Update;
        private PlayerLoopTiming _updateTiming = PlayerLoopTiming.Update;

        private bool _isActive;
        private bool _isDisposed;

        public bool IsActive => _isActive;

        public LeoEcsExecutor(LeoEcsPlayerUpdateType updateType)
        {
            _loopTiming = updateType;
        }

        public void Execute(ProtoWorld world)
        {
            if (!CanExecute()) return;

            _world = world;
            _isActive = true;
            _updateTiming = _loopTiming.ConvertToPlayerLoopTiming();
            
            var worldLifeTime = _world.GetWorldLifeTime();

            ExecuteAsync()
                .AttachExternalCancellation(worldLifeTime.Token)
                .Forget();
        }

        public void Add(IProtoSystems systems)
        {
            _systems.Add(systems);
        }

        public void Stop() => _isActive = false;

        public void Dispose()
        {
            Stop();

            _systems.Clear();
            _isDisposed = true;
            _isActive = false;
        }

        private bool CanExecute()
        {
            var isDisabled = _isDisposed ||
                             _isActive ||
                             _loopTiming == LeoEcsPlayerUpdateType.None;
            var canExecute = !isDisabled;

            return canExecute;
        }

        private async UniTask ExecuteAsync()
        {
            while (_world.IsAlive() && Application.isPlaying && _isActive)
            {
                foreach (var system in _systems)
                {
                    system.Run();
                }
                
                //wait next interval
                await UniTask.Yield(_updateTiming);
            }
        }
    }
}