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

using System;

namespace Plexdata.Utilities.Formatting.Interfaces
{
    /// <summary>
    /// Serves as output parameter to obtain an additional list of 
    /// Label-Value relations.
    /// </summary>
    /// <remarks>
    /// This interface serves as output parameter or better as result 
    /// parameter to allow users to get an additional list of Label-Value 
    /// relations.
    /// </remarks>
    public interface IArgumentRelations
    {
        #region Public Properties

        /// <summary>
        /// Gets the number of elements contained in derived classes.
        /// </summary>
        /// <remarks>
        /// This property returns the number of elements contained in 
        /// derived classes.
        /// </remarks>
        /// <value>
        /// The number of contained elements.
        /// </value>
        Int32 Count { get; }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <remarks>
        /// This property returns the element at the specified index. 
        /// A value of <c>null</c> is returned either if index is out 
        /// range or if the actual object at found index is <c>null</c>.
        /// </remarks>
        /// <param name="index">
        /// The zero-based index of the element to get.
        /// </param>
        /// <value>
        /// The returned value might be <c>null</c> for two reasons; 
        /// as first, the index is actually out of range and secondly, 
        /// the object at index is really <c>null</c>.
        /// </value>
        /// <returns>
        /// The element at the specified index or <c>null</c>.
        /// </returns>
        Object this[Int32 index] { get; }

        /// <summary>
        /// Gets the element at the specified label.
        /// </summary>
        /// <remarks>
        /// This property returns the element at the specified label. 
        /// A value of <c>null</c> is returned either if the index of 
        /// label is out range or if the actual object at found label 
        /// is <c>null</c>.
        /// </remarks>
        /// <param name="label">
        /// The label of the element to get.
        /// </param>
        /// <value>
        /// The returned value might be <c>null</c> for two reasons; 
        /// as first, the label couldn't be found and secondly, the 
        /// object at label is really <c>null</c>.
        /// </value>
        /// <returns>
        /// The element at the specified label or <c>null</c>.
        /// </returns>
        Object this[String label] { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches for the specified object and returns the zero-based 
        /// index of the first occurrence or <c>-1</c> if not found.
        /// </summary>
        /// <remarks>
        /// This method searches for the specified object and returns the 
        /// zero-based index of the first occurrence within the available 
        /// range of elements or <c>-1</c> if not found.
        /// </remarks>
        /// <param name="label">
        /// The label of the element to get the index for.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence within the available 
        /// range of elements or <c>-1</c> if not found.
        /// </returns>
        /// <seealso cref="Contains(String)"/>
        Int32 IndexOf(String label);

        /// <summary>
        /// Determines whether a relation for <paramref name="label"/> already 
        /// exists.
        /// </summary>
        /// <remarks>
        /// This method detects whether a relation for <paramref name="label"/> 
        /// is already available.
        /// </remarks>
        /// <param name="label">
        /// The label to be verified.
        /// </param>
        /// <returns>
        /// True if a relation for <paramref name="label"/> is contained already 
        /// and false otherwise.
        /// </returns>
        /// <seealso cref="IndexOf(String)"/>
        Boolean Contains(String label);

        /// <summary>
        /// Converts the available elements into an array of tuples.
        /// </summary>
        /// <remarks>
        /// This method converts the available elements into an array of tuples 
        /// and returns it.
        /// </remarks>
        /// <returns>
        /// An array of tuples representing all available elements.
        /// </returns>
        (String Label, Object Value)[] ToArray();

        #endregion
    }
}
