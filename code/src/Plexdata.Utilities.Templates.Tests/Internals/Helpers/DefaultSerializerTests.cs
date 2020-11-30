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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Plexdata.Utilities.Formatting.Helpers.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class DefaultSerializerTests
    {
        [Test]
        public void DefaultSerializer_DefaultConstruction_ThrowsNothing()
        {
            Assert.That(() => this.CreateInstance(), Throws.Nothing);
        }

        [Test]
        public void Recursions_DefaultValueCheck_ValueIsZero()
        {
            Assert.That(this.CreateInstance().Recursions, Is.Zero);
        }

        [Test]
        public void Serialize_ArgumentIsNull_ResultIsEmpty()
        {
            Assert.That(this.CreateInstance().Serialize(null, null, null, null), Is.Empty);
        }

        [Test]
        public void Serialize_ProviderIsNull_ResultAsExpected()
        {
            String expected = "[SomeLabel: \"Some Label\"; SomeIndex: 42; SomeChar1: \"\\u0003\"; SomeChar2: \"@\"; SomeObject1: null; SomeObject2: [Statement: Some Statement; Timestamp: 29.10.2020 23:17:05]; SomeObject3: 123456789; SomeTime: 1.10:17:36; SomeDate: 29.10.2020 23:17:05; SomeGuid: 00112233-4455-6677-8899-aabbccddeeff]";

            String actual = this.CreateInstance().Serialize(null, null, null, new TestClass());

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_ProviderIsInvariantCulture_ResultAsExpected()
        {
            String expected = "[SomeLabel: \"Some Label\"; SomeIndex: 42; SomeChar1: \"\\u0003\"; SomeChar2: \"@\"; SomeObject1: null; SomeObject2: [Statement: Some Statement; Timestamp: 29.10.2020 23:17:05]; SomeObject3: 123456789; SomeTime: 1.10:17:36; SomeDate: 10/29/2020 23:17:05; SomeGuid: 00112233-4455-6677-8899-aabbccddeeff]";

            String actual = this.CreateInstance().Serialize(CultureInfo.InvariantCulture, null, null, new TestClass());

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [SetUICulture("de-DE")]
        public void Serialize_ProviderIsCurrentUICulture_ResultAsExpected()
        {
            String expected = "[SomeLabel: \"Some Label\"; SomeIndex: 42; SomeChar1: \"\\u0003\"; SomeChar2: \"@\"; SomeObject1: null; SomeObject2: [Statement: Some Statement; Timestamp: 29.10.2020 23:17:05]; SomeObject3: 123456789; SomeTime: 1.10:17:36; SomeDate: 29.10.2020 23:17:05; SomeGuid: 00112233-4455-6677-8899-aabbccddeeff]";

            String actual = this.CreateInstance().Serialize(CultureInfo.CurrentUICulture, null, null, new TestClass());

            Assert.That(actual, Is.EqualTo(expected));
        }

        private IArgumentSerializer CreateInstance()
        {
            return new DefaultSerializer();
        }

        #region Test Classes

        private class TestClass
        {
            public String SomeLabel { get; } = "Some Label";

            public Int32 SomeIndex { get; } = 42;

            public Char SomeChar1 { get; } = (Char)3;

            public Char SomeChar2 { get; } = '@';

            public Object SomeObject1 { get; } = null;

            public Object SomeObject2 { get; } = new ChildTestClass();

            public Object SomeObject3 { get; } = Int32.Parse("123456789");

            public TimeSpan SomeTime { get; } = System.TimeSpan.FromSeconds(123456);

            public DateTime SomeDate { get; } = System.DateTime.Parse("2020-10-29T23:17:05.123");

            public Guid SomeGuid { get; } = System.Guid.Parse("00112233-4455-6677-8899-AABBCCDDEEFF");

            public ChildTestClass Other { get; } = null;

            public ChildTestClass Child { get; } = new ChildTestClass();

            public List<Int64> Items { get; } = new List<Int64>() { 1000, 2000, 3000, 4000 };

            public Dictionary<String, String> Dictionary { get; } = new Dictionary<String, String>() { { "key1", "val1" }, { "key2", "val2" } };

            protected TimeSpan SomeSpan { get; } = default;
        }

        private class ChildTestClass
        {
            public String Statement { get; } = "Some Statement";

            public DateTime Timestamp { get; } = DateTime.Parse("2020-10-29T23:17:05.333");

            public override String ToString()
            {
                return $"{nameof(this.Statement)}: {this.Statement}; {nameof(this.Timestamp)}: {this.Timestamp}";
            }
        }

        #endregion
    }
}
