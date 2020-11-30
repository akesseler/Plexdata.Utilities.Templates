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
    /// An interface to perform custom type serialization.
    /// </summary>
    /// <remarks>
    /// This interface might be implemented by users who want to perform 
    /// a serialization of their custom types.
    /// </remarks>
    public interface IArgumentSerializer
    {
        /// <summary>
        /// The depth of recursions for serialization.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A depth of recursions of <c>0</c> (zero) means for complex 
        /// data types that only the first level of properties will be 
        /// serialized. 
        /// </para>
        /// <para>
        /// Any kind of depth of recursions does not make sense for scalar 
        /// data types and should be ignored in such a case.
        /// </para>
        /// <para>
        /// Any value less than <c>0</c> (zero) may result in an empty 
        /// string or may mean <em>unlimited</em>.
        /// </para>
        /// <para>
        /// Users who implement this interface themselves may interpret 
        /// the value of this property as they wish.
        /// </para>
        /// </remarks>
        /// <value>
        /// The serialization depth of recursions.
        /// </value>
        Int32 Recursions { get; }

        /// <summary>
        /// Converts the <paramref name="argument"/> into a string.
        /// </summary>
        /// <remarks>
        /// This method tries to convert the provided <paramref name="argument"/> 
        /// into a string.
        /// </remarks>
        /// <param name="provider">
        /// An instance of <see cref="IFormatProvider"/> to be used to format 
        /// any of the serialized properties.
        /// This parameter might be <c>null</c>.
        /// </param>
        /// <param name="format">
        /// The optional format string. This parameter represents the format 
        /// specification that comes from the original input format.
        /// </param>
        /// <param name="lining">
        /// The optional lining string. This parameter represents the alignment 
        /// specification that comes from the original input format.
        /// </param>
        /// <param name="argument"></param>
        /// <returns>
        /// A string representing the text version of provided argument.
        /// </returns>
        String Serialize(IFormatProvider provider, String format, String lining, Object argument);
    }
}
