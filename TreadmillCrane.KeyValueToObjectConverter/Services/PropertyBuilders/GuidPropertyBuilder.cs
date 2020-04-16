using System;
using System.Threading;
using System.Threading.Tasks;
using TreadmillCrane.KeyValueToObjectConverter.Interfaces;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;

namespace TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders
{
    public class GuidPropertyBuilder : IModelPropertyBuilder
    {
        #region Methods

        public virtual Task<object> BuildPropertyAsync(Type propertyType, object value, CancellationToken cancellationToken = default)
        {
            // Get property type.
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Property is GUID.
            if (propertyType == typeof(Guid) && Guid.TryParse(value.ToString(), out var guid))
                return Task.FromResult((object) guid);

            if (underlyingType == typeof(Guid))
            {
                if (Guid.TryParse(value?.ToString(), out guid))
                    return Task.FromResult((object)guid);

                return Task.FromResult(default(object));
            }

            throw new IgnorePropertyBuildException();
        }


        #endregion
    }
}