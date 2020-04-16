using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TreadmillCrane.KeyValueToObjectConverter.Interfaces
{
    public interface IModelService
    {
        #region Methods

        /// <summary>
        /// Build model from parameters asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelKeyValuePairs"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"/>
        /// <returns></returns>
        Task<T> BuildModelAsync<T>(IEnumerable<KeyValuePair<string, string>> modelKeyValuePairs, object value, CancellationToken cancellationToken = default);

        #endregion
    }
}