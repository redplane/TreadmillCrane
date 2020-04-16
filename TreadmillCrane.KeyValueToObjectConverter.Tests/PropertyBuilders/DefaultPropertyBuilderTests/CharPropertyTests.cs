using NUnit.Framework;
using TreadmillCrane.KeyValueToObjectConverter.Models.Exceptions;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;

namespace TreadmillCrane.KeyValueToObjectConverter.Tests.PropertyBuilders.DefaultPropertyBuilderTests
{
    [TestFixture]
    public class CharPropertyTests
    {
        #region Methods

        /// <summary>
        /// Pass a string into a non-nullable char property.
        /// -> throws IgnorePropertyBuildException
        /// </summary>
        [Test]
        public void PassStringIntoNonNullableCharProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(char), "Hello world"));
        }

        /// <summary>
        /// Pass a string into a nullable char property.
        /// -> throws IgnorePropertyBuildException
        /// </summary>
        [Test]
        public void PassStringIntoNullableCharProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(char?), "Hello world"));
        }

        /// <summary>
        /// Pass a one character string into a non-nullable char property.
        /// -> Return the first only character of initial string.
        /// </summary>
        [Test]
        public void PassOneCharacterStringIntoNonNullableCharProperty_Returns_ThePassedCharacter()
        {
            var initialCharacter = 'H';

            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(char), $"{initialCharacter}").Result;
            Assert.AreEqual(initialCharacter, handledResult);
        }

        /// <summary>
        /// Pass a one character string into a nullable char property.
        /// -> Return the first only character of initial string.
        /// </summary>
        [Test]
        public void PassOneCharacterStringIntoNullableCharProperty_Returns_ThePassedCharacter()
        {
            var initialCharacter = 'H';

            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(char?), $"{initialCharacter}").Result;
            Assert.AreEqual(initialCharacter, handledResult);
        }

        /// <summary>
        /// Pass an empty string into a non-nullable char property.
        /// -> throws IgnorePropertyBuildException
        /// </summary>
        [Test]
        public void PassEmptyStringIntoNonNullableCharProperty_Throws_IgnorePropertyBuildException()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            Assert.CatchAsync<IgnorePropertyBuildException>(async () =>
                await defaultPropertyBuilder.BuildPropertyAsync(typeof(char), string.Empty));
        }

        /// <summary>
        /// Pass an empty string into a nullable char property.
        /// -> Return the first only character of initial string.
        /// </summary>
        [Test]
        public void PassEmptyStringIntoNullableCharProperty_Returns_Null()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(char?), string.Empty).Result;
            Assert.IsNull(handledResult);
        }

        /// <summary>
        /// Pass a number into non-nullable character property.
        /// -> throws IgnorePropertyBuildException
        /// </summary>
        [Test]
        public void PassNumberIntoNonNullableCharProperty_Returns_UnicodeControlCode()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(char), 5).Result;
            Assert.AreEqual('\u0005', handledResult);
        }

        /// <summary>
        /// Pass a number into nullable character property.
        /// -> throws IgnorePropertyBuildException
        /// </summary>
        [Test]
        public void PassNumberIntoNullableCharProperty_Returns_UnicodeControlCode()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(char?), 5).Result;
            Assert.AreEqual('\u0005', handledResult);
        }
        #endregion
    }
}