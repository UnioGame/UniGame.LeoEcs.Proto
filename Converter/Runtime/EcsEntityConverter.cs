﻿namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Abstract;
    using Core.Runtime;
    using Core.Runtime.ScriptableObjects;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UnityEngine;

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    [CreateAssetMenu(menuName = "UniGame/LeoEcs/Converter/Entity Converter",fileName = "Entity Converter")]
    public class EcsEntityConverter : LifetimeScriptableObject
    {
        #region inspector
        
        [Tooltip("if true, when create entities for each converter")]
        public bool createEntityForEachConverter = true;

#if  ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineProperty]
#endif
        public List<ComponentConverterValue> converters = new List<ComponentConverterValue>();
        
        #endregion

        public async UniTask Create(ProtoWorld world,ILifeTime lifeTime)
        {
            if (!createEntityForEachConverter)
            {
                var entity = world.NewEntity();
                lifeTime.DestroyEntityWith(entity, world);
                
                await Create(entity, world);
                return;
            }
            
            foreach (var converter in converters)
            {
                var entity = world.NewEntity();
                lifeTime.DestroyEntityWith(entity, world);
                Convert((int)entity,world,converter).Forget();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask Create(ProtoEntity entity, ProtoWorld world)
        {
            await Create((int)entity, world);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask Create(int entity, ProtoWorld world)
        {
            await UniTask.WhenAll(converters.Select(x => Convert(entity, world, x)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private UniTask Convert(int entity, ProtoWorld world, IEcsComponentConverter converter)
        {
            if (!converter.IsEnabled)
                return UniTask.CompletedTask;
            
            converter.Apply(world,entity);
            
            return UniTask.CompletedTask;
        }
    }
}