﻿namespace Game.Ecs.Core.Components
{
    using System;
    using Leopotam.EcsProto.QoL;
    using UnityEngine.Serialization;

    /// <summary>
    /// owner entity
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct OwnerComponent
    {
#if ODIN_INSPECTOR
        [FormerlySerializedAs("Entity")] 
        [Sirenix.OdinInspector.OnInspectorGUI]
#endif
        public ProtoPackedEntity Value;
    }
}