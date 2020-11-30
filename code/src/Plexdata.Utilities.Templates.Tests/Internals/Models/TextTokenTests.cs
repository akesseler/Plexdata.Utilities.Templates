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
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plexdata.Utilities.Formatting.Entities.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class TextTokenTests
    {
        [Test]
        public void TextToken_OffsetInvalid_PropertiesAsExpected()
        {
            TextToken actual = this.CreateInstance(-42, new StringBuilder("some value"));

            Assert.That(actual.Offset, Is.EqualTo(-1));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.Empty);
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [Test]
        public void TextToken_MarkerIsNull_PropertiesAsExpected()
        {
            TextToken actual = this.CreateInstance(42, null);

            Assert.That(actual.Offset, Is.EqualTo(-1));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.Empty);
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [Test]
        public void TextToken_MarkerIsEmpty_PropertiesAsExpected()
        {
            TextToken actual = this.CreateInstance(42, new StringBuilder());

            Assert.That(actual.Offset, Is.EqualTo(-1));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.Empty);
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [Test]
        public void TextToken_ParametersValid_PropertiesAsExpected()
        {
            TextToken actual = this.CreateInstance(42, new StringBuilder("some value"));

            Assert.That(actual.Offset, Is.EqualTo(42));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.EqualTo("some value"));
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [TestCase("4711", -1)]
        [TestCase("some value", -1)]
        [TestCase("some 4711", -1)]
        public void ToIndex_ParametersValid_ResultAsExpected(String source, Int32 expected)
        {
            TextToken actual = this.CreateInstance(42, new StringBuilder(source));

            Assert.That(actual.ToIndex(), Is.EqualTo(expected));
        }

        [TestCase("4711", "4711")]
        [TestCase("some value", "some value")]
        [TestCase("some 4711", "some 4711")]
        public void ToLabel_ParametersValid_ResultAsExpected(String source, String expected)
        {
            TextToken actual = this.CreateInstance(42, new StringBuilder(source));
            
            Assert.That(actual.ToLabel(), Is.EqualTo(expected));
        }

        private TextToken CreateInstance(Int32 offset, StringBuilder marker)
        {
            return new TextToken(offset, marker);
        }
    }
}
