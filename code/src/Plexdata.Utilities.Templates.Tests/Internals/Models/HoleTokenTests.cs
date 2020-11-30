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
    internal class HoleTokenTests
    {
        [Test]
        public void HoleToken_OffsetInvalid_PropertiesAsExpected()
        {
            HoleToken actual = this.CreateInstance(-42, 29, new StringBuilder("some value"));

            Assert.That(actual.Offset, Is.EqualTo(-1));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.Empty);
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [Test]
        public void HoleToken_RatingInvalid_PropertiesAsExpected()
        {
            HoleToken actual = this.CreateInstance(42, -29, new StringBuilder("some value"));

            Assert.That(actual.Offset, Is.EqualTo(42));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.EqualTo("some value"));
            Assert.That(actual.Symbol, Is.EqualTo("some value"));
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [Test]
        public void HoleToken_MarkerIsNull_PropertiesAsExpected()
        {
            HoleToken actual = this.CreateInstance(42, 29, null);

            Assert.That(actual.Offset, Is.EqualTo(-1));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.Empty);
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [Test]
        public void HoleToken_MarkerIsEmpty_PropertiesAsExpected()
        {
            HoleToken actual = this.CreateInstance(42, 29, new StringBuilder());

            Assert.That(actual.Offset, Is.EqualTo(-1));
            Assert.That(actual.Rating, Is.EqualTo(-1));
            Assert.That(actual.Marker, Is.Empty);
            Assert.That(actual.Symbol, Is.Empty);
            Assert.That(actual.Lining, Is.Empty);
            Assert.That(actual.Format, Is.Empty);
        }

        [TestCase("{}", "", "", "")]
        [TestCase("4711", "4711", "", "")]
        [TestCase("{4711}", "4711", "", "")]
        [TestCase("@4711", "@4711", "", "")]
        [TestCase("{@4711}", "@4711", "", "")]
        [TestCase("$4711", "$4711", "", "")]
        [TestCase("{$4711}", "$4711", "", "")]
        [TestCase("SomeSymbol", "SomeSymbol", "", "")]
        [TestCase("{SomeSymbol}", "SomeSymbol", "", "")]
        [TestCase("@SomeSymbol", "@SomeSymbol", "", "")]
        [TestCase("{@SomeSymbol}", "@SomeSymbol", "", "")]
        [TestCase("$SomeSymbol", "$SomeSymbol", "", "")]
        [TestCase("{$SomeSymbol}", "$SomeSymbol", "", "")]
        [TestCase("4711", "4711", "", "")]
        [TestCase("{4711}", "4711", "", "")]
        [TestCase("@4711", "@4711", "", "")]
        [TestCase("{@4711}", "@4711", "", "")]
        [TestCase("$4711", "$4711", "", "")]
        [TestCase("{$4711}", "$4711", "", "")]
        [TestCase(",SomeLining", "", "SomeLining", "")]
        [TestCase("{,SomeLining}", "", "SomeLining", "")]
        [TestCase("SomeSymbol,SomeLining", "SomeSymbol", "SomeLining", "")]
        [TestCase("{SomeSymbol,SomeLining}", "SomeSymbol", "SomeLining", "")]
        [TestCase("@SomeSymbol,SomeLining", "@SomeSymbol", "SomeLining", "")]
        [TestCase("{@SomeSymbol,SomeLining}", "@SomeSymbol", "SomeLining", "")]
        [TestCase("$SomeSymbol,SomeLining", "$SomeSymbol", "SomeLining", "")]
        [TestCase("{$SomeSymbol,SomeLining}", "$SomeSymbol", "SomeLining", "")]
        [TestCase("4711,SomeLining", "4711", "SomeLining", "")]
        [TestCase("{4711,SomeLining}", "4711", "SomeLining", "")]
        [TestCase("@4711,SomeLining", "@4711", "SomeLining", "")]
        [TestCase("{@4711,SomeLining}", "@4711", "SomeLining", "")]
        [TestCase("$4711,SomeLining", "$4711", "SomeLining", "")]
        [TestCase("{$4711,SomeLining}", "$4711", "SomeLining", "")]
        [TestCase(":SomeFormat", "", "", "SomeFormat")]
        [TestCase("{:SomeFormat}", "", "", "SomeFormat")]
        [TestCase("SomeSymbol:SomeFormat", "SomeSymbol", "", "SomeFormat")]
        [TestCase("{SomeSymbol:SomeFormat}", "SomeSymbol", "", "SomeFormat")]
        [TestCase("@SomeSymbol:SomeFormat", "@SomeSymbol", "", "SomeFormat")]
        [TestCase("{@SomeSymbol:SomeFormat}", "@SomeSymbol", "", "SomeFormat")]
        [TestCase("$SomeSymbol:SomeFormat", "$SomeSymbol", "", "SomeFormat")]
        [TestCase("{$SomeSymbol:SomeFormat}", "$SomeSymbol", "", "SomeFormat")]
        [TestCase("4711:SomeFormat", "4711", "", "SomeFormat")]
        [TestCase("{4711:SomeFormat}", "4711", "", "SomeFormat")]
        [TestCase("@4711:SomeFormat", "@4711", "", "SomeFormat")]
        [TestCase("{@4711:SomeFormat}", "@4711", "", "SomeFormat")]
        [TestCase("$4711:SomeFormat", "$4711", "", "SomeFormat")]
        [TestCase("{$4711:SomeFormat}", "$4711", "", "SomeFormat")]
        [TestCase(",SomeLining:SomeFormat", "", "SomeLining", "SomeFormat")]
        [TestCase("{,SomeLining:SomeFormat}", "", "SomeLining", "SomeFormat")]
        [TestCase("SomeSymbol,SomeLining:SomeFormat", "SomeSymbol", "SomeLining", "SomeFormat")]
        [TestCase("{SomeSymbol,SomeLining:SomeFormat}", "SomeSymbol", "SomeLining", "SomeFormat")]
        [TestCase("@SomeSymbol,SomeLining:SomeFormat", "@SomeSymbol", "SomeLining", "SomeFormat")]
        [TestCase("{@SomeSymbol,SomeLining:SomeFormat}", "@SomeSymbol", "SomeLining", "SomeFormat")]
        [TestCase("$SomeSymbol,SomeLining:SomeFormat", "$SomeSymbol", "SomeLining", "SomeFormat")]
        [TestCase("{$SomeSymbol,SomeLining:SomeFormat}", "$SomeSymbol", "SomeLining", "SomeFormat")]
        [TestCase("4711,SomeLining:SomeFormat", "4711", "SomeLining", "SomeFormat")]
        [TestCase("{4711,SomeLining:SomeFormat}", "4711", "SomeLining", "SomeFormat")]
        [TestCase("@4711,SomeLining:SomeFormat", "@4711", "SomeLining", "SomeFormat")]
        [TestCase("{@4711,SomeLining:SomeFormat}", "@4711", "SomeLining", "SomeFormat")]
        [TestCase("$4711,SomeLining:SomeFormat", "$4711", "SomeLining", "SomeFormat")]
        [TestCase("{$4711,SomeLining:SomeFormat}", "$4711", "SomeLining", "SomeFormat")]
        public void HoleToken_ParametersValid_PropertiesAsExpected(String marker, String symbol, String lining, String format)
        {
            HoleToken actual = this.CreateInstance(42, 29, new StringBuilder(marker));

            Assert.That(actual.Offset, Is.EqualTo(42));
            Assert.That(actual.Rating, Is.EqualTo(29));
            Assert.That(actual.Marker, Is.EqualTo(marker));
            Assert.That(actual.Symbol, Is.EqualTo(symbol));
            Assert.That(actual.Lining, Is.EqualTo(lining));
            Assert.That(actual.Format, Is.EqualTo(format));
        }

        [TestCase("7:HH:mm:ss.ffff", "7", "", "HH:mm:ss.ffff")]
        [TestCase("{7:HH:mm:ss.ffff}", "7", "", "HH:mm:ss.ffff")]
        [TestCase("7:DD/MM//YYYY", "7", "", "DD/MM//YYYY")]
        [TestCase("{7:DD/MM//YYYY}", "7", "", "DD/MM//YYYY")]
        [TestCase("7:#,###0.0", "7", "", "#,###0.0")]
        [TestCase("{7:#,###0.0}", "7", "", "#,###0.0")]
        [TestCase("7,123:HH:mm:ss.ffff", "7", "123", "HH:mm:ss.ffff")]
        [TestCase("{7,123:HH:mm:ss.ffff}", "7", "123", "HH:mm:ss.ffff")]
        [TestCase("7,123:DD/MM//YYYY", "7", "123", "DD/MM//YYYY")]
        [TestCase("{7,123:DD/MM//YYYY}", "7", "123", "DD/MM//YYYY")]
        [TestCase("7,123:#,###0.0", "7", "123", "#,###0.0")]
        [TestCase("{7,123:#,###0.0}", "7", "123", "#,###0.0")]
        [TestCase("name:HH:mm:ss.ffff", "name", "", "HH:mm:ss.ffff")]
        [TestCase("{name:HH:mm:ss.ffff}", "name", "", "HH:mm:ss.ffff")]
        [TestCase("name:DD/MM//YYYY", "name", "", "DD/MM//YYYY")]
        [TestCase("{name:DD/MM//YYYY}", "name", "", "DD/MM//YYYY")]
        [TestCase("name:#,###0.0", "name", "", "#,###0.0")]
        [TestCase("{name:#,###0.0}", "name", "", "#,###0.0")]
        [TestCase("name,123:HH:mm:ss.ffff", "name", "123", "HH:mm:ss.ffff")]
        [TestCase("{name,123:HH:mm:ss.ffff}", "name", "123", "HH:mm:ss.ffff")]
        [TestCase("name,123:DD/MM//YYYY", "name", "123", "DD/MM//YYYY")]
        [TestCase("{name,123:DD/MM//YYYY}", "name", "123", "DD/MM//YYYY")]
        [TestCase("name,123:#,###0.0", "name", "123", "#,###0.0")]
        [TestCase("{name,123:#,###0.0}", "name", "123", "#,###0.0")]
        public void HoleToken_RealisticFormats_PropertiesAsExpected(String marker, String symbol, String lining, String format)
        {
            HoleToken actual = this.CreateInstance(42, 29, new StringBuilder(marker));

            Assert.That(actual.Offset, Is.EqualTo(42));
            Assert.That(actual.Rating, Is.EqualTo(29));
            Assert.That(actual.Marker, Is.EqualTo(marker));
            Assert.That(actual.Symbol, Is.EqualTo(symbol));
            Assert.That(actual.Lining, Is.EqualTo(lining));
            Assert.That(actual.Format, Is.EqualTo(format));
        }

        [TestCase("{7::,$@_- }", "7", "", ":,$@_- ")]
        [TestCase("{7:,:$@_- }", "7", "", ",:$@_- ")]
        [TestCase("{7,123::,$@_- }", "7", "123", ":,$@_- ")]
        [TestCase("{7,123:,:$@_- }", "7", "123", ",:$@_- ")]
        [TestCase("{na{m}e,1{2}3:{,:$@_- }}", "name", "123", ",:$@_- ")]
        public void HoleToken_TokenCheck_PropertiesAsExpected(String marker, String symbol, String lining, String format)
        {
            HoleToken actual = this.CreateInstance(42, 29, new StringBuilder(marker));

            Assert.That(actual.Offset, Is.EqualTo(42));
            Assert.That(actual.Rating, Is.EqualTo(29));
            Assert.That(actual.Marker, Is.EqualTo(marker));
            Assert.That(actual.Symbol, Is.EqualTo(symbol));
            Assert.That(actual.Lining, Is.EqualTo(lining));
            Assert.That(actual.Format, Is.EqualTo(format));
        }

        [TestCase("{}", false)]
        [TestCase("@", false)]
        [TestCase("{@}", false)]
        [TestCase("$", false)]
        [TestCase("{$}", false)]
        [TestCase("4711", true)]
        [TestCase("{4711}", true)]
        [TestCase("@4711", true)]
        [TestCase("{@4711}", true)]
        [TestCase("$4711", true)]
        [TestCase("{$4711}", true)]
        [TestCase("SomeSymbol", false)]
        [TestCase("{SomeSymbol}", false)]
        [TestCase("@SomeSymbol", false)]
        [TestCase("{@SomeSymbol}", false)]
        [TestCase("$SomeSymbol", false)]
        [TestCase("{$SomeSymbol}", false)]
        public void IsNumbering_ParametersValid_ResultAsExpected(String marker, Boolean expected)
        {
            HoleToken actual = this.CreateInstance(new StringBuilder(marker));

            Assert.That(actual.IsNumbering, Is.EqualTo(expected));
        }

        [TestCase("{}", false)]
        [TestCase("@", false)]
        [TestCase("{@}", false)]
        [TestCase("$", true)]
        [TestCase("{$}", true)]
        [TestCase("4711", false)]
        [TestCase("{4711}", false)]
        [TestCase("@4711", false)]
        [TestCase("{@4711}", false)]
        [TestCase("$4711", true)]
        [TestCase("{$4711}", true)]
        [TestCase("SomeSymbol", false)]
        [TestCase("{SomeSymbol}", false)]
        [TestCase("@SomeSymbol", false)]
        [TestCase("{@SomeSymbol}", false)]
        [TestCase("$SomeSymbol", true)]
        [TestCase("{$SomeSymbol}", true)]
        public void IsStringify_ParametersValid_ResultAsExpected(String source, Boolean expected)
        {
            StringBuilder marker = new StringBuilder(source);

            HoleToken actual = this.CreateInstance(marker);

            Assert.That(actual.IsStringify, Is.EqualTo(expected));
        }

        [TestCase("{}", false)]
        [TestCase("@", true)]
        [TestCase("{@}", true)]
        [TestCase("$", false)]
        [TestCase("{$}", false)]
        [TestCase("4711", false)]
        [TestCase("{4711}", false)]
        [TestCase("@4711", true)]
        [TestCase("{@4711}", true)]
        [TestCase("$4711", false)]
        [TestCase("{$4711}", false)]
        [TestCase("SomeSymbol", false)]
        [TestCase("{SomeSymbol}", false)]
        [TestCase("@SomeSymbol", true)]
        [TestCase("{@SomeSymbol}", true)]
        [TestCase("$SomeSymbol", false)]
        [TestCase("{$SomeSymbol}", false)]
        public void IsSpreading_ParametersValid_ResultAsExpected(String source, Boolean expected)
        {
            StringBuilder marker = new StringBuilder(source);

            HoleToken actual = this.CreateInstance(marker);

            Assert.That(actual.IsSpreading, Is.EqualTo(expected));
        }

        [TestCase("4711", 4711)]
        [TestCase("{4711}", 4711)]
        [TestCase("@4711", 4711)]
        [TestCase("{@4711}", 4711)]
        [TestCase("$4711", 4711)]
        [TestCase("{$4711}", 4711)]
        [TestCase("SomeValue", -1)]
        [TestCase("{SomeValue}", -1)]
        [TestCase("@SomeValue", -1)]
        [TestCase("{@SomeValue}", -1)]
        [TestCase("$SomeValue", -1)]
        [TestCase("{$SomeValue}", -1)]
        [TestCase("Some4711", -1)]
        [TestCase("{Some4711}", -1)]
        [TestCase("@Some4711", -1)]
        [TestCase("{@Some4711}", -1)]
        [TestCase("$Some4711", -1)]
        [TestCase("{$Some4711}", -1)]
        [TestCase("4711Some", -1)]
        [TestCase("{4711Some}", -1)]
        [TestCase("@4711Some", -1)]
        [TestCase("{@4711Some}", -1)]
        [TestCase("$4711Some", -1)]
        [TestCase("{$4711Some}", -1)]
        public void ToIndex_ParametersValidWithoutRating_ResultAsExpected(String source, Int32 expected)
        {
            StringBuilder marker = new StringBuilder(source);

            HoleToken actual = this.CreateInstance(marker);

            Assert.That(actual.ToIndex(), Is.EqualTo(expected));
        }

        [TestCase("4711", 29, 4711)]
        [TestCase("{4711}", 29, 4711)]
        [TestCase("@4711", 29, 4711)]
        [TestCase("{@4711}", 29, 4711)]
        [TestCase("$4711", 29, 4711)]
        [TestCase("{$4711}", 29, 4711)]
        [TestCase("SomeValue", 29, 29)]
        [TestCase("{SomeValue}", 29, 29)]
        [TestCase("@SomeValue", 29, 29)]
        [TestCase("{@SomeValue}", 29, 29)]
        [TestCase("$SomeValue", 29, 29)]
        [TestCase("{$SomeValue}", 29, 29)]
        [TestCase("Some4711", 29, 29)]
        [TestCase("{Some4711}", 29, 29)]
        [TestCase("@Some4711", 29, 29)]
        [TestCase("{@Some4711}", 29, 29)]
        [TestCase("$Some4711", 29, 29)]
        [TestCase("{$Some4711}", 29, 29)]
        [TestCase("4711Some", 29, 29)]
        [TestCase("{4711Some}", 29, 29)]
        [TestCase("@4711Some", 29, 29)]
        [TestCase("{@4711Some}", 29, 29)]
        [TestCase("$4711Some", 29, 29)]
        [TestCase("{$4711Some}", 29, 29)]
        public void ToIndex_ParametersValidWithRating_ResultAsExpected(String source, Int32 rating, Int32 expected)
        {
            HoleToken actual = this.CreateInstance(42, rating, new StringBuilder(source));

            Assert.That(actual.ToIndex(), Is.EqualTo(expected));
        }

        [TestCase("", "")]
        [TestCase("{}", "")]
        [TestCase("@", "")]
        [TestCase("{@}", "")]
        [TestCase("$", "")]
        [TestCase("{$}", "")]
        [TestCase("4711", "")]
        [TestCase("{4711}", "")]
        [TestCase("@4711", "")]
        [TestCase("{@4711}", "")]
        [TestCase("$4711", "")]
        [TestCase("{$4711}", "")]
        [TestCase("SomeValue", "SomeValue")]
        [TestCase("{SomeValue}", "SomeValue")]
        [TestCase("@SomeValue", "SomeValue")]
        [TestCase("{@SomeValue}", "SomeValue")]
        [TestCase("$SomeValue", "SomeValue")]
        [TestCase("{$SomeValue}", "SomeValue")]
        [TestCase("Some4711", "Some4711")]
        [TestCase("{Some4711}", "Some4711")]
        [TestCase("@Some4711", "Some4711")]
        [TestCase("{@Some4711}", "Some4711")]
        [TestCase("$Some4711", "Some4711")]
        [TestCase("{$Some4711}", "Some4711")]
        [TestCase("4711Some", "4711Some")]
        [TestCase("{4711Some}", "4711Some")]
        [TestCase("@4711Some", "4711Some")]
        [TestCase("{@4711Some}", "4711Some")]
        [TestCase("$4711Some", "4711Some")]
        [TestCase("{$4711Some}", "4711Some")]
        public void ToLabel_ParametersValid_ResultAsExpected(String source, String expected)
        {
            StringBuilder marker = new StringBuilder(source);

            HoleToken actual = this.CreateInstance(marker);

            Assert.That(actual.ToLabel(), Is.EqualTo(expected));
        }

        private HoleToken CreateInstance(StringBuilder marker)
        {
            return this.CreateInstance(0, -1, marker);
        }

        private HoleToken CreateInstance(Int32 offset, Int32 rating, StringBuilder marker)
        {
            return new HoleToken(offset, rating, marker);
        }
    }
}
