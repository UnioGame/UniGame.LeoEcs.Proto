﻿namespace Game.Ecs.UI.EndGameScreens.Systems
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.ViewSystem.Components;
    using UniGame.LeoEcs.ViewSystem.Extensions;
    using UniGame.ViewSystem.Runtime;

    /// <summary>
    /// request to show view in container
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CloseOnSystem<TEvent,TViewModel> : IProtoRunSystem
        where TEvent : struct
        where TViewModel : IViewModel
    {
        private ProtoWorld _world;
        
        private ProtoIt _eventFilter = It
            .Chain<TEvent>()
            .End();
        
        private ProtoIt _viewFilter = ViewIt
            .ViewChain()
            .End();

        private ProtoPool<ViewComponent> _viewPool;

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                foreach (var viewEntity in _viewFilter)
                {
                    ref var viewComponent = ref _viewPool.Get(viewEntity);
                    viewComponent.View.Close();
                }
            }
        }
    }
}