using System;
using System.Threading;
using System.Threading.Tasks;
using TreadmillCrane.KeyValueToObjectConverter.Interfaces;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;

namespace TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders
{
    public class EnumPropertyBuilder : IModelPropertyBuilder
    {
        #region Methods

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<object> BuildPropertyAsync(Type propertyType, object value,
            CancellationToken cancellationToken)
        {
            if (value == null)
                throw new IgnorePropertyBuildException();

            // Get property type.
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Property is Enum.
            if (propertyType.IsEnum)
                try
                {
                    var handledValue = ConvertToEnum(propertyType, value.ToString());
                    return Task.FromResult(handledValue);
                }
                catch
                {
                    var defaultValue = Activator.CreateInstance(propertyType);
                    return Task.FromResult(defaultValue);
                }

            if (underlyingType != null && underlyingType.IsEnum)
            {
                if (string.IsNullOrWhiteSpace(value.ToString()))
                    throw new IgnorePropertyBuildException();

                try
                {
                    var handledValue = ConvertToEnum(underlyingType, value.ToString());
                    return Task.FromResult(handledValue);
                }
                catch (IgnorePropertyBuildException)
                {
                    return Task.FromResult((object) null);
                }
            }

            throw new IgnorePropertyBuildException();
        }

        /// <summary>
        ///     Convert a value to an enum.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected virtual object ConvertToEnum(Type type, string val)
        {
            object handledEnum = null;

            if (int.TryParse(val, out var num))
                handledEnum = Enum.ToObject(type, num);
            else
                handledEnum = Enum.Parse(type, val, true);

            if (!Enum.IsDefined(type, handledEnum))
                throw new IgnorePropertyBuildException();

            return handledEnum;
        }

        #endregion
    }
}