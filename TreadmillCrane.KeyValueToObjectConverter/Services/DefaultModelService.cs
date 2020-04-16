using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TreadmillCrane.KeyValueToObjectConverter.Interfaces;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;

namespace TreadmillCrane.KeyValueToObjectConverter.Services
{
    public class DefaultModelService : IModelService
    {
        #region Properties

        private readonly IModelPropertyBuilder[] _modelPropertyBuilders;

        #endregion

        #region Constructor

        public DefaultModelService()
        {
            var defaultModelPropertyBuilders = new LinkedList<IModelPropertyBuilder>();
            defaultModelPropertyBuilders.AddLast(new EnumPropertyBuilder());
            defaultModelPropertyBuilders.AddLast(new GuidPropertyBuilder());
            defaultModelPropertyBuilders.AddLast(new DefaultPropertyBuilder());

            _modelPropertyBuilders = defaultModelPropertyBuilders.ToArray();
        }

        public DefaultModelService(IEnumerable<IModelPropertyBuilder> modelPropertyBuilders) : this()
        {
            var handledModelPropertyBuilders = modelPropertyBuilders as IModelPropertyBuilder[] ?? modelPropertyBuilders.ToArray();
            if (handledModelPropertyBuilders.Any())
                _modelPropertyBuilders = handledModelPropertyBuilders;
        }

        #endregion

        #region Methods

        public virtual async Task<T> BuildModelAsync<T>(IEnumerable<KeyValuePair<string, string>> modelKeyValuePairs, object value, CancellationToken cancellationToken = default)
        {
            var designatedType = typeof(T);

            // Not support abstract class deserialization.
            if (designatedType.IsAbstract)
                throw new NotSupportedException("Abstract type is not supported.");

            // Not support interface deserialization.
            if (designatedType.IsInterface || !designatedType.IsClass)
                throw new NotSupportedException("Only support concrete class deserialization.");

            // Create an instance from specific type.
            var instance = Activator.CreateInstance(designatedType);

            var keyValuePairs = modelKeyValuePairs as KeyValuePair<string, string>[] ?? modelKeyValuePairs.ToArray();
            if (!keyValuePairs.Any())
                return default;

            // Find parameter from content deposition.
            foreach (var modelKeyValuePair in keyValuePairs)
            {
                if (string.IsNullOrWhiteSpace(modelKeyValuePair.Key))
                    continue;

                var properties = GetModelProperties(modelKeyValuePair.Key);
                await ImplementModelAsync(instance, properties, value, cancellationToken);
            }

            return (T)instance;
        }

        /// <summary>
        /// Implement model from list of parameters.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameters"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task ImplementModelAsync(object model, IList<string> parameters, object value, CancellationToken cancellationToken = default)
        {
            // Initiate model pointer.
            var pointer = model;

            // Find the last key.
            //var lastKey = parameters[parameters.Count - 1];

            // Initiate property information.
            PropertyInfo propertyInfo = null;

            if (parameters == null || parameters.Count < 1)
                return;

            // Go through every part of parameters.
            // If the parameter name is : Items[0][list]. Parsed params will be : Items, 0, list.
            for (var index = 0; index < parameters.Count; index++)
            {
                // Find the next parameter index.
                // If the current parameter is : Item, the next param will be : 0
                var iNextIndex = index + 1;

                // Find parameter key.
                var key = parameters[index];

                // Numeric key is always about array.
                if (IsNumeric(key))
                {
                    // Invalid property info.
                    if (propertyInfo == null)
                        return;

                    // Current property information is not a list.
                    if (!IsList(propertyInfo.PropertyType))
                        return;

                    // Find the index of parameter.
                    if (!int.TryParse(key, out var iCollectionIndex))
                        iCollectionIndex = -1;

                    // This is the last key.
                    if (iNextIndex >= parameters.Count)
                    {
                        await InsertCollectionItemAsync(pointer, iCollectionIndex, propertyInfo, value);
                        return;
                    }

                    var val = await InsertCollectionItemAsync(pointer, iCollectionIndex, propertyInfo);
                    pointer = val;

                    // Find the property information of the next key.
                    var nextKey = parameters[iNextIndex];
                    propertyInfo = await GetPropertyInfoAsync(pointer, nextKey, cancellationToken);
                    continue;
                }

                // Find property of the current key.
                propertyInfo = await GetPropertyInfoAsync(pointer, key, cancellationToken);

                // Property doesn't exist.
                if (propertyInfo == null)
                    return;

                // This is the last parameter.
                if (iNextIndex >= parameters.Count)
                {
                    var modelValue =
                        await BuildModelPropertyAsync(propertyInfo.PropertyType, value, cancellationToken);

                    propertyInfo.SetValue(pointer, modelValue);
                    return;
                }

                // Find targeted value.
                var targetedValue = propertyInfo.GetValue(pointer);

                // Value doesn't exist.
                if (targetedValue == null)
                {
                    // Initiate property value.
                    var designatedTargetInstance = Activator.CreateInstance(propertyInfo.PropertyType);
                    targetedValue =
                        await BuildModelPropertyAsync(propertyInfo.PropertyType,
                            designatedTargetInstance, cancellationToken);

                    propertyInfo.SetValue(pointer, targetedValue);
                    pointer = targetedValue;
                    continue;
                }

                // Value is list.
                if (IsList(propertyInfo.PropertyType))
                {
                    pointer = propertyInfo.GetValue(pointer);
                    if (iNextIndex >= parameters.Count)
                        await InsertCollectionItemAsync(pointer, -1, propertyInfo, value);
                    continue;
                }

                // Go to next key
                pointer = targetedValue;
            }
        }

