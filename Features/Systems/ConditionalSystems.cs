﻿namespace Game.Ecs.Core.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;

    /// <summary>
    /// execute system if condition is true
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ConditionalSystems : IProtoInitSystem, IProtoRunSystem, IEcsDestroySystem
    {
        private ProtoWorld _world;
        private bool _takeOnce;
        private bool _valueUpdated;
        private bool _value;
        
        private List<IProtoRunSystem> _runSystems = new();
        private List<IProtoInitSystem> _initSystems = new();
        private List<IEcsDestroySystem> _destroySystems = new();

        public ConditionalSystems(IEnumerable<IEcsSystem> systems,IProtoSystems group,bool takeOnce = false)
        {
            _takeOnce = takeOnce;
            foreach (var ecsSystem in systems)
            {
                group.Add(ecsSystem);
                if (ecsSystem is IProtoRunSystem runSystem)
                    _runSystems.Add(runSystem);
                if (ecsSystem is IProtoInitSystem initSystem)
                    _initSystems.Add(initSystem);
                if (ecsSystem is IEcsDestroySystem destroySystem)
                    _destroySystems.Add(destroySystem);
            }
        }

        public virtual bool Evaluate() => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Recalculate()
        {
            if (_valueUpdated && _takeOnce) return _value;
            
            _valueUpdated = true;
            _value = Evaluate();
            
            return _value;
        }
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();

            foreach (var initSystem in _initSystems)
                initSystem.Init(systems);
        }

        public void Run()
        {
            var value = Recalculate();
            
            if (!value) return;
            
            foreach (var runSystem in _runSystems)
                runSystem.Run();
        }

        public void Destroy()
        {
            foreach (var destroySystem in _destroySystems)
                destroySystem.Destroy();
        }
    }
}