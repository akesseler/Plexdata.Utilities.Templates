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
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Collections.Generic;

namespace Plexdata.Utilities.Formatting.Helpers
{
    /// <summary>
    /// This class creates all Label-Value relation assignments.
    /// </summary>
    /// <remarks>
    /// It is the task of this class to creates all assignments 
    /// consisting of a Label-Value relation.
    /// </remarks>
    internal static class RelationsAssigner
    {
        #region Public Fields

        /// <summary>
        /// This field holds the default instance of an 
        /// <see cref="IArgumentRelations"/> derived class.
        /// </summary>
        /// <remarks>
        /// This default instance is returned if for example 
        /// nothing can be processed.
        /// </remarks>
        public static IArgumentRelations Default = new ArgumentRelations();

        #endregion

        #region Construction

        /// <summary>
        /// Default static construction.
        /// </summary>
        /// <remarks>
        /// This constructor does nothing else but to initialize all 
        /// static members.
        /// </remarks>
        static RelationsAssigner() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns the token labels to their corresponding arguments.
        /// </summary>
        /// <remarks>
        /// This method assigns the token labels to their corresponding arguments.
        /// </remarks>
        /// <param name="tokens">
        /// The list of tokens to be processed.
        /// </param>
        /// <param name="arguments">
        /// The list of arguments to be assigned.
        /// </param>
        /// <returns>
        /// An instance of an <see cref="IArgumentRelations"/> derived class to 
        /// obtains all assignments from.
        /// </returns>
        public static IArgumentRelations Assign(IEnumerable<BaseToken> tokens, Object[] arguments)
        {
            if (tokens == null)
            {
                return RelationsAssigner.Default;
            }

            if (arguments == null)
            {
                arguments = new Object[0];
            }

            const String format = "Value{0}";

            ArgumentRelations result = new ArgumentRelations();

            Boolean indexed = tokens.IsAssignAllByIndex();

            foreach (BaseToken token in tokens)
            {
                if (token is TextToken) { continue; }

                Int32 index = -1;
                String label = null;
                Object value = null;

                if (indexed)
                {
                    // Use index as label if all items are processed "by index".
                    index = token.ToIndex();
                    label = String.Format(format, index);

                    // Skip already assigned relations, if necessary. This may occur 
                    // for example in cases like "text {0} is {1}, but same as {0}".
                    // In such a case the value at index {0} has been handled already.
                    if (result.Contains(label)) { continue; }
                }
                else
                {
                    index = token.Rating;

                    if (token.IsNumbering)
                    {
                        // This is some kind of fallback that can occur together with mixed 
                        // formattings, for example something like "text {0} is {lbl1}, but 
                        // same as {1}". In such a case the "rating" is used as index, but 
                        // current token does not have a valid label. Therefore, take "rating" 
                        // as index and create a label for affected value. Uniqueness does 
                        // not need to be checked because of rating is already unique.
                        label = String.Format(format, index);
                    }
                    else
                    {
                        // Uniqueness in never guaranteed in this case, because of the Message 
                        // Template Rules allow multiple labels of same content. On the other 
                        // hand, all arguments have to be resolved by their order anyway.
                        label = token.ToLabel();
                    }
                }

                if (index >= 0 && index < arguments.Length)
                {
                    value = arguments[index];
                }

                result.Append(label, value);
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether all tokens can be handled by index.
        /// </summary>
        /// <remarks>
        /// This method determines whether all tokens can be handled 
        /// by index or have to be processed by label.
        /// </remarks>
        /// <param name="tokens">
        /// The list of tokens to be validated.
        /// </param>
        /// <returns>
        /// True if all included tokens can be processed by index 
        /// and false if at least one token must be processed by 
        /// left-to-right position.
        /// </returns>
        private static Boolean IsAssignAllByIndex(this IEnumerable<BaseToken> tokens)
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

        #endregion
    }
}
