using NUnit.Framework;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;

namespace TreadmillCrane.KeyValueToObjectConverter.Tests.PropertyBuilders.DefaultPropertyBuilderTests
{
    [TestFixture]
    public class BooleanPropertyTests
    {
        #region Methods

        /// <summary>
        /// Pass number 1 about boolean into boolean property.
        /// -> Value is handled successfully as true.
        /// </summary>
        [Test]
        public void HandlePositiveIntoNonNullableBooleanProperty_Returns_True()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool), 1)
                .Result;

            Assert.AreEqual(true, handledResult);
        }

        /// <summary>
        /// Pass number 1 about boolean into boolean property.
        /// -> Value is handled successfully as true.
        /// </summary>
        [Test]
        public void HandlePositiveIntegerIntoNullableBooleanProperty_Returns_True()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool?), 1)
                .Result;

            Assert.AreEqual(true, handledResult);
        }

        /// <summary>
        /// Pass zero into non-nullable boolean property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleZeroIntegerIntoNonNullableBooleanProperty_Returns_False()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool), 0)
                .Result;

            Assert.AreEqual(false, handledResult);
        }

        /// <summary>
        /// Pass zero into nullable boolean property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleZeroIntegerIntoNullableBooleanProperty_Returns_False()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool?), 0)
                .Result;

            Assert.AreEqual(false, handledResult);
        }

        /// <summary>
        /// Pass true string into non-nullable boolean property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleTrueStringIntoNonNullableBooleanProperty_Returns_True()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool), "true")
                .Result;

            Assert.AreEqual(true, handledResult);
        }

        /// <summary>
        /// Pass true string into nullable boolean property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleTrueStringIntoNullableBooleanProperty_Returns_False()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool?), "true")
                .Result;

            Assert.AreEqual(true, handledResult);
        }

        /// <summary>
        /// Pass false string into non-nullable boolean property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleFalseStringIntoNonNullableBooleanProperty_Returns_False()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool), "false")
                .Result;

            Assert.AreEqual(false, handledResult);
        }

        /// <summary>
        /// Pass false string into nullable boolean property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleFalseStringIntoNullableBooleanProperty_Returns_False()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool?), "false")
                .Result;

            Assert.AreEqual(false, handledResult);
        }

        /// <summary>
        /// Pass empty string into non-nullable boolean property.
        /// -> IgnorePropertyBuildException is thrown
        /// </summary>
        [Test]
        public void HandleEmptyStringIntoNonNullableBooleanProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();

            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(bool), ""));
        }

        /// <summary>
        /// Pass empty string into nullable boolean property.
        /// -> Returns null
        /// </summary>
        [Test]
        public void HandleEmptyStringIntoNullableBooleanProperty_Returns_Null()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(bool?), "").Result;
            Assert.IsNull(handledResult);
        }

        /// <summary>
        /// Pass invalid string into non-nullable boolean property.
        /// -> IgnorePropertyBuildException is thrown
        /// </summary>
        [Test]
        public void HandleInvalidStringIntoNonNullableBooleanProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();

            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(bool), "this is invalid string"));
        }

        /// <summary>
        /// Pass invalid string into nullable boolean property.
        /// -> IgnorePropertyBuildException is thrown
        /// </summary>
        [Test]
        public void HandleInvalidStringIntoNullableBooleanProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();

            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(bool?), "this is invalid string"));
        }

        #endregion
    }
}