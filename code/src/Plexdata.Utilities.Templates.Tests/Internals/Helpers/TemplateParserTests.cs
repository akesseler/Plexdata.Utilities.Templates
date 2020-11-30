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
using Plexdata.Utilities.Formatting.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plexdata.Utilities.Formatting.Helpers.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class TemplateParserTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Parse_FormatIsInvalid_ResultIsEmpty(String format)
        {
            IEnumerable<BaseToken> actual = TemplateParser.Parse(format);

            Assert.That(actual, Is.Empty);
        }

        [TestCase("some text", 1, "some text")]
        [TestCase("some text with {{escaped}} values", 1, "some text with {escaped} values")]
        [TestCase("some text with {invalid formatting", 2, "some text with ", "{invalid formatting")]
        [TestCase("some text with {} empty format", 3, "some text with ", "{}", " empty format")]
        public void Parse_FormatIsTextOnly_ResultAsExpected(String format, Int32 count, params String[] markers)
        {
            List<BaseToken> actual = TemplateParser.Parse(format).ToList();

            Assert.That(actual.Count, Is.EqualTo(count));

            for (Int32 index = 0; index < actual.Count; index++)
            {
                Assert.That(actual[index], Is.InstanceOf<TextToken>());
                Assert.That(actual[index].Marker, Is.EqualTo(markers[index]));
            }
        }

        [TestCase("{0}", 1, "{0}")]
        [TestCase("{0}{0}", 2, "{0}", "{0}")]
        [TestCase("{0}{1}{3}", 3, "{0}", "{1}", "{3}")]
        public void Parse_FormatIsHoleOnly_ResultAsExpected(String format, Int32 count, params String[] markers)
        {
            List<BaseToken> actual = TemplateParser.Parse(format).ToList();

            Assert.That(actual.Count, Is.EqualTo(count));

            for (Int32 index = 0; index < actual.Count; index++)
            {
                Assert.That(actual[index], Is.InstanceOf<HoleToken>());
                Assert.That(actual[index].Marker, Is.EqualTo(markers[index]));
            }
        }

        [TestCase("some {{value}} with more {data}", 2, "some {value} with more ", "{data}")]
        [TestCase("some {value} with {{more}} data", 3, "some ", "{value}", " with {more} data")]
        [TestCase("some {value} with {more} data", 5, "some ", "{value}", " with ", "{more}", " data")]
        public void Parse_FormatIsMixed_ResultAsExpected(String format, Int32 count, params String[] markers)
        {
            List<BaseToken> actual = TemplateParser.Parse(format).ToList();

            Assert.That(actual.Count, Is.EqualTo(count));

            for (Int32 index = 0; index < actual.Count; index++)
            {
                Assert.That(actual[index].Marker, Is.EqualTo(markers[index]));
            }
        }

        [TestCase("some {da{ta}} ", 3, "some ", "{da{ta}", "} ")]
        [TestCase("some value {with,-10,42} multi-alignments", 3, "some value ", "{with,-10,42}", " multi-alignments")]
        [TestCase("some value {with,2-3} misplaced hyphen operator", 3, "some value ", "{with,2-3}", " misplaced hyphen operator")]
        [TestCase("some value {with,X} invalid alignment character", 3, "some value ", "{with,X}", " invalid alignment character")]
        [TestCase("some value {w$ith} misplaced stringify operator", 3, "some value ", "{w$ith}", " misplaced stringify operator")]
        [TestCase("some value {w@ith} misplaced spreading operator", 3, "some value ", "{w@ith}", " misplaced spreading operator")]
        [TestCase("some value {@wi_t h} invalid name character", 3, "some value ", "{@wi_t h}", " invalid name character")]
        public void Parse_FormatIsInvalid_ResultAsExpected(String format, Int32 count, params String[] markers)
        {
            List<BaseToken> actual = TemplateParser.Parse(format).ToList();

            Assert.That(actual.Count, Is.EqualTo(count));

            for (Int32 index = 0; index < actual.Count; index++)
            {
                Assert.That(actual[index], Is.InstanceOf<TextToken>());
                Assert.That(actual[index].Marker, Is.EqualTo(markers[index]));
            }
        }

        [TestCase("{value}", 1, "{value}")]
        [TestCase("{val_ue}", 1, "{val_ue}")]
        [TestCase("{ValUe001}", 1, "{ValUe001}")]
        [TestCase("{value,10}", 1, "{value,10}")]
        [TestCase("{value,-10}", 1, "{value,-10}")]
        [TestCase("{value:XXX}", 1, "{value:XXX}")]
        [TestCase("{value:X x X}", 1, "{value:X x X}")]
        [TestCase("{value:X@x%X}", 1, "{value:X@x%X}")]
        [TestCase("{$value}", 1, "{$value}")]
        [TestCase("{$val_ue}", 1, "{$val_ue}")]
        [TestCase("{$ValUe001}", 1, "{$ValUe001}")]
        [TestCase("{$value,10}", 1, "{$value,10}")]
        [TestCase("{$value,-10}", 1, "{$value,-10}")]
        [TestCase("{$value:XXX}", 1, "{$value:XXX}")]
        [TestCase("{$value:X x X}", 1, "{$value:X x X}")]
        [TestCase("{$value:X@x%X}", 1, "{$value:X@x%X}")]
        [TestCase("{@value}", 1, "{@value}")]
        [TestCase("{@val_ue}", 1, "{@val_ue}")]
        [TestCase("{@ValUe001}", 1, "{@ValUe001}")]
        [TestCase("{@value,10}", 1, "{@value,10}")]
        [TestCase("{@value,-10}", 1, "{@value,-10}")]
        [TestCase("{@value:XXX}", 1, "{@value:XXX}")]
        [TestCase("{@value:X x X}", 1, "{@value:X x X}")]
        [TestCase("{@value:X@x%X}", 1, "{@value:X@x%X}")]
        [TestCase("{1}", 1, "{1}")]
        [TestCase("{001}", 1, "{001}")]
        [TestCase("{0,10}", 1, "{0,10}")]
        [TestCase("{5,-10}", 1, "{5,-10}")]
        [TestCase("{4:XXX}", 1, "{4:XXX}")]
        [TestCase("{3:X x X}", 1, "{3:X x X}")]
        [TestCase("{2:X@x%X}", 1, "{2:X@x%X}")]
        [TestCase("{$2}", 1, "{$2}")]
        [TestCase("{$001}", 1, "{$001}")]
        [TestCase("{$1,10}", 1, "{$1,10}")]
        [TestCase("{$2,-10}", 1, "{$2,-10}")]
        [TestCase("{$4:XXX}", 1, "{$4:XXX}")]
        [TestCase("{$3:X x X}", 1, "{$3:X x X}")]
        [TestCase("{$0:X@x%X}", 1, "{$0:X@x%X}")]
        [TestCase("{@1}", 1, "{@1}")]
        [TestCase("{@001}", 1, "{@001}")]
        [TestCase("{@1,10}", 1, "{@1,10}")]
        [TestCase("{@2,-10}", 1, "{@2,-10}")]
        [TestCase("{@4:XXX}", 1, "{@4:XXX}")]
        [TestCase("{@3:X x X}", 1, "{@3:X x X}")]
        [TestCase("{@0:X@x%X}", 1, "{@0:X@x%X}")]
        public void Parse_FormatIsValid_ResultAsExpected(String format, Int32 count, params String[] markers)
        {
            List<BaseToken> actual = TemplateParser.Parse(format).ToList();

            Assert.That(actual.Count, Is.EqualTo(count));

            for (Int32 index = 0; index < actual.Count; index++)
            {
                Assert.That(actual[index], Is.InstanceOf<HoleToken>());
                Assert.That(actual[index].Marker, Is.EqualTo(markers[index]));
            }
        }

        [TestCase("{7:HH:mm:ss.ffff}", "7", "", "HH:mm:ss.ffff", true, false, false)]
        [TestCase("{7:DD/MM//YYYY}", "7", "", "DD/MM//YYYY", true, false, false)]
        [TestCase("{7:#,###0.0}", "7", "", "#,###0.0", true, false, false)]
        [TestCase("{7,123:HH:mm:ss.ffff}", "7", "123", "HH:mm:ss.ffff", true, false, false)]
        [TestCase("{7,123:DD/MM//YYYY}", "7", "123", "DD/MM//YYYY", true, false, false)]
        [TestCase("{7,123:#,###0.0}", "7", "123", "#,###0.0", true, false, false)]
        [TestCase("{7,-123:HH:mm:ss.ffff}", "7", "-123", "HH:mm:ss.ffff", true, false, false)]
        [TestCase("{7,-123:DD/MM//YYYY}", "7", "-123", "DD/MM//YYYY", true, false, false)]
        [TestCase("{7,-123:#,###0.0}", "7", "-123", "#,###0.0", true, false, false)]
        [TestCase("{$7:HH:mm:ss.ffff}", "$7", "", "HH:mm:ss.ffff", true, true, false)]
        [TestCase("{$7:DD/MM//YYYY}", "$7", "", "DD/MM//YYYY", true, true, false)]
        [TestCase("{$7:#,###0.0}", "$7", "", "#,###0.0", true, true, false)]
        [TestCase("{$7,123:HH:mm:ss.ffff}", "$7", "123", "HH:mm:ss.ffff", true, true, false)]
        [TestCase("{$7,123:DD/MM//YYYY}", "$7", "123", "DD/MM//YYYY", true, true, false)]
        [TestCase("{$7,123:#,###0.0}", "$7", "123", "#,###0.0", true, true, false)]
        [TestCase("{$7,-123:HH:mm:ss.ffff}", "$7", "-123", "HH:mm:ss.ffff", true, true, false)]
        [TestCase("{$7,-123:DD/MM//YYYY}", "$7", "-123", "DD/MM//YYYY", true, true, false)]
        [TestCase("{$7,-123:#,###0.0}", "$7", "-123", "#,###0.0", true, true, false)]
        [TestCase("{@7:HH:mm:ss.ffff}", "@7", "", "HH:mm:ss.ffff", true, false, true)]
        [TestCase("{@7:DD/MM//YYYY}", "@7", "", "DD/MM//YYYY", true, false, true)]
        [TestCase("{@7:#,###0.0}", "@7", "", "#,###0.0", true, false, true)]
        [TestCase("{@7,123:HH:mm:ss.ffff}", "@7", "123", "HH:mm:ss.ffff", true, false, true)]
        [TestCase("{@7,123:DD/MM//YYYY}", "@7", "123", "DD/MM//YYYY", true, false, true)]
        [TestCase("{@7,123:#,###0.0}", "@7", "123", "#,###0.0", true, false, true)]
        [TestCase("{@7,-123:HH:mm:ss.ffff}", "@7", "-123", "HH:mm:ss.ffff", true, false, true)]
        [TestCase("{@7,-123:DD/MM//YYYY}", "@7", "-123", "DD/MM//YYYY", true, false, true)]
        [TestCase("{@7,-123:#,###0.0}", "@7", "-123", "#,###0.0", true, false, true)]
        [TestCase("{name:HH:mm:ss.ffff}", "name", "", "HH:mm:ss.ffff", false, false, false)]
        [TestCase("{name:DD/MM//YYYY}", "name", "", "DD/MM//YYYY", false, false, false)]
        [TestCase("{name:#,###0.0}", "name", "", "#,###0.0", false, false, false)]
        [TestCase("{name,123:HH:mm:ss.ffff}", "name", "123", "HH:mm:ss.ffff", false, false, false)]
        [TestCase("{name,123:DD/MM//YYYY}", "name", "123", "DD/MM//YYYY", false, false, false)]
        [TestCase("{name,123:#,###0.0}", "name", "123", "#,###0.0", false, false, false)]
        [TestCase("{name,-123:HH:mm:ss.ffff}", "name", "-123", "HH:mm:ss.ffff", false, false, false)]
        [TestCase("{name,-123:DD/MM//YYYY}", "name", "-123", "DD/MM//YYYY", false, false, false)]
        [TestCase("{name,-123:#,###0.0}", "name", "-123", "#,###0.0", false, false, false)]
        [TestCase("{$name:HH:mm:ss.ffff}", "$name", "", "HH:mm:ss.ffff", false, true, false)]
        [TestCase("{$name:DD/MM//YYYY}", "$name", "", "DD/MM//YYYY", false, true, false)]
        [TestCase("{$name:#,###0.0}", "$name", "", "#,###0.0", false, true, false)]
        [TestCase("{$name,123:HH:mm:ss.ffff}", "$name", "123", "HH:mm:ss.ffff", false, true, false)]
        [TestCase("{$name,123:DD/MM//YYYY}", "$name", "123", "DD/MM//YYYY", false, true, false)]
        [TestCase("{$name,123:#,###0.0}", "$name", "123", "#,###0.0", false, true, false)]
        [TestCase("{$name,-123:HH:mm:ss.ffff}", "$name", "-123", "HH:mm:ss.ffff", false, true, false)]
        [TestCase("{$name,-123:DD/MM//YYYY}", "$name", "-123", "DD/MM//YYYY", false, true, false)]
        [TestCase("{$name,-123:#,###0.0}", "$name", "-123", "#,###0.0", false, true, false)]
        [TestCase("{@name:HH:mm:ss.ffff}", "@name", "", "HH:mm:ss.ffff", false, false, true)]
        [TestCase("{@name:DD/MM//YYYY}", "@name", "", "DD/MM//YYYY", false, false, true)]
        [TestCase("{@name:#,###0.0}", "@name", "", "#,###0.0", false, false, true)]
        [TestCase("{@name,123:HH:mm:ss.ffff}", "@name", "123", "HH:mm:ss.ffff", false, false, true)]
        [TestCase("{@name,123:DD/MM//YYYY}", "@name", "123", "DD/MM//YYYY", false, false, true)]
        [TestCase("{@name,123:#,###0.0}", "@name", "123", "#,###0.0", false, false, true)]
        [TestCase("{@name,-123:HH:mm:ss.ffff}", "@name", "-123", "HH:mm:ss.ffff", false, false, true)]
        [TestCase("{@name,-123:DD/MM//YYYY}", "@name", "-123", "DD/MM//YYYY", false, false, true)]
        [TestCase("{@name,-123:#,###0.0}", "@name", "-123", "#,###0.0", false, false, true)]
        public void Parse_RealisticFormats_PropertiesAsExpected(String marker, String symbol, String lining, String format, Boolean numbering, Boolean stringify, Boolean spreading)
        {
            List<BaseToken> actual = TemplateParser.Parse(marker).ToList();

            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual[0], Is.InstanceOf<HoleToken>());
            Assert.That(actual[0].Marker, Is.EqualTo(marker));
            Assert.That(actual[0].Symbol, Is.EqualTo(symbol));
            Assert.That(actual[0].Lining, Is.EqualTo(lining));
            Assert.That(actual[0].Format, Is.EqualTo(format));
            Assert.That(actual[0].IsNumbering, Is.EqualTo(numbering));
            Assert.That(actual[0].IsStringify, Is.EqualTo(stringify));
            Assert.That(actual[0].IsSpreading, Is.EqualTo(spreading));
        }

        [TestCase("{42, }")]
        [TestCase("{42,@}")]
        [TestCase("{42,$}")]
        [TestCase("{42,{}")]
        [TestCase("{42,,}")]
        [TestCase("{42,_}")]
        [TestCase("{42,a}")]
        public void Parse_InvalidLining_ResultIsTextToken(String marker)
        {
            BaseToken actual = TemplateParser.Parse(marker).First();

            Assert.That(actual, Is.InstanceOf<TextToken>());
        }

        [TestCase("{42,123,123}")]
        [TestCase("{name,123,123}")]
        [TestCase("{42,-123,123}")]
        [TestCase("{name,-123,123}")]
        [TestCase("{42,123,-123}")]
        [TestCase("{name,123,-123}")]
        [TestCase("{42,-123,-123}")]
        [TestCase("{name,-123,-123}")]
        public void Parse_DoubleLining_ResultIsTextToken(String marker)
        {
            BaseToken actual = TemplateParser.Parse(marker).First();

            Assert.That(actual, Is.InstanceOf<TextToken>());
        }

        [TestCase("{42}")]
        [TestCase("{42,}")]
        [TestCase("{42:}")]
        public void Parse_EmptyLining_ResultIsNotJustified(String marker)
        {
            BaseToken actual = TemplateParser.Parse(marker).First();

            Assert.That(actual, Is.InstanceOf<HoleToken>());
            Assert.That(actual.IsLeftJustified, Is.False);
            Assert.That(actual.IsRightJustified, Is.False);
        }

        [Test]
        public void Parse_LeftLining_ResultIsLeftJustified()
        {
            BaseToken actual = TemplateParser.Parse("{42,-123}").First();

            Assert.That(actual, Is.InstanceOf<HoleToken>());
            Assert.That(actual.IsLeftJustified, Is.True);
            Assert.That(actual.IsRightJustified, Is.False);
        }

        [Test]
        public void Parse_RightLining_ResultIsRightJustified()
        {
            BaseToken actual = TemplateParser.Parse("{42,123}").First();

            Assert.That(actual, Is.InstanceOf<HoleToken>());
            Assert.That(actual.IsLeftJustified, Is.False);
            Assert.That(actual.IsRightJustified, Is.True);
        }

        [TestCase("{42:{}")]
        public void Parse_InvalidFormat_ResultIsTextToken(String marker)
        {
            BaseToken actual = TemplateParser.Parse(marker).First();

            Assert.That(actual, Is.InstanceOf<TextToken>());
        }

        [TestCase("{0}{1}{2}{3}", 0, 1, 2, 3)]
        [TestCase("{0} {1} {2} {3}", 0, -1, 1, -1, 2, -1, 3)]
        [TestCase("{3}{2}{1}{0}", 0, 1, 2, 3)]
        [TestCase("{3} {2} {1} {0}", 0, -1, 1, -1, 2, -1, 3)]
        public void Parse_RatingValidation_ResultAsExpected(String marker, params Int32[] expected)
        {
            List<BaseToken> actual = TemplateParser.Parse(marker).ToList();

            for (Int32 index = 0; index < expected.Length; index++)
            {
                Assert.That(actual[index].Rating, Is.EqualTo(expected[index]));
            }
        }

        [TestCase("{0}{1}{2}{3}", 0, 1, 2, 3)]
        [TestCase("{0} {1} {2} {3}", 0, -1, 1, -1, 2, -1, 3)]
        [TestCase("{3}{2}{1}{0}", 3, 2, 1, 0)]
        [TestCase("{3} {2} {1} {0}", 3, -1, 2, -1, 1, -1, 0)]
        public void Parse_IndexValidation_ResultAsExpected(String marker, params Int32[] expected)
        {
            List<BaseToken> actual = TemplateParser.Parse(marker).ToList();

            for (Int32 index = 0; index < expected.Length; index++)
            {
                Assert.That(actual[index].ToIndex(), Is.EqualTo(expected[index]));
            }
        }
    }
}
