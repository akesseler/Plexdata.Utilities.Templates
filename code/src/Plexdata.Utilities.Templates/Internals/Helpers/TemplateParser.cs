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
using System;
using System.Collections.Generic;
using System.Text;

namespace Plexdata.Utilities.Formatting.Helpers
{
    /// <summary>
    /// Performs the template parsing.
    /// </summary>
    /// <remarks>
    /// This class does the actual template parsing and creates a list 
    /// of recognized formatting tokens that are used later on for the 
    /// real string formatting.
    /// </remarks>
    internal static class TemplateParser
    {
        #region Private Fields

        /// <summary>
        /// This field holds the constant value of default capacity.
        /// </summary>
        /// <remarks>
        /// The value of this field is used to create instances 
        /// of <see cref="StringBuilder"/> and provide them with 
        /// a pre-defined caparity. The default value is <c>128</c> 
        /// characters.
        /// </remarks>
        private const Int32 DefaultCapacity = 128;

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses a <paramref name="format"/> string and returns a list of 
        /// corresponding <see cref="BaseToken"/>s.
        /// </summary>
        /// <remarks>
        /// This method parses the provided <paramref name="format"/> string 
        /// and returns a list of corresponding <see cref="BaseToken"/>s.
        /// </remarks>
        /// <param name="format">
        /// The format string to be parsed.
        /// </param>
        /// <returns>
        /// A list of <see cref="BaseToken"/>s. An empty list is returned 
        /// in case of given <paramref name="format"/> string is invalid.
        /// </returns>
        public static IEnumerable<BaseToken> Parse(String format)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                yield break;
            }

            StringBuilder source = new StringBuilder(format);
            Int32 length = source.Length;
            Int32 behind = 0;
            Int32 rating = 0;

