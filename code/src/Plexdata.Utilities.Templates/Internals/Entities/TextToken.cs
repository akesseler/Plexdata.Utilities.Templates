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
using System.Text;

namespace Plexdata.Utilities.Formatting.Entities
{
    /// <summary>
    /// Represent any kind of plain (unformattable) text.
    /// </summary>
    /// <remarks>
    /// This class represent any kind of plain (unformattable) 
    /// text including any escaped tokens.
    /// </remarks>
    internal class TextToken : BaseToken
    {
        #region Construction

        /// <inheritdoc/>
        /// <summary>
        /// Public parameterized construction.
        /// </summary>
        public TextToken(Int32 offset, StringBuilder marker)
            : base(offset, marker)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        /// <returns>
        /// Returns always <c>-1</c>.
        /// </returns>
        public override Int32 ToIndex()
        {
            return -1;
        }

        /// <inheritdoc/>
        /// <returns>
        /// Returns always <see cref="BaseToken.Marker"/>
        /// </returns>
        public override String ToLabel()
        {
            return base.Marker;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// This method does the initialization as shown below.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// Property <see cref="BaseToken.Offset"/> is set to parameter <paramref name="offset"/>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Rating"/> is set to <c>-1</c>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Marker"/> is set to parameter <paramref name="marker"/>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Symbol"/> is set to <see cref="String.Empty"/>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Lining"/> is set to <see cref="String.Empty"/>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Format"/> is set to <see cref="String.Empty"/>.
        /// </description></item>
        /// </list>
        /// </remarks>
        protected override void Parse(Int32 offset, Int32 rating, StringBuilder marker)
        {
            if (offset < 0 || marker == null || marker.Length < 1)
            {
                return;
            }

            base.Offset = offset;
            base.Rating = -1;
            base.Marker = marker.ToString();
            base.Symbol = String.Empty;
            base.Lining = String.Empty;
            base.Format = String.Empty;
        }

        #endregion
    }

}
