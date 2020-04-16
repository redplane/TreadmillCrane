using System;
using System.Threading;
using System.Threading.Tasks;

namespace TreadmillCrane.KeyValueToObjectConverter.Interfaces
{
    public interface IModelPropertyBuilder
    {
        #region Properties

        /// <summary>
        /// Build request model value from property information and value.
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> BuildPropertyAsync(Type propertyType, object value, CancellationToken cancellationToken = default);

        #endregion
    }
}