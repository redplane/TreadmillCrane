using System;
using System.Threading;
using System.Threading.Tasks;
using TreadmillCrane.KeyValueToObjectConverter.Interfaces;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;

namespace TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders
{
    public class DefaultPropertyBuilder : IModelPropertyBuilder
    {
        #region Methods

        public virtual Task<object> BuildPropertyAsync(Type propertyType, object value, CancellationToken cancellationToken = default)
        {
            // Property is not defined.
            if (propertyType == null)
                return null;

            // Get property type.
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Other Nullable types
            if (underlyingType != null)
            {
                if (string.IsNullOrEmpty(value.ToString())) 
                    return Task.FromResult((object) null);
                propertyType = underlyingType;
            }

            try
            {
                object designatedInstance;
                if (value == null)
                    designatedInstance = Activator.CreateInstance(propertyType);
                else
                    designatedInstance = Convert.ChangeType(value, propertyType);

                return Task.FromResult(designatedInstance);
            }
            catch (FormatException)
            {
                throw new IgnorePropertyBuildException();
            }
            catch (InvalidCastException)
            {
                throw new IgnorePropertyBuildException();
            }
        }


        #endregion
    }
}