        /// <summary>
        /// Build model properties asynchronously.
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task<object> BuildModelPropertyAsync(Type propertyType, object value, CancellationToken cancellationToken = default)
        {
            // Output property value.
            var outputPropertyValue = value;

            if (propertyType == null)
                return outputPropertyValue;

            if (_modelPropertyBuilders == null || _modelPropertyBuilders.Length < 1)
                return outputPropertyValue;

            foreach (var modelPropertyBuilder in _modelPropertyBuilders)
            {
                if (modelPropertyBuilder == null)
                    continue;

                try
                {
                    var builtProperty =
                        await modelPropertyBuilder.BuildPropertyAsync(propertyType, value,
                            cancellationToken);

                    outputPropertyValue = builtProperty;
                }
                catch (IgnorePropertyBuildException)
                {
                }
            }

            return outputPropertyValue;
        }

        /// <summary>
        /// Get instance property info by searching for its name.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task<PropertyInfo> GetPropertyInfoAsync(object instance, string propertyName, CancellationToken cancellationToken = default)
        {
            var designatedPropertyInfo =
                instance.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => propertyName.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(designatedPropertyInfo);
        }

        /// <summary>
        ///     Add or update member of array.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="iCollectionIndex"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual async Task<object> InsertCollectionItemAsync(object pointer, int iCollectionIndex, PropertyInfo propertyInfo,
            object value = null)
        {
            // Current member is an array, normally, it will have count property.
            var itemCountProperty = propertyInfo.PropertyType.GetProperty(nameof(Enumerable.Count));
            if (itemCountProperty == null)
                return null;

            // Find items number in the list.
            var itemCount = (int)itemCountProperty.GetValue(pointer, null);

            // Get generic arguments from property.
            var genericArguments = propertyInfo.PropertyType.GetGenericArguments();

            // No generic argument has been found.
            if (genericArguments.Length < 1)
                return null;

            // Get the first argument.
            var genericArgument = genericArguments[0];

            // Build item to add to list.
            var listItem = await BuildModelPropertyAsync(genericArgument, value);

            // Current index is invalid to the array, this means we will add a new item to the list.
            // For example, the current array has 1 element, and the iCollectionIndex is 1.
            // The item at the index is invalid, therefore, new item will be created.
            if (iCollectionIndex < 0 || iCollectionIndex > itemCount - 1)
            {
                // Find the add method.
                var addProperty = propertyInfo.PropertyType.GetMethod(nameof(IList.Add));
                if (addProperty != null)
                    addProperty.Invoke(pointer, new[] { listItem });
                return listItem;
            }

            // If the collection index is valid.
            // For example, list contains 2 element, and we are accessing the first element. This is ok, we can do it by searching for that element and set the property value.
            var elementAtMethod = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ElementAt));

            if (elementAtMethod != null)
            {
                var item = elementAtMethod.MakeGenericMethod(genericArguments);
                return item.Invoke(pointer, new[] { pointer, iCollectionIndex });
            }

            return null;
        }

        /// <summary>
        ///     Check whether text is only numeric or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual bool IsNumeric(string text)
        {
            var regexNumeric = new Regex("^[0-9]*$");
            return regexNumeric.IsMatch(text);
        }

        /// <summary>
        ///     Whether instance is a collection or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool IsList(Type type)
        {
            if (!type.IsGenericType)
                return false;

            return type.GetInterface(typeof(IEnumerable<>).FullName) != null;
        }

        /// <summary>
        ///     Find content disposition parameters
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetModelProperties(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return text.Trim()
                .Split(':')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        #endregion
    }
}