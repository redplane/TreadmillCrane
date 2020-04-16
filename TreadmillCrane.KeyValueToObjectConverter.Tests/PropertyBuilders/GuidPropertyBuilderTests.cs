using System;
using NUnit.Framework;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;

namespace TreadmillCrane.KeyValueToObjectConverter.Tests.PropertyBuilders
{
    [TestFixture]
    public class GuidPropertyBuilderTests
    {
        #region Methods

        /// <summary>
        /// Pass a valid guid string into non-nullable guid.
        /// -> Guid can be handled successfully.
        /// </summary>
        [Test]
        public void HandleValidGuid_Returns_Guid()
        {
            var guidPropertyBuilder = new GuidPropertyBuilder();
            var guid = Guid.NewGuid();
            var result = guidPropertyBuilder.BuildPropertyAsync(typeof(Guid), guid.ToString("D"))
                .Result;

            Assert.AreEqual(guid, result);
        }

        /// <summary>
        /// Pass an invalid guid string.
        /// -> IgnorePropertyBuildException should be thrown.
        /// </summary>
        [Test]
        public void HandleInvalidGuid_Throws_IgnorePropertyBuildException()
        {
            var guidPropertyBuilder = new GuidPropertyBuilder();
            var guid = "this is a string";

            Assert.CatchAsync<IgnorePropertyBuildException>(async () => await guidPropertyBuilder.BuildPropertyAsync(typeof(Guid), guid));
        }

        /// <summary>
        /// Pass an empty guid into non-nullable guid property.
        /// -> Guid property will be an empty guid.
        /// </summary>
        [Test]
        public void HandleEmptyGuid_Returns_EmptyGuid()
        {
            var guidPropertyBuilder = new GuidPropertyBuilder();
            var guid = Guid.Empty;

            var result = guidPropertyBuilder.BuildPropertyAsync(typeof(Guid), guid.ToString("D"))
                .Result;

            Assert.AreEqual(guid, result);
        }

        /// <summary>
        /// Pass empty text into nullable guid field.
        /// -> Property will be null.
        /// </summary>
        [Test]
        public void HandleNullableGuidWithEmptyValue_Returns_Null()
        {
            var guidPropertyBuilder = new GuidPropertyBuilder();
            var result = guidPropertyBuilder.BuildPropertyAsync(typeof(Guid?), "").Result;
            Assert.IsNull(result);
        }

        /// <summary>
        /// Pass invalid string into nullable guid property.
        /// -> Property null be null.
        /// </summary>
        [Test]
        public void HandleNullableGuidWithInvalidValue_Returns_Null()
        {
            var guidPropertyBuilder = new GuidPropertyBuilder();
            var result = guidPropertyBuilder.BuildPropertyAsync(typeof(Guid?), "this is a string").Result;
            Assert.IsNull(result);
        }

        /// <summary>
        /// Pass Guid.Empty into nullable guid property.
        /// -> Property will be Guid.Empty.
        /// </summary>
        [Test]
        public void HandleNullableGuidWithEmptyValue_Returns_GuidEmpty()
        {
            var guidPropertyBuilder = new GuidPropertyBuilder();
            var emptyGuid = Guid.Empty;

            var result = guidPropertyBuilder.BuildPropertyAsync(typeof(Guid?), emptyGuid.ToString("D")).Result;
            Assert.AreEqual(emptyGuid, result);
        }

        /// <summary>
        /// Pass valid Guid value into nullable Guid property.
        /// -> Guid will be parsed successfully.
        /// </summary>
        [Test]
        public void HandleNullableGuidWithValidValue_Returns_GuidValue()
        {
            var guidPropertyBuilder = new GuidPropertyBuilder();
            var guid = Guid.NewGuid();

            var result = guidPropertyBuilder.BuildPropertyAsync(typeof(Guid?), guid.ToString("D")).Result;
            Assert.AreEqual(guid, result);
        }

        #endregion
    }
}
