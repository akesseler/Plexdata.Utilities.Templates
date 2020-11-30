/*
 * MIT License
 * 
 * Copyright (c) 2020 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using NUnit.Framework;
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Plexdata.Utilities.Formatting.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class OptionsTests
    {
        [Test]
        public void Options_DefaultConstruction_PropertiesAsExpected()
        {
            Options actual = new Options();

            Assert.That(actual.Provider, Is.EqualTo(CultureInfo.InvariantCulture));
            Assert.That(actual.Serializer, Is.Null);
            Assert.That(actual.IsLimited, Is.False);
            Assert.That(actual.Maximum, Is.EqualTo(Int32.MaxValue));
            Assert.That(actual.IsFallback, Is.False);
            Assert.That(actual.Fallback, Is.Null);
        }

        [Test]
        public void Default_PropertyValidation_PropertiesAsExpected()
        {
            Options actual = Options.Default;

            Assert.That(actual.Provider, Is.EqualTo(CultureInfo.InvariantCulture));
            Assert.That(actual.Serializer, Is.Null);
            Assert.That(actual.IsLimited, Is.False);
            Assert.That(actual.Maximum, Is.EqualTo(Int32.MaxValue));
            Assert.That(actual.IsFallback, Is.False);
            Assert.That(actual.Fallback, Is.Null);
        }

        [Test]
        public void Create_ProviderIsNullSerializerIsNull_PropertiesAsExpected()
        {
            Options actual = Options.Create(null, null);

            Assert.That(actual.Provider, Is.Null);
            Assert.That(actual.Serializer, Is.Null);
            Assert.That(actual.IsLimited, Is.False);
            Assert.That(actual.Maximum, Is.EqualTo(Int32.MaxValue));
            Assert.That(actual.IsFallback, Is.False);
            Assert.That(actual.Fallback, Is.Null);
        }

        [Test]
        public void Create_ProviderIsNotNullAndSerializerIsNull_PropertiesAsExpected()
        {
            TestFormatProvider provider = new TestFormatProvider();
            TestArgumentSerializer serializer = null;

            Options actual = Options.Create(provider, serializer);

            Assert.That(actual.Provider, Is.SameAs(provider));
            Assert.That(actual.Serializer, Is.Null);
            Assert.That(actual.IsLimited, Is.False);
            Assert.That(actual.Maximum, Is.EqualTo(Int32.MaxValue));
            Assert.That(actual.IsFallback, Is.False);
            Assert.That(actual.Fallback, Is.Null);
        }

        [Test]
        public void Create_ProviderIsNullAndSerializerIsNotNull_PropertiesAsExpected()
        {
            TestFormatProvider provider = null;
            TestArgumentSerializer serializer = new TestArgumentSerializer();

            Options actual = Options.Create(provider, serializer);

            Assert.That(actual.Provider, Is.Null);
            Assert.That(actual.Serializer, Is.SameAs(serializer));
            Assert.That(actual.IsLimited, Is.False);
            Assert.That(actual.Maximum, Is.EqualTo(Int32.MaxValue));
            Assert.That(actual.IsFallback, Is.False);
            Assert.That(actual.Fallback, Is.Null);
        }

        public void Create_ProviderIsNotNullAndSerializerIsNotNull_PropertiesAsExpected()
        {
            TestFormatProvider provider = new TestFormatProvider();
            TestArgumentSerializer serializer = new TestArgumentSerializer();

            Options actual = Options.Create(provider, serializer);

            Assert.That(actual.Provider, Is.SameAs(provider));
            Assert.That(actual.Serializer, Is.SameAs(serializer));
            Assert.That(actual.IsLimited, Is.False);
            Assert.That(actual.Maximum, Is.EqualTo(Int32.MaxValue));
            Assert.That(actual.IsFallback, Is.False);
            Assert.That(actual.Fallback, Is.Null);
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        [TestCase(Int32.MaxValue, false)]
        [TestCase(Int32.MaxValue - 1, true)]
        public void IsLimited_VariousMaximumValues_ResultAsExpected(Int32 maximum, Boolean expected)
        {
            Options actual = new Options() { Maximum = maximum };

            Assert.That(actual.IsLimited, Is.EqualTo(expected));
        }

        [TestCase(null, false)]
        [TestCase("", true)]
        [TestCase("  ", true)]
        [TestCase("fallback", true)]
        public void IsFallback_VariousMaximumValues_ResultAsExpected(String fallback, Boolean expected)
        {
            Options actual = new Options() { Fallback = fallback };

            Assert.That(actual.IsFallback, Is.EqualTo(expected));
        }

        #region Test Classes

        private class TestFormatProvider : IFormatProvider
        {
            public Object GetFormat(Type formatType)
            {
                throw new NotImplementedException();
            }
        }

        private class TestArgumentSerializer : IArgumentSerializer
        {
            public Int32 Recursions => throw new NotImplementedException();

            public String Serialize(IFormatProvider provider, String format, String lining, Object argument)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
