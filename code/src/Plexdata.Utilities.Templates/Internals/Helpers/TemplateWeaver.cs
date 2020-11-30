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

using Plexdata.Utilities.Formatting.Defines;
using Plexdata.Utilities.Formatting.Entities;
using Plexdata.Utilities.Formatting.Extensions;
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Plexdata.Utilities.Formatting.Helpers
{
    /// <summary>
    /// This class <em>weaves</em> the result.
    /// </summary>
    /// <remarks>
    /// In other words, this class assembles the result.
    /// </remarks>
    internal static class TemplateWeaver
    {
        #region Private Fields

        /// <summary>
        /// This field holds the fallback serializer.
        /// </summary>
        /// <remarks>
        /// The fallback serializer is used in every case if no other serializer 
        /// is used to turn complex data types and classes into their string 
        /// representation.
        /// </remarks>
        private readonly static IArgumentSerializer FallbackSerializer = new DefaultSerializer();

        #endregion

        #region Construction

        /// <summary>
        /// Default static construction.
        /// </summary>
        /// <remarks>
        /// This constructor does nothing else but to initialize all 
        /// static members.
        /// </remarks>
        static TemplateWeaver() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assembles the result string.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method assembles the result string according to the 
        /// template formatting rules.
        /// </para>
        /// <para>
        /// Please note, debug assertions can occur if one of the 
        /// parameters is recognized as invalid. But an exception is 
        /// (hopefully) never thrown.
        /// </para>
        /// </remarks>
        /// <param name="options">
        /// An instance of class <see cref="Options"/> to be used to 
        /// get additional settings from that may affect the output.
        /// The <see cref="Options.Default"/> options are used if this 
        /// parameter is <c>null</c>.
        /// </param>
        /// <param name="tokens">
        /// The list of tokens that describe how to build the output.
        /// Nothing is gonna happen if this parameter is <c>null</c> 
        /// or <c>empty</c>.
        /// </param>
        /// <param name="output">
        /// An instance of a <see cref="StringBuilder"/> containing the 
        /// formatted result. Nothing is gonna happen if this parameter 
        /// is <c>null</c>.
        /// </param>
        /// <param name="arguments">
        /// The list of arguments to be out into the result. An empty 
        /// object list is created if this parameter is <c>null</c>. 
        /// This may happen in cases of providing just one <c>null</c> 
        /// argument.
        /// </param>
        /// <seealso cref="IsWeaveAllByIndex(IEnumerable{BaseToken})"/>
        /// <seealso cref="WeaveByIndex(IEnumerable{BaseToken}, Options, StringBuilder, Object[])"/>
        /// <seealso cref="WeaveByLabel(IEnumerable{BaseToken}, Options, StringBuilder, Object[])"/>
        public static void Weave(Options options, IEnumerable<BaseToken> tokens, StringBuilder output, Object[] arguments)
        {
            // An assertion should be enough to keep informed about 
            // "mistakes", because of this class is publicly invisible.
            Debug.Assert(options != null);
            Debug.Assert(tokens != null && tokens.Any());
            Debug.Assert(output != null);
            Debug.Assert(arguments != null);

            if (options == null) { options = Options.Default; }
            if (tokens == null || !tokens.Any()) { return; }
            if (output == null) { return; }
            if (arguments == null) { arguments = new Object[0]; }

            // https://messagetemplates.org#capturing-rules
            //
            // => Templates that use numeric property names like {0} and {1} exclusively imply 
            //    that arguments to the template are captured by numeric index.
            // => If any of the property names are non-numeric, then all arguments are captured 
            //    by matching left-to-right with holes in the order in which they appear.
            //
            // For my understanding it means: All tokens are processed from left to right as soon 
            // as at least one token appears that does not have a numerical index.

            if (tokens.IsWeaveAllByIndex())
            {
                tokens.WeaveByIndex(options, output, arguments);
            }
            else
            {
                tokens.WeaveByLabel(options, output, arguments);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the formatted output by index.
        /// </summary>
        /// <remarks>
        /// This method reassembles all <paramref name="tokens"/> and 
        /// inserts all arguments by the order of their index.
        /// </remarks>
        /// <param name="tokens">
        /// The tokens to be assembled.
        /// </param>
        /// <param name="options">
        /// The options to be used.
        /// </param>
        /// <param name="output">
        /// The output where to put the results.
        /// </param>
        /// <param name="arguments">
        /// The list of arguments to be processed.
        /// </param>
        private static void WeaveByIndex(this IEnumerable<BaseToken> tokens, Options options, StringBuilder output, Object[] arguments)
        {
            foreach (BaseToken token in tokens)
            {
                if (token is TextToken)
                {
                    token.WeaveTextToken(output);
                }
                else if (token is HoleToken)
                {
                    // In this case the index is taken. This can be 
                    // either the real index or the corresponding 
                    // left-to-right position.
                    token.WeaveHoleToken(options, token.ToIndex(), output, arguments);
                }
            }
        }

        /// <summary>
        /// Creates the formatted output by label.
        /// </summary>
        /// <remarks>
        /// This method reassembles all <paramref name="tokens"/> and 
        /// inserts all arguments by left-to-right order.
        /// </remarks>
        /// <param name="tokens">
        /// The tokens to be assembled.
        /// </param>
        /// <param name="options">
        /// The options to be used.
        /// </param>
        /// <param name="output">
        /// The output where to put the results.
        /// </param>
        /// <param name="arguments">
        /// The list of arguments to be processed.
        /// </param>
        private static void WeaveByLabel(this IEnumerable<BaseToken> tokens, Options options, StringBuilder output, Object[] arguments)
        {
            foreach (BaseToken token in tokens)
            {
                if (token is TextToken)
                {
                    token.WeaveTextToken(output);
                }
                else if (token is HoleToken)
                {
                    // In this case the left-to-right position 
                    // actually overwrites the usage of an index.
                    token.WeaveHoleToken(options, token.Rating, output, arguments);
                }
            }
        }

        /// <summary>
        /// Appends a text token to the output.
        /// </summary>
        /// <remarks>
        /// This method appends the <see cref="BaseToken.Marker"/> 
        /// of an instance of class <see cref="TextToken"/> to the 
        /// output.
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <param name="output">
        /// The output where to put the results.
        /// </param>
        private static void WeaveTextToken(this BaseToken token, StringBuilder output)
        {
            output.Append(token.Marker);
        }

        /// <summary>
        /// Appends a void token to the output.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method handles formatting of all tokens for which 
        /// their related argument is null.
        /// </para>
        /// <para>
        /// The value of <see cref="Options.Fallback"/> of the <paramref name="options"/> 
        /// is used but only if set. Otherwise the <see cref="BaseToken.Marker"/> of 
        /// <see cref="HoleToken"/> is used.
        /// </para>
        /// <para>
        /// This behavior fits the rendering semantic of the message template definition. See also 
        /// <a href="https://messagetemplates.org" target="_blank">https://messagetemplates.org</a>.
        /// </para>
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <param name="options">
        /// The options to be used.
        /// </param>
        /// <param name="output">
        /// The output where to put the results.
        /// </param>
        private static void WeaveVoidToken(this BaseToken token, Options options, StringBuilder output)
        {
            // SEE: https://messagetemplates.org#rendering-semantics

            if (options.IsFallback)
            {
                output.Append(options.Fallback);
            }
            else
            {
                output.Append(token.Marker);
            }
        }

        /// <summary>
        /// Formats the related argument and appends it to the result.
        /// </summary>
        /// <remarks>
        /// This method formats the related argument and appends it to the result. 
        /// Furthermore, value padding (if needed) is also applied.
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <param name="options">
        /// The options to be used.
        /// </param>
        /// <param name="index">
        /// The zero-based index of the related argument.
        /// </param>
        /// <param name="output">
        /// The output where to put the results.
        /// </param>
        /// <param name="arguments">
        /// The list of arguments to be processed.
        /// </param>
        private static void WeaveHoleToken(this BaseToken token, Options options, Int32 index, StringBuilder output, Object[] arguments)
        {
            Object argument = null;

            if (index >= 0 && index < arguments.Length)
            {
                argument = arguments[index];
            }

            if (argument == null)
            {
                token.WeaveVoidToken(options, output);
                return;
            }

            String value = token.GetFormattedValue(options, argument);

            token.AddPadding(token.IsRightJustified, output, value);

            output.Append(value);

            token.AddPadding(token.IsLeftJustified, output, value);
        }

        /// <summary>
        /// Converts the <paramref name="argument"/> into its string 
        /// representation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method converts the <paramref name="argument"/> into its string 
        /// representation by taking their specific format instructions into account.
        /// </para>
        /// <para>
        /// The argument is stringified if its format instruction starts with <c>'$'</c>.
        /// </para>
        /// <para>
        /// The argument is serialized if its format instruction starts with <c>'@'</c>.
        /// </para>
        /// <para>
        /// The argument is formatted if its format instruction does neither start with 
        /// <c>'$'</c> nor with <c>'@'</c>.
        /// </para>
        /// <para>
        /// Furthermore, in case of an exception the result contains the exception name 
        /// and message instead.
        /// </para>
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <param name="options">
        /// The options to be used.
        /// </param>
        /// <param name="argument">
        /// The argument to be formatted.
        /// </param>
        /// <returns>
        /// The argument in its formatted representation.
        /// </returns>
        private static String GetFormattedValue(this BaseToken token, Options options, Object argument)
        {
            try
            {
                if (token.IsStringify)
                {
                    return token.ToStringifyValue(options, argument);
                }
                else if (token.IsSpreading)
                {
                    return token.ToSpreadingValue(options, argument);
                }
                else
                {
                    return token.ToFormattedValue(options, argument);
                }
            }
            catch (Exception exception)
            {
                return token.HandleException(exception);
            }
        }

        /// <summary>
        /// Converts the argument into a string.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="argument"/> into a 
        /// string by calling method <see cref="Object.ToString()"/>.
        /// </remarks>
        /// <param name="token">
        /// Actually unused.
        /// </param>
        /// <param name="options">
        /// Actually unused.
        /// </param>
        /// <param name="argument">
        /// The argument to call method <see cref="Object.ToString()"/> 
        /// for.
        /// </param>
        /// <returns>
        /// The argument in its string representation.
        /// </returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static String ToStringifyValue(this BaseToken token, Options options, Object argument)
        {
            return argument.ToString();
        }

        /// <summary>
        /// Converts the argument by serializing it.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="argument"/> by serializing it. 
        /// But keep in mind, serialization takes place only for complex data types. 
        /// The <paramref name="argument"/> is only formatted if it is recognized as 
        /// system type.
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <param name="options">
        /// The options to be used.
        /// </param>
        /// <param name="argument">
        /// The argument to be formatted.
        /// </param>
        /// <returns>
        /// The argument in its formatted representation.
        /// </returns>
        private static String ToSpreadingValue(this BaseToken token, Options options, Object argument)
        {
            if (argument.IsSystemType())
            {
                return token.ToFormattedValue(options, argument);
            }
            else
            {
                IArgumentSerializer serializer = options.Serializer ?? TemplateWeaver.FallbackSerializer;

                return serializer.Serialize(options.Provider, token.Format, token.Lining, argument) ?? String.Empty;
            }
        }

        /// <summary>
        /// Turns the argument into its formatted representation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method turns the argument into its formatted representation.
        /// </para>
        /// <para>
        /// Method <see cref="ICustomFormatter.Format(String, Object, IFormatProvider)"/> 
        /// is called if the argument is derived from <see cref="ICustomFormatter"/>.
        /// </para>
        /// <para>
        /// Method <see cref="IFormattable.ToString(String, IFormatProvider)"/> is called 
        /// if the argument is derived from <see cref="IFormattable"/>.
        /// </para>
        /// <para>
        /// Method <see cref="Object.ToString()"/> is called in any other case..
        /// </para>
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <param name="options">
        /// The options to be used.
        /// </param>
        /// <param name="argument">
        /// The argument to be formatted.
        /// </param>
        /// <returns>
        /// The argument in its formatted representation.
        /// </returns>
        private static String ToFormattedValue(this BaseToken token, Options options, Object argument)
        {
            if (token.Format.Length < 1)
            {
                return argument.ToString();
            }

            ICustomFormatter custom = null;

            if (options.Provider != null)
            {
                custom = options.Provider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
            }

            if (custom != null)
            {
                return custom.Format(token.Format, argument, options.Provider);
            }
            else if (argument is IFormattable helper)
            {
                return helper.ToString(token.Format, options.Provider);
            }
            else
            {
                return argument.ToString();
            }
        }

        /// <summary>
        /// This method determines whether all hole tokens 
        /// can be handled by index.
        /// </summary>
        /// <remarks>
        /// According to message template rules, all arguments can only be 
        /// rendered by index if no named token is part of the whole format 
        /// string. This is, what this method determines.
        /// </remarks>
        /// <param name="tokens">
        /// The list of tokens to be checked.
        /// </param>
        /// <returns>
        /// True if all included hole tokens can be processed 
        /// by index and false if at least one hole token must 
        /// be processed by left-to-right position.
        /// </returns>
        /// <seealso cref="BaseToken.Rating"/>
        /// <seealso cref="BaseToken.IsNumbering"/>
        /// <seealso cref="BaseToken.ToIndex()"/>
        private static Boolean IsWeaveAllByIndex(this IEnumerable<BaseToken> tokens)
        {
            foreach (BaseToken token in tokens)
            {
                if (token is HoleToken && !token.IsNumbering)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds value padding if necessary.
        /// </summary>
        /// <remarks>
        /// This method adds left or right padding to the value but only if 
        /// padding becomes enabled. Please note, the padding instruction is 
        /// part of the formatting instruction. Furthermore, no padding is 
        /// added if the value length exceeds the total padding length.
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <param name="enabled">
        /// True if padding is enabled and false if not.
        /// </param>
        /// <param name="output">
        /// The output where to put the padding.
        /// </param>
        /// <param name="value">
        /// The value to be processed.
        /// </param>
        private static void AddPadding(this BaseToken token, Boolean enabled, StringBuilder output, String value)
        {
            if (!enabled)
            {
                return;
            }

            Int32 padding = token.GetPadding() - value.Length;

            if (padding < 1)
            {
                return;
            }

            output.Append(TokenLiterals.ExpandToken, padding);
        }

        /// <summary>
        /// Gets the padding length.
        /// </summary>
        /// <remarks>
        /// This method calculates the padding length and returns it.
        /// </remarks>
        /// <param name="token">
        /// The token to be processed.
        /// </param>
        /// <returns>
        /// The total number of padding characters or 0 (zero) if no 
        /// padding information exist.
        /// </returns>
        private static Int32 GetPadding(this BaseToken token)
        {
            if (Int32.TryParse(token.Lining, out Int32 padding))
            {
                return padding < 0 ? -1 * padding : padding;
            }

            return 0;
        }

        /// <summary>
        /// Converts an exception into a string and returns it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method converts an exception into a string and returns it. The result 
        /// consists of the token <see cref="BaseToken.Marker"/>, of the exception type 
        /// name as well as of the exception <see cref="Exception.Message"/>.
        /// </para>
        /// <para>
        /// Additionally, the affected token and the whole exception is printed onto the 
        /// debug output console.
        /// </para>
        /// </remarks>
        /// <param name="token">
        /// The token causing the exception.
        /// </param>
        /// <param name="exception">
        /// The exception that has been thrown.
        /// </param>
        /// <returns>
        /// A string with (hopefully) sufficient exception information.
        /// </returns>
        private static String HandleException(this BaseToken token, Exception exception)
        {
            Debug.WriteLine("==================================================");
            Debug.WriteLine(token);
            Debug.WriteLine("--------------------------------------------------");
            Debug.WriteLine(exception);
            Debug.WriteLine("==================================================");

            return $"[{token.Marker} => {exception.GetType().Name}: \"{exception.Message}\"]";
        }

        #endregion
    }
}
