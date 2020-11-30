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
    internal class BaseTokenTests
    {
        [Test]
        public void BaseToken_Construction_ParseWasCalledOnce()
        {
            TestToken actual = this.CreateInstance() as TestToken;

            Assert.That(actual.ParseCallCount, Is.EqualTo(1));
        }

        [Test]
        public void BaseToken_Initialization_InitializedAsExpected()
        {
            BaseToken actual = this.CreateInstance();

            Assert.That(actual.Offset, Is.EqualTo(-1));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.Empty);
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
            Assert.That(actual.IsNumbering, Is.False);
            Assert.That(actual.IsStringify, Is.False);
            Assert.That(actual.IsSpreading, Is.False);
        }

        [Test]
        public void Properties_Preparation_PropertiesAsExpected()
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.Initialize(42, 23, "Marker", "Symbol", "Lining", "Format");

            Assert.That(actual.Offset, Is.EqualTo(42));
            Assert.That(actual.Rating, Is.EqualTo(23));
            Assert.That(actual.Marker, Is.EqualTo("Marker"));
            Assert.That(actual.Symbol, Is.EqualTo("Symbol"));
            Assert.That(actual.Lining, Is.EqualTo("Lining"));
            Assert.That(actual.Format, Is.EqualTo("Format"));
            Assert.That(actual.IsNumbering, Is.False);
            Assert.That(actual.IsStringify, Is.False);
            Assert.That(actual.IsSpreading, Is.False);
        }

        [TestCase(-10, -1)]
        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        [TestCase(29, 29)]
        public void Offset_ValueEvaluation_PropertyChangedAsExpected(Int32 value, Int32 expected)
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.SetOffset(value);

            Assert.That(actual.Offset, Is.EqualTo(expected));
        }

        [TestCase(-10, -1)]
        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        [TestCase(29, 29)]
        public void Rating_ValueEvaluation_PropertyChangedAsExpected(Int32 value, Int32 expected)
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.SetRating(value);

            Assert.That(actual.Rating, Is.EqualTo(expected));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase("marker", "marker")]
        public void Marker_ValueEvaluation_PropertyChangedAsExpected(String value, String expected)
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.SetMarker(value);

            Assert.That(actual.Marker, Is.EqualTo(expected));
        }

        [TestCase(null, "", false, false, false)]
        [TestCase("", "", false, false, false)]
        [TestCase("  ", "  ", false, false, false)]
        [TestCase("123", "123", true, false, false)]
        [TestCase("symbol", "symbol", false, false, false)]
        [TestCase("$symbol", "$symbol", false, true, false)]
        [TestCase("@symbol", "@symbol", false, false, true)]
        public void Symbol_ValueEvaluation_PropertyChangedAsExpected(String value, String expected, Boolean numbering, Boolean stringify, Boolean spreading)
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.SetSymbol(value);

            Assert.That(actual.Symbol, Is.EqualTo(expected));
            Assert.That(actual.IsNumbering, Is.EqualTo(numbering));
            Assert.That(actual.IsStringify, Is.EqualTo(stringify));
            Assert.That(actual.IsSpreading, Is.EqualTo(spreading));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase("lining", "lining")]
        public void Lining_ValueEvaluation_PropertyChangedAsExpected(String value, String expected)
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.SetLining(value);

            Assert.That(actual.Lining, Is.EqualTo(expected));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase("format", "format")]
        public void Format_ValueEvaluation_PropertyChangedAsExpected(String value, String expected)
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.SetFormat(value);

            Assert.That(actual.Format, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_Preparation_ResultAsExpected()
        {
            TestToken actual = this.CreateInstance() as TestToken;

            actual.Initialize(42, 23, "Marker", "Symbol", "Lining", "Format");

            Assert.That(actual.ToString(), Is.EqualTo("Type: TestToken, Offset: [42], Rating: [23], Symbol: [Symbol], Lining: [Lining], Format: [Format], Marker: [Marker]"));
        }

        private BaseToken CreateInstance()
        {
            return new TestToken(42, null);
        }

        private class TestToken : BaseToken
        {
            public Int32 ParseCallCount = 0;

            public TestToken(Int32 offset, StringBuilder marker)
                : base(offset, marker)
            {
            }

            public void Initialize(Int32 offset, Int32 rating, String marker, String symbol, String lining, String format)
            {
                this.SetOffset(offset);
                this.SetRating(rating);
                this.SetMarker(marker);
                this.SetSymbol(symbol);
                this.SetLining(lining);
                this.SetFormat(format);
            }

            public void SetOffset(Int32 offset) { base.Offset = offset; }

            public void SetRating(Int32 rating) { base.Rating = rating; }

            public void SetMarker(String marker) { base.Marker = marker; }

            public void SetSymbol(String symbol) { base.Symbol = symbol; }

            public void SetLining(String lining) { base.Lining = lining; }

            public void SetFormat(String format) { base.Format = format; }

            public override Int32 ToIndex() { throw new NotImplementedException(); }

            public override String ToLabel() { throw new NotImplementedException(); }

            protected override void Parse(Int32 offset, Int32 rating, StringBuilder marker) { this.ParseCallCount++; }
        }
    }
}
