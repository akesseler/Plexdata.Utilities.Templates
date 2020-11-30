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

namespace Plexdata.Utilities.Formatting.Entities.Tests
{
    [Category("UnitTest")]
    [ExcludeFromCodeCoverage]
    internal class ArgumentRelationsTests
    {
        [Test]
        public void ArgumentRelations_DefaultConstruction_PropertiesAsExpected()
        {
            IArgumentRelations actual = this.CreateInstance(0);

            Assert.That(actual.Count, Is.Zero);
        }

        [TestCase(-1, 0)]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(-1, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(-1, 2)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        public void ArgumentRelations_ArrayOperatorIndexIsInvalid_ResultIsNull(Int32 index, Int32 count)
        {
            IArgumentRelations actual = this.CreateInstance(count);

            Assert.That(actual[index], Is.Null);
        }

        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(1, 2)]
        [TestCase(0, 3)]
        [TestCase(1, 3)]
        [TestCase(2, 3)]
        public void ArgumentRelations_ArrayOperatorIndexIsValid_ResultIsNotNull(Int32 index, Int32 count)
        {
            IArgumentRelations actual = this.CreateInstance(count);

            Assert.That(actual[index], Is.Not.Null);
        }

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("  ", 0)]
        [TestCase("cantfind", 0)]
        [TestCase(null, 1)]
        [TestCase("", 1)]
        [TestCase("  ", 1)]
        [TestCase("cantfind", 1)]
        public void ArgumentRelations_ArrayOperatorLabelIsInvalid_ResultIsNull(String label, Int32 count)
        {
            IArgumentRelations actual = this.CreateInstance(count);

            Assert.That(actual[label], Is.Null);
        }

        [TestCase("label1", 1)]
        [TestCase("label1", 2)]
        [TestCase("label2", 2)]
        [TestCase("label1", 3)]
        [TestCase("label2", 3)]
        [TestCase("label3", 3)]
        public void ArgumentRelations_ArrayOperatorLabelIsValid_ResultIsNotNull(String label, Int32 count)
        {
            IArgumentRelations actual = this.CreateInstance(count);

            Assert.That(actual[label], Is.Not.Null);
        }

        [TestCase(null, 0, -1)]
        [TestCase("", 0, -1)]
        [TestCase("  ", 0, -1)]
        [TestCase("cantfind", 0, -1)]
        [TestCase(null, 1, -1)]
        [TestCase("", 1, -1)]
        [TestCase("  ", 1, -1)]
        [TestCase("cantfind", 1, -1)]
        [TestCase("label1", 1, 0)]
        [TestCase("label1", 2, 0)]
        [TestCase("label2", 2, 1)]
        [TestCase("label1", 3, 0)]
        [TestCase("label2", 3, 1)]
        [TestCase("label3", 3, 2)]
        public void IndexOf_LabelAsProvided_IndexAsExpected(String label, Int32 count, Int32 expected)
        {
            IArgumentRelations actual = this.CreateInstance(count);

            Assert.That(actual.IndexOf(label), Is.EqualTo(expected));
        }

        [TestCase("cantfind", false)]
        [TestCase("cantfind", false, "label1")]
        [TestCase("cantfind", false, "label1", "label2")]
        [TestCase("label1", true, "label1")]
        [TestCase("label1", true, "label1", "label2")]
        [TestCase("label1", true, "label1", "label1")]
        [TestCase("label2", true, "label1", "label2")]
        [TestCase("label2", true, "label2", "label2")]
        public void Contains_LabelAsProvided_ResultAsExpected(String actual, Boolean expected, params String[] labels)
        {
            ArgumentRelations instance = new ArgumentRelations();

            foreach (String label in labels)
            {
                instance.Append(label, null);
            }

            Assert.That(instance.Contains(actual), Is.EqualTo(expected));
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void ToArray_WithDifferentCounts_LengthAsExpected(Int32 count, Int32 expected)
        {
            IArgumentRelations actual = this.CreateInstance(count);

            Assert.That(actual.ToArray().Length, Is.EqualTo(expected));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ToArray_WithDifferentCounts_ItemsAsExpected(Int32 count)
        {
            IArgumentRelations instance = this.CreateInstance(count);

            (String Label, Object Value)[] actual = instance.ToArray();

            for (Int32 index = 0; index < actual.Length; index++)
            {
                (String Label, Object Value) tuple = actual[index];

                Assert.That(tuple.Label, Is.EqualTo($"label{index + 1}"));
                Assert.That(tuple.Value, Is.EqualTo($"value{index + 1}"));
            }
        }

        [TestCase(0, "Count: 0")]
        [TestCase(1, "Count: 1")]
        [TestCase(2, "Count: 2")]
        public void ToString_WithDifferentCounts_ResultAsExpected(Int32 count, String expected)
        {
            IArgumentRelations actual = this.CreateInstance(count);

            Assert.That(actual.ToString(), Is.EqualTo(expected));
        }

        private IArgumentRelations CreateInstance(Int32 count)
        {
            ArgumentRelations instance = new ArgumentRelations();

            for (Int32 index = 1; index < count + 1; index++)
            {
                instance.Append($"label{index}", $"value{index}");
            }

            return instance;
        }
    }
}
