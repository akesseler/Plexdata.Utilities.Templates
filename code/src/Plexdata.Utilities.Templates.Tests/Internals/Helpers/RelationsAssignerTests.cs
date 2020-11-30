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
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plexdata.Utilities.Formatting.Helpers.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class RelationsAssignerTests
    {
        [Test]
        public void Default_ValueValidation_ValueNotNull()
        {
            Assert.That(RelationsAssigner.Default, Is.Not.Null);
        }

        [Test]
        public void Assign_TokensNull_ResultAsExpected()
        {
            IEnumerable<BaseToken> tokens = null;
            Object[] arguments = new Object[] { "argument1" };

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.Zero);
        }

        [Test]
        public void Assign_ArgumentsNull_ResultAsExpected()
        {
            IEnumerable<BaseToken> tokens = new List<BaseToken>() { new TextToken(0, new StringBuilder("sometext")) };
            Object[] arguments = null;

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.Zero);
        }

        [Test]
        public void Assign_TokensEmpty_ResultAsExpected()
        {
            IEnumerable<BaseToken> tokens = new List<BaseToken>();
            Object[] arguments = new Object[] { "argument1" };

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.Zero);
        }

        [Test]
        public void Assign_ArgumentsEmpty_ResultAsExpected()
        {
            IEnumerable<BaseToken> tokens = new List<BaseToken>() { new TextToken(0, new StringBuilder("sometext")) };
            Object[] arguments = new Object[0];

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.Zero);
        }

        [Test]
        public void Assign_OnlyTextTokens_ResultAsExpected()
        {
            IEnumerable<BaseToken> tokens = new List<BaseToken>() { new TextToken(0, new StringBuilder("sometext")) };
            Object[] arguments = new Object[] { "someargument", 42, Guid.NewGuid() };

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.Zero);
        }

        [Test]
        public void Assign_OneHoleTokenWithFormatThreeArguments_ResultIsOne()
        {
            IEnumerable<BaseToken> tokens = new List<BaseToken>()
            {
                new HoleToken(0, 1, new StringBuilder("{fmt}"))
            };

            Object[] arguments = new Object[]
            {
                "someargument", 42, Guid.NewGuid()
            };

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual.Count, Is.EqualTo(1));
        }

        [Test]
        public void Assign_ThreeHoleTokensWithFormatOneArgument_ResultIsThree()
        {
            IEnumerable<BaseToken> tokens = new List<BaseToken>()
            {
                new HoleToken(0, 1, new StringBuilder("{fmt}")),
                new HoleToken(0, 2, new StringBuilder("{fmt}")),
                new HoleToken(0, 3, new StringBuilder("{fmt}"))
            };

            Object[] arguments = new Object[]
            {
                "someargument"
            };

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual.Count, Is.EqualTo(3));
        }

        [Test]
        public void Assign_ThreeHoleTokensWithFormatThreeArguments_ResultIsThree()
        {
            IEnumerable<BaseToken> tokens = new List<BaseToken>()
            {
                new HoleToken(0, 1, new StringBuilder("{fmt}")),
                new HoleToken(0, 2, new StringBuilder("{fmt}")),
                new HoleToken(0, 3, new StringBuilder("{fmt}"))
            };

            Object[] arguments = new Object[]
            {
                "someargument", 42, Guid.NewGuid()
            };

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual.Count, Is.EqualTo(3));
        }

        [TestCase("txt1|{0}|txt2", 1)]
        [TestCase("txt1|{fmt1}|txt2", 1)]
        [TestCase("txt1|{0}|txt2|{1}", 2)]
        [TestCase("txt1|{fmt1}|txt2|{fmt2}", 2)]
        [TestCase("txt1|{0}|txt2|{fmt1}", 2)]
        [TestCase("txt1|{fmt1}|txt2|{0}", 2)]
        [TestCase("txt1|{0}|txt2|{0}", 1)]
        [TestCase("txt1|{1}|txt2|{0}", 2)]
        [TestCase("txt1|{fmt1}|txt2|{fmt1}", 2)]
        [TestCase("txt1|{fmt1}|txt2|{fmt2}", 2)]
        public void Assign_WithMixedTokensSixArguments_CountAsExpected(String formats, Int32 expected)
        {
            IEnumerable<BaseToken> tokens = this.CreateTokens(formats);

            Object[] arguments = new Object[] { 111111, 222222, 333333, 444444, 555555, 666666 };

            IArgumentRelations actual = RelationsAssigner.Assign(tokens, arguments);

            Assert.That(actual.Count, Is.EqualTo(expected));
        }

        private IEnumerable<BaseToken> CreateTokens(String formats)
        {
            Int32 rating = 0;
            List<BaseToken> result = new List<BaseToken>();

            foreach (String format in formats.Split('|'))
            {
                if (format.StartsWith("{"))
                {
                    result.Add(new HoleToken(0, rating++, new StringBuilder(format)));
                }
                else
                {
                    result.Add(new TextToken(0, new StringBuilder(format)));
                }
            }

            return result;
        }
    }
}
