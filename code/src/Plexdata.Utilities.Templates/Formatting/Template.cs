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

using Plexdata.Utilities.Formatting.Entities;
using Plexdata.Utilities.Formatting.Extensions;
using Plexdata.Utilities.Formatting.Helpers;
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plexdata.Utilities.Formatting
{
    /// <summary>
    /// This class represents the <em>Plexdata Template Formatter</em>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <em>Plexdata Template Formatter</em> can be used like the equivalent 
    /// <c>Format</c> methods of class <c>String</c> but with full template support.
    /// </para>
    /// <para>
    /// More information about template formatting can be found on the Internet under 
    /// <a href="https://messagetemplates.org" target="_blank">https://messagetemplates.org</a>.
    /// </para>
    /// </remarks>
    /// <example>
    /// Here are some examples to illustrate a usage.
    /// <code language="cs">
    /// // Both examples would produce an output like:
    /// // "Date: 10/29/20, Buyer: John Doe, Sales: $1,234.57"
    /// 
    /// Template.Format(
    ///     "Date: {0:MM/dd/yy}, Buyer: {1}, Sales: {2:C}", 
    ///     date, name, sales);
    /// 
    /// Template.Format(
    ///     "Date: {date:MM/dd/yy}, Buyer: {buyer}, Sales: {sales:C}", 
    ///     date, name, sales);
    /// </code>
    /// </example>
    public static class Template
    {
        #region Public Methods

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string. This is for sure the simplest way of template formatting.
        /// </remarks>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(IFormatProvider, String, Object[])"/>
        /// <seealso cref="Template.Format(IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(String format, params Object[] arguments)
        {
            return Template.Format(true, Options.Create(null, null), out _, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string. This is for sure the simplest way of template formatting.
        /// </remarks>
        /// <param name="relations">
        /// When this method returns, this parameter contains a list of Label-Value relations consisting 
        /// of the labels taken from <paramref name="format"/> and the <paramref name="arguments"/> as 
        /// their values.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(IFormatProvider, String, Object[])"/>
        /// <seealso cref="Template.Format(IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(out IArgumentRelations relations, String format, params Object[] arguments)
        {
            return Template.Format(false, Options.Create(null, null), out relations, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string 
        /// using given format <paramref name="provider"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string using given format <paramref name="provider"/>.
        /// </remarks>
        /// <param name="provider">
        /// An instance of a <see cref="IFormatProvider"/> derived class used the to perform culture-specific 
        /// formatting.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(String, Object[])"/>
        /// <seealso cref="Template.Format(IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(IFormatProvider provider, String format, params Object[] arguments)
        {
            return Template.Format(true, Options.Create(provider, null), out _, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string 
        /// using given format <paramref name="provider"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string using given format <paramref name="provider"/>.
        /// </remarks>
        /// <param name="provider">
        /// An instance of a <see cref="IFormatProvider"/> derived class used the to perform culture-specific 
        /// formatting.
        /// </param>
        /// <param name="relations">
        /// When this method returns, this parameter contains a list of Label-Value relations consisting 
        /// of the labels taken from <paramref name="format"/> and the <paramref name="arguments"/> as 
        /// their values.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(String, Object[])"/>
        /// <seealso cref="Template.Format(IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(IFormatProvider provider, out IArgumentRelations relations, String format, params Object[] arguments)
        {
            return Template.Format(false, Options.Create(provider, null), out relations, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string 
        /// using given argument <paramref name="serializer"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string using given argument <paramref name="serializer"/>.
        /// </remarks>
        /// <param name="serializer">
        /// An instance of a <see cref="IArgumentSerializer"/> derived class used the to perform any kind 
        /// of serialization for custom types.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(IArgumentSerializer serializer, String format, params Object[] arguments)
        {
            return Template.Format(true, Options.Create(null, serializer), out _, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string 
        /// using given argument <paramref name="serializer"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string using given argument <paramref name="serializer"/>.
        /// </remarks>
        /// <param name="serializer">
        /// An instance of a <see cref="IArgumentSerializer"/> derived class used the to perform any kind 
        /// of serialization for custom types.
        /// </param>
        /// <param name="relations">
        /// When this method returns, this parameter contains a list of Label-Value relations consisting 
        /// of the labels taken from <paramref name="format"/> and the <paramref name="arguments"/> as 
        /// their values.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(IArgumentSerializer serializer, out IArgumentRelations relations, String format, params Object[] arguments)
        {
            return Template.Format(false, Options.Create(null, serializer), out relations, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string using 
        /// given format <paramref name="provider"/> as well as given argument <paramref name="serializer"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string 
        /// using given format <paramref name="provider"/> as well as given argument <paramref name="serializer"/>.
        /// </remarks>
        /// <param name="provider">
        /// An instance of a <see cref="IFormatProvider"/> derived class used the to perform culture-specific 
        /// formatting.
        /// </param>
        /// <param name="serializer">
        /// An instance of a <see cref="IArgumentSerializer"/> derived class used the to perform any kind 
        /// of serialization for custom types.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, String, Object[])"/>
        /// <seealso cref="Template.Format(IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(IFormatProvider provider, IArgumentSerializer serializer, String format, params Object[] arguments)
        {
            return Template.Format(true, Options.Create(provider, serializer), out _, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string using 
        /// given format <paramref name="provider"/> as well as given argument <paramref name="serializer"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string 
        /// using given format <paramref name="provider"/> as well as given argument <paramref name="serializer"/>.
        /// </remarks>
        /// <param name="provider">
        /// An instance of a <see cref="IFormatProvider"/> derived class used the to perform culture-specific 
        /// formatting.
        /// </param>
        /// <param name="serializer">
        /// An instance of a <see cref="IArgumentSerializer"/> derived class used the to perform any kind 
        /// of serialization for custom types.
        /// </param>
        /// <param name="relations">
        /// When this method returns, this parameter contains a list of Label-Value relations consisting 
        /// of the labels taken from <paramref name="format"/> and the <paramref name="arguments"/> as 
        /// their values.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        /// <seealso cref="Template.Format(String, Object[])"/>
        /// <seealso cref="Template.Format(IFormatProvider, String, Object[])"/>
        /// <seealso cref="Template.Format(IArgumentSerializer, String, Object[])"/>
        /// <seealso cref="Template.Format(Options, String, Object[])"/>
        public static String Format(IFormatProvider provider, IArgumentSerializer serializer, out IArgumentRelations relations, String format, params Object[] arguments)
        {
            return Template.Format(false, Options.Create(provider, serializer), out relations, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string using 
        /// given <paramref name="options"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string using given <paramref name="options"/>.
        /// </remarks>
        /// <param name="options">
        /// An instance of class <see cref="Options"/> with detailed formatting instructions.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        public static String Format(Options options, String format, params Object[] arguments)
        {
            return Template.Format(true, options, out _, format, arguments);
        }

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string using 
        /// given <paramref name="options"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string using given <paramref name="options"/>.
        /// </remarks>
        /// <param name="options">
        /// An instance of class <see cref="Options"/> with detailed formatting instructions.
        /// </param>
        /// <param name="relations">
        /// When this method returns, this parameter contains a list of Label-Value relations consisting 
        /// of the labels taken from <paramref name="format"/> and the <paramref name="arguments"/> as 
        /// their values.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        public static String Format(Options options, out IArgumentRelations relations, String format, params Object[] arguments)
        {
            return Template.Format(false, options, out relations, format, arguments);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Formats the list of <paramref name="arguments"/> into the <paramref name="format"/> string using 
        /// given <paramref name="options"/>.
        /// </summary>
        /// <remarks>
        /// This method formats the list of <paramref name="arguments"/> into the <paramref name="format"/> 
        /// string using given <paramref name="options"/>.
        /// </remarks>
        /// <param name="discard">
        /// This parameter is only for internal handling and tells the method to resolve relation assignments 
        /// or not.
        /// </param>
        /// <param name="options">
        /// An instance of class <see cref="Options"/> with detailed formatting instructions.
        /// </param>
        /// <param name="relations">
        /// When this method returns, this parameter contains a list of Label-Value relations consisting 
        /// of the labels taken from <paramref name="format"/> and the <paramref name="arguments"/> as 
        /// their values.
        /// </param>
        /// <param name="format">
        /// A string containing formatting instructions, either for index-based formatting, like 
        /// <c>"{0}, {1}, ..."</c>, or for template-based formatting, like <c>"{name1}, {name2}, ..."</c>).
        /// </param>
        /// <param name="arguments">
        /// The optional list of arguments to be used.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="format"/> with the format elements replaced by the string representation 
        /// of the corresponding objects in <paramref name="arguments"/>.
        /// </returns>
        private static String Format(Boolean discard, Options options, out IArgumentRelations relations, String format, params Object[] arguments)
        {
            relations = RelationsAssigner.Default;

            if (format == null || format.Length == 0)
            {
                return String.Empty;
            }

            StringBuilder output = new StringBuilder(512);

            IEnumerable<BaseToken> tokens = TemplateParser.Parse(format);

            arguments = arguments.ExpandArguments();

            TemplateWeaver.Weave(options, tokens, output, arguments);

            if (!discard)
            {
                relations = RelationsAssigner.Assign(tokens, arguments);
            }

            return Template.Truncate(options, output).ToString();
        }

        /// <summary>
        /// Cuts off the <paramref name="builder"/>, if necessary.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method cuts off the value of <paramref name="builder"/>, but only 
        /// if its length exceeds the <see cref="Options.Maximum"/> length defined 
        /// in provided <paramref name="options"/>.
        /// </para>
        /// <para>
        /// Triple dots are appended to the builder in case of its content has been 
        /// cut off.
        /// </para>
        /// </remarks>
        /// <param name="options">
        /// The options to take the maximum allowed length from.
        /// </param>
        /// <param name="builder">
        /// The <paramref name="builder"/> whose content must be limited.
        /// </param>
        /// <returns>
        /// The <paramref name="builder"/> whose content has been limited.
        /// </returns>
        /// <seealso cref="Options.Maximum"/>
        /// <seealso cref="Options.IsLimited"/>
        private static StringBuilder Truncate(Options options, StringBuilder builder)
        {
            if (!options.IsLimited)
            {
                return builder;
            }

            const String trailing = "...";

            Int32 count = builder.Length;
            Int32 limit = options.Maximum;
            Int32 delta = trailing.Length;

            if (count <= limit)
            {
                return builder;
            }

            // Cut off builder content up to maximum length.
            builder.Length = limit;

            if (count <= delta || limit <= delta)
            {
                return builder;
            }

            // Cut off length of "triple dots" to be able to append truncation indicator.
            builder.Length -= delta;
            builder.Append(trailing);

            return builder;
        }

        #endregion
    }
}
