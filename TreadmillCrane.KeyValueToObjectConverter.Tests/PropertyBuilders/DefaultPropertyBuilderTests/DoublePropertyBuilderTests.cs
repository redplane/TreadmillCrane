using NUnit.Framework;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;

namespace TreadmillCrane.KeyValueToObjectConverter.Tests.PropertyBuilders.DefaultPropertyBuilderTests
{
    [TestFixture]
    public class DoublePropertyBuilderTests
    {
        #region Methods
        
        /// <summary>
        /// Pass a valid string about integer into double property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleValidDoubleIntoNonNullableIntProperty_Returns_ValidDouble()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(double), "1")
                .Result;

            Assert.AreEqual(1, handledResult);
        }

        /// <summary>
        /// Pass a valid string about integer into nullable double property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleValidIntegerIntoNullableIntProperty_Returns_ValidDouble()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(double?), "1")
                .Result;

            Assert.AreEqual(1, handledResult);
        }

        /// <summary>
        /// Pass an invalid string to an non-nullable double field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleInvalidIntegerIntoNonNullableIntProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () => 
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(double), "this is not an double"));
        }

        /// <summary>
        /// Pass an invalid string to an nullable double field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleInvalidIntegerIntoNullableIntProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(double?), "this is not an double"));
        }

        #endregion
    }
}