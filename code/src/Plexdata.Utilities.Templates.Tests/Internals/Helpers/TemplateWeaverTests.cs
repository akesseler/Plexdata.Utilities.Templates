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
using Plexdata.Utilities.Formatting.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plexdata.Utilities.Formatting.Helpers.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class TemplateWeaverTests
    {
        private Options options;
        private IEnumerable<BaseToken> tokens;
        private StringBuilder output;
        private Object[] arguments;

        [SetUp]
        public void SetUp()
        {
            this.options = new Options();
            this.tokens = new List<BaseToken>() { new TextToken(0, new StringBuilder("text")) };
            this.output = new StringBuilder();
            this.arguments = new Object[0];

        }

        #region Parameters Handling

        [Test]
        public void Weave_OptionsIsNull_ThrowsNothing()
        {
            Assert.That(() => TemplateWeaver.Weave(null, this.tokens, this.output, this.arguments), Throws.Nothing);
        }

        [Test]
        public void Weave_TokensIsNull_ThrowsNothing()
        {
            Assert.That(() => TemplateWeaver.Weave(this.options, null, this.output, this.arguments), Throws.Nothing);
        }

        [Test]
        public void Weave_TokensIsEmpty_ThrowsNothing()
        {
            Assert.That(() => TemplateWeaver.Weave(this.options, new List<BaseToken>(), this.output, this.arguments), Throws.Nothing);
        }

        [Test]
        public void Weave_OutputIsNull_ThrowsNothing()
        {
            Assert.That(() => TemplateWeaver.Weave(this.options, this.tokens, null, this.arguments), Throws.Nothing);
        }

        [Test]
        public void Weave_ArgumentsIsNull_ThrowsNothing()
        {
            Assert.That(() => TemplateWeaver.Weave(this.options, this.tokens, this.output, null), Throws.Nothing);
        }

        #endregion

        #region Weave All By Index

        [TestCase(null, "{2}", "{1}", "{0}", "{2}txtAarg1txtBarg0txtC", "arg0", "arg1")]
        [TestCase(null, "{2}", "{1}", "{0}", "arg2txtAarg1txtBarg0txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{2}", "{1}", "{0}", "arg2txtAarg1txtBarg0txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{2}", "{1}", "{0}", "FLBtxtAarg1txtBarg0txtC", "arg0", "arg1")]
        [TestCase("FLB", "{2}", "{1}", "{0}", "arg2txtAarg1txtBarg0txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{2}", "{1}", "{0}", "arg2txtAarg1txtBarg0txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{2,-5}", "{1,-10}", "{0,-2}", "{2,-5}txtAarg1      txtBarg0txtC", "arg0", "arg1")]
        [TestCase(null, "{2,-5}", "{1,-10}", "{0,-2}", "arg2 txtAarg1      txtBarg0txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{2,-5}", "{1,-10}", "{0,-2}", "arg2 txtAarg1      txtBarg0txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{2,-5}", "{1,-10}", "{0,-2}", "FLBtxtAarg1      txtBarg0txtC", "arg0", "arg1")]
        [TestCase("FLB", "{2,-5}", "{1,-10}", "{0,-2}", "arg2 txtAarg1      txtBarg0txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{2,-5}", "{1,-10}", "{0,-2}", "arg2 txtAarg1      txtBarg0txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{2,5}", "{1,10}", "{0,2}", "{2,5}txtA      arg1txtBarg0txtC", "arg0", "arg1")]
        [TestCase(null, "{2,5}", "{1,10}", "{0,2}", " arg2txtA      arg1txtBarg0txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{2,5}", "{1,10}", "{0,2}", " arg2txtA      arg1txtBarg0txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{2,5}", "{1,10}", "{0,2}", "FLBtxtA      arg1txtBarg0txtC", "arg0", "arg1")]
        [TestCase("FLB", "{2,5}", "{1,10}", "{0,2}", " arg2txtA      arg1txtBarg0txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{2,5}", "{1,10}", "{0,2}", " arg2txtA      arg1txtBarg0txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{2:N2}", "{1:X8}", "{0:P2}", "{2:N2}txtA0003640EtxtB11,111,100.00 %txtC", 111111, 222222)]
        [TestCase(null, "{2:N2}", "{1:X8}", "{0:P2}", "333,333.00txtA0003640EtxtB11,111,100.00 %txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:N2}", "{1:X8}", "{0:P2}", "333,333.00txtA0003640EtxtB11,111,100.00 %txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:N2}", "{1:X8}", "{0:P2}", "FLBtxtA0003640EtxtB11,111,100.00 %txtC", 111111, 222222)]
        [TestCase("FLB", "{2:N2}", "{1:X8}", "{0:P2}", "333,333.00txtA0003640EtxtB11,111,100.00 %txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:N2}", "{1:X8}", "{0:P2}", "333,333.00txtA0003640EtxtB11,111,100.00 %txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-12:N2}", "{1,-10:X8}", "{0,-20:P2}", "{2,-12:N2}txtA0003640E  txtB11,111,100.00 %     txtC", 111111, 222222)]
        [TestCase(null, "{2,-12:N2}", "{1,-10:X8}", "{0,-20:P2}", "333,333.00  txtA0003640E  txtB11,111,100.00 %     txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-12:N2}", "{1,-10:X8}", "{0,-20:P2}", "333,333.00  txtA0003640E  txtB11,111,100.00 %     txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-12:N2}", "{1,-10:X8}", "{0,-20:P2}", "FLBtxtA0003640E  txtB11,111,100.00 %     txtC", 111111, 222222)]
        [TestCase("FLB", "{2,-12:N2}", "{1,-10:X8}", "{0,-20:P2}", "333,333.00  txtA0003640E  txtB11,111,100.00 %     txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-12:N2}", "{1,-10:X8}", "{0,-20:P2}", "333,333.00  txtA0003640E  txtB11,111,100.00 %     txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,12:N2}", "{1,10:X8}", "{0,20:P2}", "{2,12:N2}txtA  0003640EtxtB     11,111,100.00 %txtC", 111111, 222222)]
        [TestCase(null, "{2,12:N2}", "{1,10:X8}", "{0,20:P2}", "  333,333.00txtA  0003640EtxtB     11,111,100.00 %txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,12:N2}", "{1,10:X8}", "{0,20:P2}", "  333,333.00txtA  0003640EtxtB     11,111,100.00 %txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,12:N2}", "{1,10:X8}", "{0,20:P2}", "FLBtxtA  0003640EtxtB     11,111,100.00 %txtC", 111111, 222222)]
        [TestCase("FLB", "{2,12:N2}", "{1,10:X8}", "{0,20:P2}", "  333,333.00txtA  0003640EtxtB     11,111,100.00 %txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,12:N2}", "{1,10:X8}", "{0,20:P2}", "  333,333.00txtA  0003640EtxtB     11,111,100.00 %txtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByIndexFallbackArgumentsLiningFormatAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, -1, new StringBuilder(fmt1)),
                new TextToken(0,     new StringBuilder("txtA")),
                new HoleToken(0, -1, new StringBuilder(fmt2)),
                new TextToken(0,     new StringBuilder("txtB")),
                new HoleToken(0, -1, new StringBuilder(fmt3)),
                new TextToken(0,     new StringBuilder("txtC")),
            };

            this.arguments = args;

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "{2:XYZ}txtA222222txtB111111txtC", 111111, 222222)]
        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333txtA222222txtB111111txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333txtA222222txtB111111txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "FLBtxtA222222txtB111111txtC", 111111, 222222)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333txtA222222txtB111111txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333txtA222222txtB111111txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "{2,-15:XYZ}txtA222222              txtB111111                   txtC", 111111, 222222)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333         txtA222222              txtB111111                   txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333         txtA222222              txtB111111                   txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "FLBtxtA222222              txtB111111                   txtC", 111111, 222222)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333         txtA222222              txtB111111                   txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333         txtA222222              txtB111111                   txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "{2,15:XYZ}txtA              222222txtB                   111111txtC", 111111, 222222)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "         333333txtA              222222txtB                   111111txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "         333333txtA              222222txtB                   111111txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "FLBtxtA              222222txtB                   111111txtC", 111111, 222222)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "         333333txtA              222222txtB                   111111txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "         333333txtA              222222txtB                   111111txtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByIndexWithTestClassUnformattableAndFallbackAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, -1, new StringBuilder(fmt1)),
                new TextToken(0,     new StringBuilder("txtA")),
                new HoleToken(0, -1, new StringBuilder(fmt2)),
                new TextToken(0,     new StringBuilder("txtB")),
                new HoleToken(0, -1, new StringBuilder(fmt3)),
                new TextToken(0,     new StringBuilder("txtC")),
            };

            List<Object> helper = new List<Object>();
            foreach (Object arg in args) { helper.Add(new TestClassUnformattable(arg)); }
            this.arguments = helper.ToArray();

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "{2:XYZ}txtA222222-XYZtxtB111111-XYZtxtC", 111111, 222222)]
        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333-XYZtxtA222222-XYZtxtB111111-XYZtxtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333-XYZtxtA222222-XYZtxtB111111-XYZtxtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "FLBtxtA222222-XYZtxtB111111-XYZtxtC", 111111, 222222)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333-XYZtxtA222222-XYZtxtB111111-XYZtxtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "333333-XYZtxtA222222-XYZtxtB111111-XYZtxtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "{2,-15:XYZ}txtA222222-XYZ          txtB111111-XYZ               txtC", 111111, 222222)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333-XYZ     txtA222222-XYZ          txtB111111-XYZ               txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333-XYZ     txtA222222-XYZ          txtB111111-XYZ               txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "FLBtxtA222222-XYZ          txtB111111-XYZ               txtC", 111111, 222222)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333-XYZ     txtA222222-XYZ          txtB111111-XYZ               txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "333333-XYZ     txtA222222-XYZ          txtB111111-XYZ               txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "{2,15:XYZ}txtA          222222-XYZtxtB               111111-XYZtxtC", 111111, 222222)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     333333-XYZtxtA          222222-XYZtxtB               111111-XYZtxtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     333333-XYZtxtA          222222-XYZtxtB               111111-XYZtxtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "FLBtxtA          222222-XYZtxtB               111111-XYZtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     333333-XYZtxtA          222222-XYZtxtB               111111-XYZtxtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     333333-XYZtxtA          222222-XYZtxtB               111111-XYZtxtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByIndexWithTestClassFormattableAndFallbackAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, -1, new StringBuilder(fmt1)),
                new TextToken(0,     new StringBuilder("txtA")),
                new HoleToken(0, -1, new StringBuilder(fmt2)),
                new TextToken(0,     new StringBuilder("txtB")),
                new HoleToken(0, -1, new StringBuilder(fmt3)),
                new TextToken(0,     new StringBuilder("txtC")),
            };

            List<Object> helper = new List<Object>();
            foreach (Object arg in args) { helper.Add(new TestClassFormattable(arg)); }
            this.arguments = helper.ToArray();

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "{2:XYZ}txtAXYZ-222222txtBXYZ-111111txtC", 111111, 222222)]
        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "XYZ-333333txtAXYZ-222222txtBXYZ-111111txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "XYZ-333333txtAXYZ-222222txtBXYZ-111111txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "FLBtxtAXYZ-222222txtBXYZ-111111txtC", 111111, 222222)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "XYZ-333333txtAXYZ-222222txtBXYZ-111111txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:XYZ}", "{1:XYZ}", "{0:XYZ}", "XYZ-333333txtAXYZ-222222txtBXYZ-111111txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "{2,-15:XYZ}txtAXYZ-222222          txtBXYZ-111111               txtC", 111111, 222222)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-333333     txtAXYZ-222222          txtBXYZ-111111               txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-333333     txtAXYZ-222222          txtBXYZ-111111               txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "FLBtxtAXYZ-222222          txtBXYZ-111111               txtC", 111111, 222222)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-333333     txtAXYZ-222222          txtBXYZ-111111               txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-15:XYZ}", "{1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-333333     txtAXYZ-222222          txtBXYZ-111111               txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "{2,15:XYZ}txtA          XYZ-222222txtB               XYZ-111111txtC", 111111, 222222)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     XYZ-333333txtA          XYZ-222222txtB               XYZ-111111txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     XYZ-333333txtA          XYZ-222222txtB               XYZ-111111txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "FLBtxtA          XYZ-222222txtB               XYZ-111111txtC", 111111, 222222)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     XYZ-333333txtA          XYZ-222222txtB               XYZ-111111txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,15:XYZ}", "{1,20:XYZ}", "{0,25:XYZ}", "     XYZ-333333txtA          XYZ-222222txtB               XYZ-111111txtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByIndexWithTestClassCustomFormatterAndFallbackAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;
            this.options.Provider = new TestClassCustomFormatter();

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, -1, new StringBuilder(fmt1)),
                new TextToken(0,     new StringBuilder("txtA")),
                new HoleToken(0, -1, new StringBuilder(fmt2)),
                new TextToken(0,     new StringBuilder("txtB")),
                new HoleToken(0, -1, new StringBuilder(fmt3)),
                new TextToken(0,     new StringBuilder("txtC")),
            };

            this.arguments = args;

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        #endregion

        #region Weave All By Label

        [TestCase(null, "{2}", "{lbl1}", "{0}", "arg0txtAarg1txtB{0}txtC", "arg0", "arg1")]
        [TestCase(null, "{2}", "{lbl1}", "{0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{2}", "{lbl1}", "{0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{2}", "{lbl1}", "{0}", "arg0txtAarg1txtBFLBtxtC", "arg0", "arg1")]
        [TestCase("FLB", "{2}", "{lbl1}", "{0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{2}", "{lbl1}", "{0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{2,-5}", "{lbl1,-10}", "{0,-2}", "arg0 txtAarg1      txtB{0,-2}txtC", "arg0", "arg1")]
        [TestCase(null, "{2,-5}", "{lbl1,-10}", "{0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{2,-5}", "{lbl1,-10}", "{0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{2,-5}", "{lbl1,-10}", "{0,-2}", "arg0 txtAarg1      txtBFLBtxtC", "arg0", "arg1")]
        [TestCase("FLB", "{2,-5}", "{lbl1,-10}", "{0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{2,-5}", "{lbl1,-10}", "{0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{2,5}", "{lbl1,10}", "{0,2}", " arg0txtA      arg1txtB{0,2}txtC", "arg0", "arg1")]
        [TestCase(null, "{2,5}", "{lbl1,10}", "{0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{2,5}", "{lbl1,10}", "{0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{2,5}", "{lbl1,10}", "{0,2}", " arg0txtA      arg1txtBFLBtxtC", "arg0", "arg1")]
        [TestCase("FLB", "{2,5}", "{lbl1,10}", "{0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{2,5}", "{lbl1,10}", "{0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{2:N2}", "{lbl1:X8}", "{0:P2}", "111,111.00txtA0003640EtxtB{0:P2}txtC", 111111, 222222)]
        [TestCase(null, "{2:N2}", "{lbl1:X8}", "{0:P2}", "111,111.00txtA0003640EtxtB33,333,300.00 %txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:N2}", "{lbl1:X8}", "{0:P2}", "111,111.00txtA0003640EtxtB33,333,300.00 %txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:N2}", "{lbl1:X8}", "{0:P2}", "111,111.00txtA0003640EtxtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2:N2}", "{lbl1:X8}", "{0:P2}", "111,111.00txtA0003640EtxtB33,333,300.00 %txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:N2}", "{lbl1:X8}", "{0:P2}", "111,111.00txtA0003640EtxtB33,333,300.00 %txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-12:N2}", "{lbl1,-10:X8}", "{0,-20:P2}", "111,111.00  txtA0003640E  txtB{0,-20:P2}txtC", 111111, 222222)]
        [TestCase(null, "{2,-12:N2}", "{lbl1,-10:X8}", "{0,-20:P2}", "111,111.00  txtA0003640E  txtB33,333,300.00 %     txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-12:N2}", "{lbl1,-10:X8}", "{0,-20:P2}", "111,111.00  txtA0003640E  txtB33,333,300.00 %     txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-12:N2}", "{lbl1,-10:X8}", "{0,-20:P2}", "111,111.00  txtA0003640E  txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,-12:N2}", "{lbl1,-10:X8}", "{0,-20:P2}", "111,111.00  txtA0003640E  txtB33,333,300.00 %     txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-12:N2}", "{lbl1,-10:X8}", "{0,-20:P2}", "111,111.00  txtA0003640E  txtB33,333,300.00 %     txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,12:N2}", "{lbl1,10:X8}", "{0,20:P2}", "  111,111.00txtA  0003640EtxtB{0,20:P2}txtC", 111111, 222222)]
        [TestCase(null, "{2,12:N2}", "{lbl1,10:X8}", "{0,20:P2}", "  111,111.00txtA  0003640EtxtB     33,333,300.00 %txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,12:N2}", "{lbl1,10:X8}", "{0,20:P2}", "  111,111.00txtA  0003640EtxtB     33,333,300.00 %txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,12:N2}", "{lbl1,10:X8}", "{0,20:P2}", "  111,111.00txtA  0003640EtxtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,12:N2}", "{lbl1,10:X8}", "{0,20:P2}", "  111,111.00txtA  0003640EtxtB     33,333,300.00 %txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,12:N2}", "{lbl1,10:X8}", "{0,20:P2}", "  111,111.00txtA  0003640EtxtB     33,333,300.00 %txtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByLabelFallbackArgumentsLiningFormatAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, 0, new StringBuilder(fmt1)),
                new TextToken(0,    new StringBuilder("txtA")),
                new HoleToken(0, 1, new StringBuilder(fmt2)),
                new TextToken(0,    new StringBuilder("txtB")),
                new HoleToken(0, 2, new StringBuilder(fmt3)),
                new TextToken(0,    new StringBuilder("txtC")),
            };

            this.arguments = args;

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        [TestCase(null, "{$lbl2}", "{$lbl1}", "{$lbl0}", "arg0txtAarg1txtB{$lbl0}txtC", "arg0", "arg1")]
        [TestCase(null, "{$lbl2}", "{$lbl1}", "{$lbl0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{$lbl2}", "{$lbl1}", "{$lbl0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{$lbl2}", "{$lbl1}", "{$lbl0}", "arg0txtAarg1txtBFLBtxtC", "arg0", "arg1")]
        [TestCase("FLB", "{$lbl2}", "{$lbl1}", "{$lbl0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{$lbl2}", "{$lbl1}", "{$lbl0}", "arg0txtAarg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{$lbl2,-5}", "{$lbl1,-10}", "{$lbl0,-2}", "arg0 txtAarg1      txtB{$lbl0,-2}txtC", "arg0", "arg1")]
        [TestCase(null, "{$lbl2,-5}", "{$lbl1,-10}", "{$lbl0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{$lbl2,-5}", "{$lbl1,-10}", "{$lbl0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{$lbl2,-5}", "{$lbl1,-10}", "{$lbl0,-2}", "arg0 txtAarg1      txtBFLBtxtC", "arg0", "arg1")]
        [TestCase("FLB", "{$lbl2,-5}", "{$lbl1,-10}", "{$lbl0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{$lbl2,-5}", "{$lbl1,-10}", "{$lbl0,-2}", "arg0 txtAarg1      txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{$lbl2,5}", "{$lbl1,10}", "{$lbl0,2}", " arg0txtA      arg1txtB{$lbl0,2}txtC", "arg0", "arg1")]
        [TestCase(null, "{$lbl2,5}", "{$lbl1,10}", "{$lbl0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase(null, "{$lbl2,5}", "{$lbl1,10}", "{$lbl0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase("FLB", "{$lbl2,5}", "{$lbl1,10}", "{$lbl0,2}", " arg0txtA      arg1txtBFLBtxtC", "arg0", "arg1")]
        [TestCase("FLB", "{$lbl2,5}", "{$lbl1,10}", "{$lbl0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2")]
        [TestCase("FLB", "{$lbl2,5}", "{$lbl1,10}", "{$lbl0,2}", " arg0txtA      arg1txtBarg2txtC", "arg0", "arg1", "arg2", "arg3")]
        [TestCase(null, "{$lbl2:N2}", "{$lbl1:X8}", "{$lbl0:P2}", "111111txtA222222txtB{$lbl0:P2}txtC", 111111, 222222)]
        [TestCase(null, "{$lbl2:N2}", "{$lbl1:X8}", "{$lbl0:P2}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333)]
        [TestCase(null, "{$lbl2:N2}", "{$lbl1:X8}", "{$lbl0:P2}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{$lbl2:N2}", "{$lbl1:X8}", "{$lbl0:P2}", "111111txtA222222txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{$lbl2:N2}", "{$lbl1:X8}", "{$lbl0:P2}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{$lbl2:N2}", "{$lbl1:X8}", "{$lbl0:P2}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{$lbl2,-12:N2}", "{$lbl1,-10:X8}", "{$lbl0,-20:P2}", "111111      txtA222222    txtB{$lbl0,-20:P2}txtC", 111111, 222222)]
        [TestCase(null, "{$lbl2,-12:N2}", "{$lbl1,-10:X8}", "{$lbl0,-20:P2}", "111111      txtA222222    txtB333333              txtC", 111111, 222222, 333333)]
        [TestCase(null, "{$lbl2,-12:N2}", "{$lbl1,-10:X8}", "{$lbl0,-20:P2}", "111111      txtA222222    txtB333333              txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{$lbl2,-12:N2}", "{$lbl1,-10:X8}", "{$lbl0,-20:P2}", "111111      txtA222222    txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{$lbl2,-12:N2}", "{$lbl1,-10:X8}", "{$lbl0,-20:P2}", "111111      txtA222222    txtB333333              txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{$lbl2,-12:N2}", "{$lbl1,-10:X8}", "{$lbl0,-20:P2}", "111111      txtA222222    txtB333333              txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{$lbl2,12:N2}", "{$lbl1,10:X8}", "{$lbl0,20:P2}", "      111111txtA    222222txtB{$lbl0,20:P2}txtC", 111111, 222222)]
        [TestCase(null, "{$lbl2,12:N2}", "{$lbl1,10:X8}", "{$lbl0,20:P2}", "      111111txtA    222222txtB              333333txtC", 111111, 222222, 333333)]
        [TestCase(null, "{$lbl2,12:N2}", "{$lbl1,10:X8}", "{$lbl0,20:P2}", "      111111txtA    222222txtB              333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{$lbl2,12:N2}", "{$lbl1,10:X8}", "{$lbl0,20:P2}", "      111111txtA    222222txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{$lbl2,12:N2}", "{$lbl1,10:X8}", "{$lbl0,20:P2}", "      111111txtA    222222txtB              333333txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{$lbl2,12:N2}", "{$lbl1,10:X8}", "{$lbl0,20:P2}", "      111111txtA    222222txtB              333333txtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByLabelFallbackArgumentsLiningFormatAsGivenToStringifyValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, 0, new StringBuilder(fmt1)),
                new TextToken(0,    new StringBuilder("txtA")),
                new HoleToken(0, 1, new StringBuilder(fmt2)),
                new TextToken(0,    new StringBuilder("txtB")),
                new HoleToken(0, 2, new StringBuilder(fmt3)),
                new TextToken(0,    new StringBuilder("txtC")),
            };

            this.arguments = args;

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111txtA222222txtB{0:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111txtA222222txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111txtA222222txtB333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111         txtA222222              txtB{0,-25:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111         txtA222222              txtB333333                   txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111         txtA222222              txtB333333                   txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111         txtA222222              txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111         txtA222222              txtB333333                   txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111         txtA222222              txtB333333                   txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "         111111txtA              222222txtB{0,25:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "         111111txtA              222222txtB                   333333txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "         111111txtA              222222txtB                   333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "         111111txtA              222222txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "         111111txtA              222222txtB                   333333txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "         111111txtA              222222txtB                   333333txtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByLabelWithTestClassUnformattableAndFallbackAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, 0, new StringBuilder(fmt1)),
                new TextToken(0,    new StringBuilder("txtA")),
                new HoleToken(0, 1, new StringBuilder(fmt2)),
                new TextToken(0,    new StringBuilder("txtB")),
                new HoleToken(0, 2, new StringBuilder(fmt3)),
                new TextToken(0,    new StringBuilder("txtC")),
            };

            List<Object> helper = new List<Object>();
            foreach (Object arg in args) { helper.Add(new TestClassUnformattable(arg)); }
            this.arguments = helper.ToArray();

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111-XYZtxtA222222-XYZtxtB{0:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111-XYZtxtA222222-XYZtxtB333333-XYZtxtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111-XYZtxtA222222-XYZtxtB333333-XYZtxtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111-XYZtxtA222222-XYZtxtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111-XYZtxtA222222-XYZtxtB333333-XYZtxtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "111111-XYZtxtA222222-XYZtxtB333333-XYZtxtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111-XYZ     txtA222222-XYZ          txtB{0,-25:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111-XYZ     txtA222222-XYZ          txtB333333-XYZ               txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111-XYZ     txtA222222-XYZ          txtB333333-XYZ               txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111-XYZ     txtA222222-XYZ          txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111-XYZ     txtA222222-XYZ          txtB333333-XYZ               txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "111111-XYZ     txtA222222-XYZ          txtB333333-XYZ               txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     111111-XYZtxtA          222222-XYZtxtB{0,25:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     111111-XYZtxtA          222222-XYZtxtB               333333-XYZtxtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     111111-XYZtxtA          222222-XYZtxtB               333333-XYZtxtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     111111-XYZtxtA          222222-XYZtxtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     111111-XYZtxtA          222222-XYZtxtB               333333-XYZtxtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     111111-XYZtxtA          222222-XYZtxtB               333333-XYZtxtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByLabelWithTestClassFormattableAndFallbackAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, 0, new StringBuilder(fmt1)),
                new TextToken(0,    new StringBuilder("txtA")),
                new HoleToken(0, 1, new StringBuilder(fmt2)),
                new TextToken(0,    new StringBuilder("txtB")),
                new HoleToken(0, 2, new StringBuilder(fmt3)),
                new TextToken(0,    new StringBuilder("txtC")),
            };

            List<Object> helper = new List<Object>();
            foreach (Object arg in args) { helper.Add(new TestClassFormattable(arg)); }
            this.arguments = helper.ToArray();

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "XYZ-111111txtAXYZ-222222txtB{0:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "XYZ-111111txtAXYZ-222222txtBXYZ-333333txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "XYZ-111111txtAXYZ-222222txtBXYZ-333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "XYZ-111111txtAXYZ-222222txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "XYZ-111111txtAXYZ-222222txtBXYZ-333333txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2:XYZ}", "{lbl1:XYZ}", "{0:XYZ}", "XYZ-111111txtAXYZ-222222txtBXYZ-333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-111111     txtAXYZ-222222          txtB{0,-25:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-111111     txtAXYZ-222222          txtBXYZ-333333               txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-111111     txtAXYZ-222222          txtBXYZ-333333               txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-111111     txtAXYZ-222222          txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-111111     txtAXYZ-222222          txtBXYZ-333333               txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,-15:XYZ}", "{lbl1,-20:XYZ}", "{0,-25:XYZ}", "XYZ-111111     txtAXYZ-222222          txtBXYZ-333333               txtC", 111111, 222222, 333333, 444444)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     XYZ-111111txtA          XYZ-222222txtB{0,25:XYZ}txtC", 111111, 222222)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     XYZ-111111txtA          XYZ-222222txtB               XYZ-333333txtC", 111111, 222222, 333333)]
        [TestCase(null, "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     XYZ-111111txtA          XYZ-222222txtB               XYZ-333333txtC", 111111, 222222, 333333, 444444)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     XYZ-111111txtA          XYZ-222222txtBFLBtxtC", 111111, 222222)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     XYZ-111111txtA          XYZ-222222txtB               XYZ-333333txtC", 111111, 222222, 333333)]
        [TestCase("FLB", "{2,15:XYZ}", "{lbl1,20:XYZ}", "{0,25:XYZ}", "     XYZ-111111txtA          XYZ-222222txtB               XYZ-333333txtC", 111111, 222222, 333333, 444444)]
        public void Weave_AllByLabelWithTestClassCustomFormatterAndFallbackAsGivenToFormattedValue_ResultAsExpected(String flb, String fmt1, String fmt2, String fmt3, String expected, params Object[] args)
        {
            this.options.Fallback = flb;
            this.options.Provider = new TestClassCustomFormatter();

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, 0, new StringBuilder(fmt1)),
                new TextToken(0,    new StringBuilder("txtA")),
                new HoleToken(0, 1, new StringBuilder(fmt2)),
                new TextToken(0,    new StringBuilder("txtB")),
                new HoleToken(0, 2, new StringBuilder(fmt3)),
                new TextToken(0,    new StringBuilder("txtC")),
            };

            this.arguments = args;

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo(expected));
        }

        #endregion

        #region Exception Handling

        [Test]
        public void Weave_GetFormattedValueHandlesException_ResultAsExpected()
        {
            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, 0, new StringBuilder("{label}")),
            };

            this.arguments = new Object[] { new TestClassWithException("Value") };

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(this.output.ToString(), Is.EqualTo("[{label} => FormatException: \"Value => TEST FORMAT EXCEPTION\"]"));
        }

        #endregion

        #region Custom Serializer

        [Test]
        public void Weave_AllByIndexWithMixedTypesAndCustomSerializerToSpreadingValue_ResultAsExpected()
        {
            TestClassArgumentSerializer serializer = new TestClassArgumentSerializer();

            this.options.Serializer = serializer;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, -1, new StringBuilder("{@1}")),
                new TextToken(0,     new StringBuilder("txtA")),
                new HoleToken(0, -1, new StringBuilder("{@0}")),
                new TextToken(0,     new StringBuilder("txtB")),
                new HoleToken(0, -1, new StringBuilder("{@3}")),
                new TextToken(0,     new StringBuilder("txtC")),
                new HoleToken(0, -1, new StringBuilder("{@2}")),
                new TextToken(0,     new StringBuilder("txtD")),
            };

            List<Object> helper = new List<Object>();
            helper.Add(111111);
            helper.Add(new TestClassUnformattable(222222));
            helper.Add(333333);
            helper.Add(new TestClassUnformattable(444444));
            this.arguments = helper.ToArray();

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(serializer.CallCount, Is.EqualTo(2));
            Assert.That(this.output.ToString(), Is.EqualTo("txtA111111txtBtxtC333333txtD"));
        }

        [Test]
        public void Weave_AllByLabelWithMixedTypesAndCustomSerializerToSpreadingValue_ResultAsExpected()
        {
            TestClassArgumentSerializer serializer = new TestClassArgumentSerializer();

            this.options.Serializer = serializer;

            this.tokens = new List<BaseToken>()
            {
                new HoleToken(0, 0, new StringBuilder("{@fmt1}")),
                new TextToken(0,    new StringBuilder("txtA")),
                new HoleToken(0, 1, new StringBuilder("{@fmt2}")),
                new TextToken(0,    new StringBuilder("txtB")),
                new HoleToken(0, 2, new StringBuilder("{@fmt3}")),
                new TextToken(0,    new StringBuilder("txtC")),
                new HoleToken(0, 3, new StringBuilder("{@fmt4}")),
                new TextToken(0,    new StringBuilder("txtD")),
            };

            List<Object> helper = new List<Object>();
            helper.Add(111111);
            helper.Add(new TestClassUnformattable(222222));
            helper.Add(333333);
            helper.Add(new TestClassUnformattable(444444));
            this.arguments = helper.ToArray();

            TemplateWeaver.Weave(this.options, this.tokens, this.output, this.arguments);

            Assert.That(serializer.CallCount, Is.EqualTo(2));
            Assert.That(this.output.ToString(), Is.EqualTo("111111txtAtxtB333333txtCtxtD"));
        }

        #endregion

        #region Test Classes

        private class TestClassUnformattable
        {
            private readonly Object value;

            public TestClassUnformattable(Object value)
            {
                this.value = value;
            }

            public override String ToString()
            {
                return this.value.ToString();
            }
        }

        private class TestClassFormattable : IFormattable
        {
            private readonly Object value;

            public TestClassFormattable(Object value)
            {
                this.value = value;
            }

            public override String ToString()
            {
                return this.value.ToString();
            }

            public String ToString(String format, IFormatProvider formatProvider)
            {
                return $"{this.value}-{format}";
            }
        }

        private class TestClassCustomFormatter : IFormatProvider, ICustomFormatter
        {
            public Object GetFormat(Type formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                {
                    return this;
                }

                return null;
            }

            public String Format(String format, Object arg, IFormatProvider formatProvider)
            {
                return $"{format}-{arg}";
            }
        }

        private class TestClassWithException
        {
            private readonly Object value;

            public TestClassWithException(Object value)
            {
                this.value = value;
            }

            public override String ToString()
            {
                throw new FormatException($"{this.value} => TEST FORMAT EXCEPTION");
            }
        }

        private class TestClassArgumentSerializer : IArgumentSerializer
        {
            public Int32 CallCount { get; private set; } = 0;

            public Int32 Recursions => throw new NotImplementedException();

            public String Serialize(IFormatProvider provider, String format, String lining, Object argument)
            {
                this.CallCount += 1;
                return String.Empty;
            }
        }

        #endregion
    }
}
