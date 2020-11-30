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
using System;
using System.Text;

namespace Plexdata.Utilities.Formatting.Entities
{
    /// <summary>
    /// Represent a formattable token.
    /// </summary>
    /// <remarks>
    /// This class represent a formattable token. Such a formattable token 
    /// is characterized by a property name or a property index, as well as 
    /// an additional alignment and/or an additional formatting instruction. 
    /// </remarks>
    internal class HoleToken : BaseToken
    {
        #region Construction

        /// <inheritdoc/>
        /// <summary>
        /// Public parameterized construction.
        /// </summary>
        public HoleToken(Int32 offset, Int32 rating, StringBuilder marker)
            : base(offset, rating, marker)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        /// <remarks>
        /// This method returns the zero based index referencing the corresponding value 
        /// within the list of arguments. In case of <see cref="BaseToken.IsNumbering"/> 
        /// is true, this index is the integer version of the <see cref="BaseToken.Symbol"/>, 
        /// otherwise this index is the value of <see cref="BaseToken.Rating"/>.
        /// </remarks>
        public override Int32 ToIndex()
        {
            if (base.IsNumbering)
            {
                if (base.IsStringify || base.IsSpreading)
                {
                    return Int32.Parse(base.Symbol.Substring(1));
                }

                return Int32.Parse(base.Symbol);
            }
            else
            {
                return base.Rating;
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// This method returns the label to be used to as argument name. The 
        /// corresponding value within the list of arguments is referenced by 
        /// property <see cref="BaseToken.Rating"/>.
        /// </para>
        /// <para>
        /// Furthermore, this method returns <see cref="String.Empty"/> if 
        /// <see cref="BaseToken.IsNumbering"/> is currently set to true. 
        /// Otherwise, it returns the value of <see cref="BaseToken.Symbol"/> 
        /// but without prefixes, such as <c>'@'</c> and <c>'$'</c>.
        /// </para>
        /// </remarks>
        public override String ToLabel()
        {
            if (!base.IsNumbering)
            {
                if (base.IsStringify || base.IsSpreading)
                {
                    return base.Symbol.Substring(1);
                }

                return base.Symbol;
            }

            return String.Empty;
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
        /// Property <see cref="BaseToken.Rating"/> is set to parameter <paramref name="rating"/>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Marker"/> is set to parameter <paramref name="marker"/>. 
        /// Such a marker consists in that order of a <c>symbol</c>, an optional 
        /// <c>lining</c> and/or an optional <c>format</c>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Symbol"/> is set to the <c>symbol</c> part of parameter 
        /// <paramref name="marker"/>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Lining"/> is set to the <c>lining</c> part of parameter 
        /// <paramref name="marker"/>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="BaseToken.Format"/> is set to the <c>format</c> part of parameter 
        /// <paramref name="marker"/>.
        /// </description></item>
        /// </list>
        /// </remarks>
        protected override void Parse(Int32 offset, Int32 rating, StringBuilder marker)
        {
            if (offset < 0 || marker == null || marker.Length < 1)
            {
                return;
            }

            const Int32 SymbolLevel = 0;
            const Int32 LiningLevel = 1;
            const Int32 FormatLevel = 2;

            Int32 count = marker.Length;
            Int32 index = 0;
            Int32 level = SymbolLevel;

            StringBuilder symbol = new StringBuilder(count);
            StringBuilder lining = new StringBuilder(count);
            StringBuilder format = new StringBuilder(count);

            do
            {
                Char token = marker[index];

                if (token == TokenLiterals.OpenedToken || token == TokenLiterals.ClosedToken)
                {
                    index++;
                    continue;
                }

                if (level == SymbolLevel && token == TokenLiterals.LiningToken)
                {
                    level = LiningLevel;
                    index++;
                    continue;
                }

                if (level != FormatLevel && token == TokenLiterals.FormatToken)
                {
                    level = FormatLevel;
                    index++;
                    continue;
                }

                switch (level)
                {
                    case LiningLevel:
                        lining.Append(token);
                        break;
                    case FormatLevel:
                        format.Append(token);
                        break;
                    default:
                        symbol.Append(token);
                        break;
                }

                index++;
            }
            while (index < count);

            base.Offset = offset;
            base.Rating = rating;
            base.Marker = marker.ToString();
            base.Symbol = symbol.ToString();
            base.Lining = lining.ToString();
            base.Format = format.ToString();
        }

        #endregion
    }
}
