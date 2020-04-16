using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;
using TreadmillCrane.KeyValueToObjectConverter.Tests.Enums;

namespace TreadmillCrane.KeyValueToObjectConverter.Tests.PropertyBuilders
{
    [TestFixture]
    public class EnumPropertyBuilderTests
    {
        #region Methods

        /// <summary>
        /// Pass a valid string to non-nullable property.
        /// -> Enum can be parsed successfully.
        /// </summary>
        [Test]
        public void HandleNonNullableEnumWithValidIntegerValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumPropertyBuilder();
            var goodString = $"{(int)StudentTypes.Good}";

            var handledResult = enumModelBinderService.BuildPropertyAsync(typeof(StudentTypes), goodString, CancellationToken.None)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        /// <summary>
        /// Pass a valid string to nullable property.
        /// -> Enum can be parsed successfully.
        /// </summary>
        [Test]
        public void HandleNonNullableEnumWithValidStringValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumPropertyBuilder();
            var goodString = $"{nameof(StudentTypes.Good)}";

            var handledResult = enumModelBinderService.BuildPropertyAsync(typeof(StudentTypes), goodString, CancellationToken.None)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        /// <summary>
        /// Pass out of range integer value
        /// -> Returns default value of enumeration.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task HandleNonNullableEnumWithInvalidIntegerValue_Returns_DefaultValue()
        {
            var enumModelBinderService = new EnumPropertyBuilder();
            var invalidValue = "-1";

            var handledValue = await enumModelBinderService.BuildPropertyAsync(typeof(StudentTypes), invalidValue, CancellationToken.None);
            Assert.AreEqual(default(StudentTypes), (StudentTypes)handledValue);
        }

        /// <summary>
        /// Pass invalid string to non-nullable enum field.
        /// -> Returns default value of enumeration.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task HandleNonNullableEnumWithInvalidStringValue_Returns_DefaultValue()
        {
            var enumModelBinderService = new EnumPropertyBuilder();
            var invalidValue = "Invalid string";

            var handledValue = await enumModelBinderService.BuildPropertyAsync(typeof(StudentTypes), invalidValue, CancellationToken.None);
            Assert.AreEqual(default(StudentTypes), (StudentTypes)handledValue);
        }

        /// <summary>
        /// Pass a valid integer into nullable enum field.
        /// -> Enum can be handled successfully.
        /// </summary>
        [Test]
        public void HandleNullableEnumWithValidIntegerValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumPropertyBuilder();
            var goodString = $"{(int)StudentTypes.Good}";

            var handledResult = enumModelBinderService.BuildPropertyAsync(typeof(StudentTypes?), goodString, CancellationToken.None)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        /// <summary>
        /// Pas a valid string into nullable enum field.
        /// -> Enum can be handled successfully.
        /// </summary>
        [Test]
        public void HandleNullableEnumWithValidStringValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumPropertyBuilder();
            var goodString = $"{nameof(StudentTypes.Good)}";

            var handledResult = enumModelBinderService.BuildPropertyAsync(typeof(StudentTypes?), goodString, CancellationToken.None)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        #endregion
    }
}