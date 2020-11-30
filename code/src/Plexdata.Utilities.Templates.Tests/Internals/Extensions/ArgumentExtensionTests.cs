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
using Plexdata.Utilities.Formatting.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Formatting.Tests.Extensions
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class ArgumentExtensionTests
    {
        #region ExpandArguments

        [Test]
        public void ExpandArguments_ParameterIsEmpty_RessultLengthIsZero()
        {
            Assert.That(this.ExpandArgumentsHelper().Length, Is.EqualTo(0));
        }

        [Test]
        public void ExpandArguments_ParameterByCountButNull_RessultLengthAsExpected([Values(1, 2, 3)] Int32 count)
        {
            switch (count)
            {
                case 1:
                    Assert.That(this.ExpandArgumentsHelper(null).Length, Is.EqualTo(1));
                    break;
                case 2:
                    Assert.That(this.ExpandArgumentsHelper(null, null).Length, Is.EqualTo(2));
                    break;
                case 3:
                    Assert.That(this.ExpandArgumentsHelper(null, null, null).Length, Is.EqualTo(3));
                    break;
                default:
                    Assert.That(false);
                    break;
            }
        }

        [Test]
        public void ExpandArguments_ParameterIsStringArray_RessultLengthAsExpected()
        {
            Assert.That(this.ExpandArgumentsHelper(new[] { "s1", "s2" }).Length, Is.EqualTo(2));
        }

        [Test]
        public void ExpandArguments_ParameterIsIntegerArray_RessultLengthAsExpected()
        {
            Assert.That(this.ExpandArgumentsHelper(new[] { 1, 2 }).Length, Is.EqualTo(2));
        }

        [Test]
        public void ExpandArguments_ParameterIsCustomStructArray_RessultLengthAsExpected()
        {
            Assert.That(this.ExpandArgumentsHelper(new[] { new CustomStruct(), new CustomStruct() }).Length, Is.EqualTo(2));
        }

        [Test]
        public void ExpandArguments_ParameterIsCustomClassArray_RessultLengthAsExpected()
        {
            Assert.That(this.ExpandArgumentsHelper(new[] { new CustomClass(), new CustomClass() }).Length, Is.EqualTo(2));
        }

        [TestCaseSource(nameof(MixedContentTestData))]
        public void ExpandArguments_FromTestCaseSource_RessultLengthAsExpected(Object value)
        {
            TestCaseHelper current = (TestCaseHelper)value;

            Assert.That(current.Arguments.ExpandArguments().Length, Is.EqualTo(current.Expected));
        }

        #endregion

        #region IsSystemType

        [Test]
        public void IsSystemType_ArgumentIsNull_ResultIsFalse()
        {
            Object argument = null;

            Assert.That(argument.IsSystemType(), Is.False);
        }

        [Test]
        public void IsSystemType_ArgumentIsObject_ResultIsTrue()
        {
            Object argument = new Object();

            Assert.That(argument.IsSystemType(), Is.True);
        }

        [Test]
        public void IsSystemType_ArgumentIsString_ResultIsTrue()
        {
            String argument = String.Empty;

            Assert.That(argument.IsSystemType(), Is.True);
        }

        [Test]
        public void IsSystemType_ArgumentIsInt32_ResultIsTrue()
        {
            Int32 argument = 0;

            Assert.That(argument.IsSystemType(), Is.True);
        }

        [Test]
        public void IsSystemType_ArgumentIsCustomStruct_ResultIsFalse()
        {
            CustomStruct argument = new CustomStruct();

            Assert.That(argument.IsSystemType(), Is.False);
        }

        [Test]
        public void IsSystemType_ArgumentIsCustomClass_ResultIsFalse()
        {
            CustomClass argument = new CustomClass();

            Assert.That(argument.IsSystemType(), Is.False);
        }

        #endregion

        #region Private Helpers

        private Object[] ExpandArgumentsHelper(params Object[] arguments)
        {
            return arguments.ExpandArguments();
        }

        private static IEnumerable<TestCaseHelper> MixedContentTestData
        {
            get
            {
                yield return new TestCaseHelper("MixedContent-001", 3, "aaa", new[] { 1, 2 });
                yield return new TestCaseHelper("MixedContent-002", 3, new[] { 1, 2 }, "aaa");
                yield return new TestCaseHelper("MixedContent-003", 3, 1, new[] { "aaa", "bbb" });
                yield return new TestCaseHelper("MixedContent-004", 3, new[] { "aaa", "bbb" }, 1);
                yield return new TestCaseHelper("MixedContent-005", 4, new[] { 1, 2 }, new[] { "aaa", "bbb" });
                yield return new TestCaseHelper("MixedContent-006", 4, new[] { "aaa", "bbb" }, new[] { 1, 2 });
                yield return new TestCaseHelper("MixedContent-007", 7, 42, new[] { "aaa", "bbb" }, StringComparison.Ordinal, new[] { 1, 2 }, "ccc");
                yield return new TestCaseHelper("MixedContent-008", 2, new CustomClass(), new CustomStruct());
                yield return new TestCaseHelper("MixedContent-009", 4, new[] { new CustomClass(), new CustomClass() }, new[] { new CustomStruct(), new CustomStruct() });
                yield return new TestCaseHelper("MixedContent-010", 5, 1, "aaa", new CustomStruct(), new CustomClass(), StringComparison.Ordinal);
            }
        }

        private class TestCaseHelper
        {
            public TestCaseHelper(String name, Int32 expected, params Object[] arguments)
            {
                this.Name = name;
                this.Expected = expected;
                this.Arguments = arguments;

                System.Diagnostics.Debug.Assert(!String.IsNullOrWhiteSpace(this.Name));
            }

            public String Name { get; private set; }

            public Object[] Arguments { get; private set; }

            public Int32 Expected { get; private set; }

            public override String ToString()
            {
                return this.Name;
            }
        }

        private struct CustomStruct { }

        private class CustomClass { }

        #endregion
    }
}
