using NUnit.Framework;
using TreadmillCrane.KeyValueToObjectConverter.Services.PropertyBuilders;

namespace TreadmillCrane.KeyValueToObjectConverter.Tests.PropertyBuilders.DefaultPropertyBuilderTests
{
    [TestFixture]
    public class StringPropertyBuilderTests
    {
        #region Methods

        /// <summary>
        /// Pass empty string into string field.
        /// -> Returns string empty.
        /// </summary>
        [Test]
        public void PassEmptyStringIntoStringField_Returns_StringEmpty()
        {
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(string), string.Empty)
                .Result;

            Assert.AreEqual(string.Empty, handledResult);
        }

        /// <summary>
        /// Pass a valid into string field.
        /// -> Returns passed string.
        /// </summary>
        [Test]
        public void PassStringIntoStringField_Returns_PassedString()
        {
            var text = "Hello world";
            var defaultPropertyBuilder = new DefaultPropertyBuilder();
            var handledResult = defaultPropertyBuilder.BuildPropertyAsync(typeof(string), text)
                .Result;

            Assert.AreEqual(text, handledResult);
        }

        #endregion
    }
}