using NUnit.Framework;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;

namespace TreadmillCrane.KeyValueToObjectConverter.Tests.PropertyBuilders.DefaultPropertyBuilderTests
{
    [TestFixture]
    public class IntegerPropertyBuilderTests
    {
        #region Methods

        /// <summary>
        /// Pass a valid string about integer into int property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleValidIntegerIntoNonNullableIntProperty_Returns_ValidInteger()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(int), "1")
                .Result;

            Assert.AreEqual(1, handledResult);
        }

        /// <summary>
        /// Pass a valid string about integer into nullable int property.
        /// -> Value is handled successfully.
        /// </summary>
        [Test]
        public void HandleValidIntegerIntoNullableIntProperty_Returns_ValidInteger()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(int?), "1")
                .Result;

            Assert.AreEqual(1, handledResult);
        }

        /// <summary>
        /// Pass an invalid string to an non-nullable integer field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleInvalidIntegerIntoNonNullableIntProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(int), "this is not an integer"));
        }

        /// <summary>
        /// Pass an invalid string to an nullable integer field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleInvalidIntegerIntoNullableIntProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(int?), "this is not an integer"));
        }

        /// <summary>
        /// Pass an out of range integer inside string to an non-nullable integer field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleOutOfRangeMaxIntegerInNonNullableInt_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(int), $"{double.MaxValue}"));
        }

        /// <summary>
        /// Pass an out of range integer inside string to an nullable integer field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleOutOfRangeMaxIntegerNullableInt_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(int?), $"{double.MaxValue}"));
        }

        /// <summary>
        /// Pass an out of range integer inside string to an non-nullable integer field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleOutOfRangeMinIntegerInNonNullableInt_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(int), $"{double.MinValue}"));
        }

        /// <summary>
        /// Pass an out of range integer inside string to an nullable integer field.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleOutOfRangeMinIntegerInNullableInt_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(int?), $"{double.MinValue}"));
        }

        #endregion
    }
}