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

using Moq;
using NUnit.Framework;
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Formatting.Tests
{
    [SetCulture("en-US")]
    [Category("IntegrationTest")]
    [ExcludeFromCodeCoverage]
    internal class TemplateTests
    {
        // NOTE: Keep in mind, these tests are just some kind of smoke-tests.

        private Options options;
        private Object[] arguments;

        private Mock<IFormatProvider> provider;
        private Mock<ICustomFormatter> formatter;
        private Mock<IFormattable> formattable;
        private Mock<IArgumentSerializer> serializer;

        [SetUp]
        public void SetUp()
        {
            this.options = new Options();
            this.arguments = new Object[] { 123456, "argument", DateTime.Parse("2020-10-29T23:17:05") };

            this.formatter = new Mock<ICustomFormatter>();
            this.formatter.Setup(x => x.Format(It.IsAny<String>(), It.IsAny<Object>(), It.IsAny<IFormatProvider>()));

            this.provider = new Mock<IFormatProvider>();
            this.provider.Setup(x => x.GetFormat(It.IsAny<Type>())).Returns(this.formatter.Object);

            this.formattable = new Mock<IFormattable>();
            this.formattable.Setup(x => x.ToString(It.IsAny<String>(), It.IsAny<IFormatProvider>()));

            this.serializer = new Mock<IArgumentSerializer>();
        }

        [TestCase("text {1} text {2} text {0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {val1} text {val2} text {val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        [TestCase("text {$1} text {$2} text {$0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {$val1} text {$val2} text {$val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        [TestCase("text {@1} text {@2} text {@0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {@val1} text {@val2} text {@val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        public void Format_FormatArguments_ResultAsExpected(String format, String expected)
        {
            String actual = Template.Format(format, this.arguments);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("text {1} text {2} text {0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {val1} text {val2} text {val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        [TestCase("text {$1} text {$2} text {$0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {$val1} text {$val2} text {$val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        [TestCase("text {@1} text {@2} text {@0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {@val1} text {@val2} text {@val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        public void Format_RelationsFormatArguments_ResultAsExpected(String format, String expected)
        {
            IArgumentRelations relations;
            String actual = Template.Format(out relations, format, this.arguments);

            Assert.That(actual, Is.EqualTo(expected));

            String[] parts = expected.Split(new[] { "text" }, StringSplitOptions.RemoveEmptyEntries);
            for (Int32 index = 0; index < relations.Count; index++)
            {
                String item = relations[index].ToString();
                Assert.That(item, Is.EqualTo(parts[index].Trim()));
            }
        }

        [TestCase("text {1} text {2} text {0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {val1} text {val2} text {val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        [TestCase("text {$1} text {$2} text {$0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {$val1} text {$val2} text {$val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        [TestCase("text {@1} text {@2} text {@0}", "text argument text 10/29/2020 11:17:05 PM text 123456")]
        [TestCase("text {@val1} text {@val2} text {@val3}", "text 123456 text argument text 10/29/2020 11:17:05 PM")]
        public void Format_ProviderRelationsFormatArguments_ResultAsExpected(String format, String expected)
        {
            IArgumentRelations relations;
            String actual = Template.Format(this.provider.Object, out relations, format, this.arguments);

            Assert.That(actual, Is.EqualTo(expected));

            String[] parts = expected.Split(new[] { "text" }, StringSplitOptions.RemoveEmptyEntries);
            for (Int32 index = 0; index < relations.Count; index++)
            {
                String item = relations[index].ToString();
                Assert.That(item, Is.EqualTo(parts[index].Trim()));
            }
        }

        [TestCase("text {1} text {2} text {0}", 0)]
        [TestCase("text {val1} text {val2} text {val3}", 0)]
        [TestCase("text {1:XX} text {2:XX} text {0:XX}", 3)]
        [TestCase("text {val1:XX} text {val2:XX} text {val3:XX}", 3)]
        [TestCase("text {$1} text {$2} text {$0}", 0)]
        [TestCase("text {$val1} text {$val2} text {$val3}", 0)]
        [TestCase("text {$1:XX} text {$2:XX} text {$0:XX}", 0)]
        [TestCase("text {$val1:XX} text {$val2:XX} text {$val3:XX}", 0)]
        [TestCase("text {@1} text {@2} text {@0}", 0)]
        [TestCase("text {@val1} text {@val2} text {@val3}", 0)]
        [TestCase("text {@1:XX} text {@2:XX} text {@0:XX}", 3)]
        [TestCase("text {@val1:XX} text {@val2:XX} text {@val3:XX}", 3)]
        public void Format_ProviderReturnsCustomFormatter_MethodsCalledExpectedTimes(String format, Int32 expected)
        {
            String actual = Template.Format(this.provider.Object, format, this.arguments);

            this.provider.Verify(x => x.GetFormat(It.IsAny<Type>()), Times.Exactly(expected));
            this.formatter.Verify(x => x.Format("XX", It.IsAny<Object>(), this.provider.Object), Times.Exactly(expected));
        }

        [TestCase("text {1} text {2} text {0}", 0)]
        [TestCase("text {val1} text {val2} text {val3}", 0)]
        [TestCase("text {1:XX} text {2:XX} text {0:XX}", 3)]
        [TestCase("text {val1:XX} text {val2:XX} text {val3:XX}", 3)]
        [TestCase("text {$1} text {$2} text {$0}", 0)]
        [TestCase("text {$val1} text {$val2} text {$val3}", 0)]
        [TestCase("text {$1:XX} text {$2:XX} text {$0:XX}", 0)]
        [TestCase("text {$val1:XX} text {$val2:XX} text {$val3:XX}", 0)]
        [TestCase("text {@1} text {@2} text {@0}", 0)]
        [TestCase("text {@val1} text {@val2} text {@val3}", 0)]
        [TestCase("text {@1:XX} text {@2:XX} text {@0:XX}", 0)]
        [TestCase("text {@val1:XX} text {@val2:XX} text {@val3:XX}", 0)]
        public void Format_ProviderReturnsNothing_MethodsCalledExpectedTimes(String format, Int32 expected)
        {
            this.arguments = new Object[] { this.formattable.Object, this.formattable.Object, this.formattable.Object };
            this.provider.Setup(x => x.GetFormat(It.IsAny<Type>())).Returns(null);

            String actual = Template.Format(this.provider.Object, format, this.arguments);

            this.provider.Verify(x => x.GetFormat(It.IsAny<Type>()), Times.Exactly(expected));
            this.formattable.Verify(x => x.ToString("XX", this.provider.Object), Times.Exactly(expected));
        }

        [Test]
        public void Format_SerializerFormatArguments_SerializerSerializeCalledOnce()
        {
            TestClassNoSystemType candiate = new TestClassNoSystemType();

            String actual = Template.Format(this.serializer.Object, "text {@val1,-20:XYZ} text", new Object[] { candiate });

            this.serializer.Verify(x => x.Serialize(It.IsAny<IFormatProvider>(), "XYZ", "-20", candiate), Times.Once);
        }

        [Test]
        public void Format_SerializerRelationsFormatArguments_SerializerSerializeCalledOnceAndRelationsWithCountOne()
        {
            TestClassNoSystemType candiate = new TestClassNoSystemType();

            IArgumentRelations relations;
            String actual = Template.Format(this.serializer.Object, out relations, "text {@val1,-20:XYZ} text", new Object[] { candiate });

            this.serializer.Verify(x => x.Serialize(It.IsAny<IFormatProvider>(), "XYZ", "-20", candiate), Times.Once);

            Assert.That(relations.Count, Is.EqualTo(1));
        }

        [Test]
        public void Format_ProviderSerializerFormatArguments_SerializerSerializeCalledOnce()
        {
            TestClassNoSystemType candiate = new TestClassNoSystemType();

            String actual = Template.Format(this.provider.Object, this.serializer.Object, "text {@val1,-20:XYZ} text", new Object[] { candiate });

            this.serializer.Verify(x => x.Serialize(this.provider.Object, "XYZ", "-20", candiate), Times.Once);
        }

        [Test]
        public void Format_ProviderSerializerRelationsFormatArguments_SerializerSerializeCalledOnceAndRelationsWithCountOne()
        {
            TestClassNoSystemType candiate = new TestClassNoSystemType();

            IArgumentRelations relations;
            String actual = Template.Format(this.provider.Object, this.serializer.Object, out relations, "text {@val1,-20:XYZ} text", new Object[] { candiate });

            this.serializer.Verify(x => x.Serialize(this.provider.Object, "XYZ", "-20", candiate), Times.Once);

            Assert.That(relations.Count, Is.EqualTo(1));
        }

        [TestCase("text {1} text {2} text {0}", "text broken text argument text 1...")]
        [TestCase("text {val1} text {val2} text {val3}", "text 123456 text broken text arg...")]
        [TestCase("text {$1} text {$2} text {$0}", "text broken text argument text 1...")]
        [TestCase("text {$val1} text {$val2} text {$val3}", "text 123456 text broken text arg...")]
        [TestCase("text {@1} text {@2} text {@0}", "text broken text argument text 1...")]
        [TestCase("text {@val1} text {@val2} text {@val3}", "text 123456 text broken text arg...")]
        public void Format_OptionsFormatArguments_ResultAsExpected(String format, String expected)
        {
            this.arguments = new Object[] { 123456, null, "argument" };
            this.options.Fallback = "broken";
            this.options.Maximum = 35;

            String actual = Template.Format(this.options, format, this.arguments);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("text {1} text {2} text {0}", "text broken text argument text 1...")]
        [TestCase("text {val1} text {val2} text {val3}", "text 123456 text broken text arg...")]
        [TestCase("text {$1} text {$2} text {$0}", "text broken text argument text 1...")]
        [TestCase("text {$val1} text {$val2} text {$val3}", "text 123456 text broken text arg...")]
        [TestCase("text {@1} text {@2} text {@0}", "text broken text argument text 1...")]
        [TestCase("text {@val1} text {@val2} text {@val3}", "text 123456 text broken text arg...")]
        public void Format_OptionsRelationsFormatArguments_ResultAsExpected(String format, String expected)
        {
            this.arguments = new Object[] { 123456, null, "argument" };
            this.options.Fallback = "broken";
            this.options.Maximum = 35;

            IArgumentRelations relations;
            String actual = Template.Format(this.options, out relations, format, this.arguments);

            Assert.That(actual, Is.EqualTo(expected));

            Assert.That(relations.Count, Is.EqualTo(3));
        }

        [TestCase(1, "a", "a")]
        [TestCase(1, "ab", "a")]
        [TestCase(1, "abc", "a")]
        [TestCase(1, "abcd", "a")]
        [TestCase(1, "abcde", "a")]
        [TestCase(1, "abcdef", "a")]
        [TestCase(1, "abcdefg", "a")]
        [TestCase(2, "a", "a")]
        [TestCase(2, "ab", "ab")]
        [TestCase(2, "abc", "ab")]
        [TestCase(2, "abcd", "ab")]
        [TestCase(2, "abcde", "ab")]
        [TestCase(2, "abcdef", "ab")]
        [TestCase(2, "abcdefg", "ab")]
        [TestCase(3, "a", "a")]
        [TestCase(3, "ab", "ab")]
        [TestCase(3, "abc", "abc")]
        [TestCase(3, "abcd", "abc")]
        [TestCase(3, "abcde", "abc")]
        [TestCase(3, "abcdef", "abc")]
        [TestCase(3, "abcdefg", "abc")]
        [TestCase(4, "a", "a")]
        [TestCase(4, "ab", "ab")]
        [TestCase(4, "abc", "abc")]
        [TestCase(4, "abcd", "abcd")]
        [TestCase(4, "abcde", "a...")]
        [TestCase(4, "abcdef", "a...")]
        [TestCase(4, "abcdefg", "a...")]
        [TestCase(5, "a", "a")]
        [TestCase(5, "ab", "ab")]
        [TestCase(5, "abc", "abc")]
        [TestCase(5, "abcd", "abcd")]
        [TestCase(5, "abcde", "abcde")]
        [TestCase(5, "abcdef", "ab...")]
        [TestCase(5, "abcdefg", "ab...")]
        public void Format_OutputTruncation_ResultAsExpected(Int32 maximum, String format, String expected)
        {
            this.options.Maximum = maximum;

            String actual = Template.Format(this.options, format, null);

            Assert.That(actual, Is.EqualTo(expected));
        }

        private class TestClassNoSystemType { }
    }
}
