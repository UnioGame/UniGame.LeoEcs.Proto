﻿namespace UniGame.LeoEcs.Debug.Editor
{
    using System;

    [Serializable]
    public class CheckProtoWorldStatusFilter : IProtoWorldSearchFilter
    {
        public const string worldIsEmptyMessage = 
            "World Not Found";

        public EcsFilterData Execute(EcsFilterData filterData)
        {
            if (filterData.world == null)
            {
                filterData.errorMessage = worldIsEmptyMessage;
                filterData.type = ResultType.Error;
            }

            return filterData;
        }
    }
}