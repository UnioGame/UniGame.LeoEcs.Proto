﻿namespace UniGame.LeoEcs.Shared.Components
{
    using System;
    using Bootstrap.Runtime.Abstract;
    using Leopotam.EcsProto;

    [Serializable]
    public class RendererAspect : EcsAspect
    {
        public ProtoPool<RendererComponent> Render;
        public ProtoPool<RendererEnabledComponent> Enabled;
        public ProtoPool<RendererVisibleComponent> Visible;
    }
}