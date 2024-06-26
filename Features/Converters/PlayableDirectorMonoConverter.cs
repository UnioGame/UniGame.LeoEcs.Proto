﻿namespace Game.Ecs.Core.Converters
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;
    using UnityEngine.Playables;
    
    public sealed class PlayableDirectorMonoConverter : MonoLeoEcsConverter
    {
        [SerializeField]
        private PlayableDirector _playableDirector;
        
        public override void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            var playableDirectorPool = world.GetPool<PlayableDirectorComponent>();
            ref var playableDirectorComponent = ref playableDirectorPool.GetOrAddComponent(entity);

            playableDirectorComponent.Value = _playableDirector;
        }
    }
    
    [Serializable]
    public sealed class PlayableDirectorConverter : LeoEcsConverter,IConverterEntityDestroyHandler
    {
        [SerializeField]
        public PlayableDirector playableDirector;
        
        public override void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            var playableDirectorPool = world.GetPool<PlayableDirectorComponent>();
            ref var playableDirectorComponent = ref playableDirectorPool.GetOrAddComponent(entity);
            playableDirectorComponent.Value = playableDirector;
        }
        
        public void OnEntityDestroy(ProtoWorld world, ProtoEntity entity)
        {
            world.TryRemoveComponent<PlayableDirectorComponent>(entity);
        }
    }
    
    
}