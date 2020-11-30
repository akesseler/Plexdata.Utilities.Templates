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
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Formatting.Defines.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class TokenLiteralsTests
    {
        [Test]
        public void TokenLiterals_OpenedToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.OpenedToken, Is.EqualTo('{'));
        }

        [Test]
        public void TokenLiterals_ClosedToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.ClosedToken, Is.EqualTo('}'));
        }

        [Test]
        public void TokenLiterals_LiningToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.LiningToken, Is.EqualTo(','));
        }

        [Test]
        public void TokenLiterals_FormatToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.FormatToken, Is.EqualTo(':'));
        }

        [Test]
        public void TokenLiterals_StringToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.StringToken, Is.EqualTo('$'));
        }

        [Test]
        public void TokenLiterals_SpreadToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.SpreadToken, Is.EqualTo('@'));
        }

        [Test]
        public void TokenLiterals_HollowToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.HollowToken, Is.EqualTo('_'));
        }

        [Test]
        public void TokenLiterals_HyphenToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.HyphenToken, Is.EqualTo('-'));
        }

        [Test]
        public void TokenLiterals_ExpandToken_ValueAsExpected()
        {
            Assert.That(TokenLiterals.ExpandToken, Is.EqualTo(' '));
        }
    }
}