            while (true)
            {
                BaseToken token;
                Int32 index = behind;

                token = TemplateParser.ParseTextMarker(index, ref behind, source);

                if (behind > index)
                {
                    yield return token;
                }

                if (behind >= length)
                {
                    yield break;
                }

                index = behind;

                token = TemplateParser.ParseHoleMarker(index, ref behind, ref rating, source);

                if (behind > index)
                {
                    yield return token;
                }

                if (behind >= length)
                {
                    yield break;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a text marker.
        /// </summary>
        /// <remarks>
        /// This method tries to find the next text marker within the original 
        /// format string and returns it as instance of class <see cref="TextToken"/>.
        /// </remarks>
        /// <param name="index">
        /// The index at where to start parsing.
        /// </param>
        /// <param name="after">
        /// The next index behind the text token.
        /// </param>
        /// <param name="source">
        /// The source format string.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="TextToken"/> containing the text part.
        /// </returns>
        private static BaseToken ParseTextMarker(Int32 index, ref Int32 after, StringBuilder source)
        {
            StringBuilder buffer = new StringBuilder(TemplateParser.DefaultCapacity);

            Int32 length = source.Length;
            Int32 offset = index;

            while (index < length)
            {
                Char token = source[index];

                if (token == TokenLiterals.OpenedToken)
                {
                    if (index + 1 < length && source[index + 1] == TokenLiterals.OpenedToken)
                    {
                        buffer.Append(token);
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (token == TokenLiterals.ClosedToken)
                    {
                        if (index + 1 < length && source[index + 1] == TokenLiterals.ClosedToken)
                        {
                            index++;
                        }
                    }

                    buffer.Append(token);
                }

                index++;

            }

            after = index;

            return new TextToken(offset, buffer);
        }

        /// <summary>
        /// Parses a hole marker.
        /// </summary>
        /// <remarks>
        /// This method tries to find the next hole marker within the original 
        /// format string and returns it as instance of class <see cref="HoleToken"/>.
        /// </remarks>
        /// <param name="index">
        /// The index at where to start parsing.
        /// </param>
        /// <param name="after">
        /// The next index behind the hole token.
        /// </param>
        /// <param name="rating">
        /// The rating is nothing else but another expression of index and 
        /// represents the zero-based offset within the format string.
        /// </param>
        /// <param name="source">
        /// The source format string.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="HoleToken"/> containing the formatting 
        /// part or an instance of class <see cref="TextToken"/> when the formatting 
        /// part was recognized as invalid format definition..
        /// </returns>
        private static BaseToken ParseHoleMarker(Int32 index, ref Int32 after, ref Int32 rating, StringBuilder source)
        {
            StringBuilder buffer = new StringBuilder(TemplateParser.DefaultCapacity);

            Int32 length = source.Length;
            Int32 offset = -1;
            Int32 origin = index;
            Boolean invalid = false;

            Char token = source[index];

            // Could never be hit...
            if (token != TokenLiterals.OpenedToken)
            {
                return TemplateParser.ParseTextMarker(origin, ref after, source);
            }

            while (true)
            {
                if (token == TokenLiterals.OpenedToken)
                {
                    offset = index;
                }
                else if (token == TokenLiterals.ClosedToken)
                {
                    index++;
                    buffer.Append(token);
                    break;
                }

                buffer.Append(token);

                index++;

                if (index >= length)
                {
                    invalid = true;
                    break;
                }

                token = source[index];
            }

            after = index;

            if (invalid)
            {
                return new TextToken(origin, buffer);
            }
            else if (!TemplateParser.IsValidHoleMarker(buffer))
            {
                return new TextToken(offset, buffer);
            }
            else
            {
                return new HoleToken(offset, rating++, buffer);
            }
        }

        /// <summary>
        /// Checks the validity of provided hole <paramref name="marker"/>.
        /// </summary>
        /// <remarks>
        /// According to template formatting rules, a hole marker can only consist 
        /// of a few of valid characters.Task of this method is to ensure these allowed 
        /// characters depending on their position within the marker.
        /// </remarks>
        /// <param name="marker">
        /// The marker whose content has to be validated.
        /// </param>
        /// <returns>
        /// True if the marker only contains valid format instructions and false otherwise. 
        /// A hole token is turned into a text token in case of this method returns false.
        /// </returns>
        private static Boolean IsValidHoleMarker(StringBuilder marker)
        {
            const Int32 SymbolStage = 0;
            const Int32 LiningStage = 1;
            const Int32 FormatStage = 2;

            Boolean valid = false;
            Int32 index = 0;
            Int32 count = marker.Length;
            Int32 stage = SymbolStage;

            // SEE: https://messagetemplates.org
            // According to the rules for message templates each hole is surrounded by curly braces 
            // and contains a name or an index. Optionally, the alignment is separated by comma and 
            // the format is separated colon. Additionally, keep in mind white spaces are not allowed 
            // in declaration and alignment parts. The format part instead may consist of any character.
            // Definition: '{' ( '@' | '$' )? ( Name | Index ) ( ',' Alignment )? ( ':' Format )? '}'

            // Applies to "empty" as well as to "{}" (almost empty).
            if (count < 3)
            {
                return false;
            }

            // Verify that the marker is surrounded by "{...}". But could never be hit...
            if (marker[index] != TokenLiterals.OpenedToken || marker[count - 1] != TokenLiterals.ClosedToken)
            {
                return false;
            }

            // Skip prefixed and suffixed curly brace.
            index++;
            count--;

            for (; index < count; index++)
            {
                Char token = marker[index];

                // Any additional curly brace causes to fail.
                if (token == TokenLiterals.OpenedToken || token == TokenLiterals.ClosedToken)
                {
                    return false;
                }

                if (stage == SymbolStage && token == TokenLiterals.LiningToken)
                {
                    // Anything behind the very first lining token has to 
                    // be considered as lining content. But note that the 
                    // lining can only start behind the symbol but it can 
                    // be followed by an additional format.
                    stage = LiningStage;

                    continue;
                }
                else if (stage != FormatStage && token == TokenLiterals.FormatToken)
                {
                    // Anything behind the very first format token has to 
                    // be considered as format content. But note that the 
                    // format can either start behind the symbol or behind 
                    // the lining.
                    stage = FormatStage;

                    continue;
                }

                switch (stage)
                {
                    case SymbolStage:

                        // The declaration part can either be a name or an index. In case of name, 
                        // that part can contain letters, digits as well as underscores, and it can 
                        // be prefixed either by @ or by $. In case of index, that part can only 
                        // consist of digits.
                        // Definition (Name):  [0-9A-z_]+
                        // Definition (Index): [0-9]+

                        if (index > 1 && (token == TokenLiterals.SpreadToken || token == TokenLiterals.StringToken))
                        {
                            return false;
                        }

                        if (token != TokenLiterals.SpreadToken && token != TokenLiterals.StringToken &&
                            token != TokenLiterals.HollowToken && !Char.IsLetterOrDigit(token))
                        {
                            return false;
                        }

                        // The declaration part has been hit at least once.
                        valid = true;

                        continue;

                    case LiningStage:

                        // The alignment part can only consist of digits and one optional leading hyphen. 
                        // Definition: '-'? [0-9]+

                        if (token == TokenLiterals.HyphenToken && marker[index - 1] != TokenLiterals.LiningToken)
                        {
                            return false;
                        }

                        if (token != TokenLiterals.HyphenToken && !Char.IsDigit(token))
                        {
                            return false;
                        }

                        continue;

                    case FormatStage:

                        // The format part can contain any character except '{'. But that would never 
                        // fit right here because of all open brackets are already handled above. 
                        // Definition: [^\{]+

                        continue;

                    default:

                        // Can never happen as long as no additional stage has to be handled.
                        throw new IndexOutOfRangeException($"A staging value of {stage} is not applicable in that context.");
                }
            }

            return valid;
        }

        #endregion
    }
}
