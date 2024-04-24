﻿namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.SerializableType;
    using Sirenix.OdinInspector;

    [Serializable]
    public class AspectsData
    {
        public bool autoRegisterAspects = true;
        
#if ODIN_INSPECTOR 
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [ListDrawerSettings(ListElementLabelName = "@name")]
#endif
        public List<AspectData> aspects = new List<AspectData>();
    }

    [Serializable]
    public class AspectData 
#if ODIN_INSPECTOR
        : ISearchFilterable
#endif
    {
        public string name;
        public bool enabled;
        public SType aspectType = new SType();
        
        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if(name.Contains(searchString)) return true;
            if(aspectType.TypeName!= null && aspectType.TypeName.Contains(searchString)) return true;
            return false;
        }
    }
}