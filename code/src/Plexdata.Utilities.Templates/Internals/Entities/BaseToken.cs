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
    /// The base class of all other token classes.
    /// </summary>
    /// <remarks>
    /// The internal abstract class serves as base 
    /// implementation of all other token classes.
    /// </remarks>
    internal abstract class BaseToken
    {
        #region Private Fields

        /// <summary>
        /// This field holds the offset value.
        /// </summary>
        /// <remarks>
        /// The default value is <c>-1</c>.
        /// </remarks>
        private Int32 offset = -1;

        /// <summary>
        /// This field holds the rating value.
        /// </summary>
        /// <remarks>
        /// The default value is <c>-1</c>.
        /// </remarks>
        private Int32 rating = -1;

        /// <summary>
        /// This field holds the marker value.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="String.Empty"/>.
        /// </remarks>
        private String marker = String.Empty;

        /// <summary>
        /// This field holds the symbol value.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="String.Empty"/>.
        /// </remarks>
        private String symbol = String.Empty;

        /// <summary>
        /// This field holds the lining value.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="String.Empty"/>.
        /// </remarks>
        private String lining = String.Empty;

        /// <summary>
        /// This field holds the format value.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="String.Empty"/>.
        /// </remarks>
        private String format = String.Empty;

        #endregion

        #region Construction

        /// <summary>
        /// Protected parameterized construction.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all properties according to provided 
        /// parameters.
        /// </remarks>
        /// <param name="offset">
        /// The <see cref="Offset"/> within the whole source string.
        /// </param>
        /// <param name="marker">
        /// The actual formatting statement consisting of the <see cref="Symbol"/>, 
        /// an optional <see cref="Lining"/> and/or an optional <see cref="Format"/>.
        /// </param>
        /// <seealso cref="BaseToken(Int32, Int32, StringBuilder)"/>
        protected BaseToken(Int32 offset, StringBuilder marker)
            : this(offset, -1, marker)
        {
        }

        /// <summary>
        /// Protected parameterized construction.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all properties according to provided 
        /// parameters.
        /// </remarks>
        /// <param name="offset">
        /// The <see cref="Offset"/> within the whole source string.
        /// </param>
        /// <param name="rating">
        /// The <see cref="Rating"/> within the whole source string.
        /// </param>
        /// <param name="marker">
        /// The actual formatting statement consisting of the <see cref="Symbol"/>, 
        /// an optional <see cref="Lining"/> and/or an optional <see cref="Format"/>.
        /// </param>
        /// <seealso cref="BaseToken(Int32, StringBuilder)"/>
        /// <seealso cref="BaseToken.Parse(Int32, Int32, StringBuilder)"/>
        protected BaseToken(Int32 offset, Int32 rating, StringBuilder marker)
            : base()
        {
            this.Parse(offset, rating, marker);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the offset within the whole source string.
        /// </summary>
        /// <remarks>
        /// This property allows to get currently assigned offset value.
        /// </remarks>
        /// <value>
        /// The offset within the whole source string.
        /// </value>
        public Int32 Offset
        {
            get
            {
                return this.offset;
            }
            protected set
            {
                if (value < 0)
                {
                    value = -1;
                }

                this.offset = value;
            }
        }

        /// <summary>
        /// Gets the rating within the whole source string.
        /// </summary>
        /// <remarks>
        /// This property allows to get currently assigned rating value. 
        /// This rating represents the zero based index of the corresponding 
        /// argument.
        /// </remarks>
        /// <value>
        /// The zero based index of the corresponding argument.
        /// </value>
        public Int32 Rating
        {
            get
            {
                return this.rating;
            }
            protected set
            {
                if (value < 0)
                {
                    value = -1;
                }

                this.rating = value;
            }
        }

        /// <summary>
        /// Gets the whole formatting instruction.
        /// </summary>
        /// <remarks>
        /// The marker represents the whole formatting instruction.
        /// </remarks>
        /// <value>
        /// The whole formatting instruction.
        /// </value>
        public String Marker
        {
            get
            {
                return this.marker;
            }
            protected set
            {
                if (value == null)
                {
                    value = String.Empty;
                }

                this.marker = value;
            }
        }

        /// <summary>
        /// Gets the symbol (name or index).
        /// </summary>
        /// <remarks>
        /// The symbol actually represents the positioning instruction 
        /// of a format string. It can be either just a number (like used 
        /// for String.Format) or it can be a name (aka template).
        /// </remarks>
        /// <value>
        /// The symbol (name or index).
        /// </value>
        /// <seealso cref="BaseToken.IsNumbering"/>
        /// <seealso cref="BaseToken.IsStringify"/>
        /// <seealso cref="BaseToken.IsSpreading"/>
        /// <seealso cref="BaseToken.ToIndex()"/>
        /// <seealso cref="BaseToken.ToLabel()"/>
        public String Symbol
        {
            get
            {
                return this.symbol;
            }
            protected set
            {
                if (value == null)
                {
                    value = String.Empty;
                }

                this.symbol = value;

                this.ApplySymbolDependencies();
            }
        }

        /// <summary>
        /// Gets how to line up the formatted data. This is also known as 
        /// value alignment.
        /// </summary>
        /// <remarks>
        /// Alignment is left justified as soon as it is prefixed by <c>'-'</c>. 
        /// Otherwise the alignment is right justified.
        /// </remarks>
        /// <value>
        /// The assigned lining value.
        /// </value>
        /// <seealso cref="BaseToken.IsLeftJustified"/>
        /// <seealso cref="BaseToken.IsRightJustified"/>
        public String Lining
        {
            get
            {
                return this.lining;
            }
            protected set
            {
                if (value == null)
                {
                    value = String.Empty;
                }

                this.lining = value;

                this.ApplyLiningDependencies();
            }
        }

        /// <summary>
        /// Gets the additional formatting instructions.
        /// </summary>
        /// <remarks>
        /// This additional formatting instruction is used together with 
        /// an <see cref="IFormatProvider"/> derived class.
        /// </remarks>
        /// <value>
        /// The assigned format value.
        /// </value>
        public String Format
        {
            get
            {
                return this.format;
            }
            protected set
            {
                if (value == null)
                {
                    value = String.Empty;
                }

                this.format = value;
            }
        }

        /// <summary>
        /// Indicates if the symbol can be used as index.
        /// </summary>
        /// <remarks>
        /// A symbol can be used as zero based index as soon as it consists 
        /// only of digits. Otherwise it must be treated as name. But note, 
        /// the prefix <c>'$'</c> or <c>'@'</c> does not affect the numbering 
        /// state.
        /// </remarks>
        /// <value>
        /// True if the symbol can be used as zero based index and false 
        /// otherwise.
        /// </value>
        /// <seealso cref="BaseToken.Symbol"/>
        /// <seealso cref="BaseToken.IsStringify"/>
        /// <seealso cref="BaseToken.IsSpreading"/>
        /// <seealso cref="BaseToken.ToIndex()"/>
        /// <seealso cref="BaseToken.ToLabel()"/>
        public Boolean IsNumbering { get; private set; } = false;

        /// <summary>
        /// Indicates if the value corresponding to the symbol should 
        /// simply be converted into a string.
        /// </summary>
        /// <remarks>
        /// The stringify instruction (prefix <c>'$'</c>) tells the template 
        /// formatter to convert a corresponding value into its string 
        /// representation by using method <see cref="Object.ToString()"/>.
        /// </remarks>
        /// <value>
        /// True if the <see cref="Symbol"/> is prefixed by <c>'$'</c> and false 
        /// otherwise.
        /// </value>
        /// <seealso cref="BaseToken.Symbol"/>
        /// <seealso cref="BaseToken.IsNumbering"/>
        /// <seealso cref="BaseToken.IsSpreading"/>
        /// <seealso cref="BaseToken.ToIndex()"/>
        /// <seealso cref="BaseToken.ToLabel()"/>
        /// <seealso cref="TokenLiterals.StringToken"/>
        public Boolean IsStringify { get; private set; } = false;

        /// <summary>
        /// Indicates if the value corresponding to the symbol should be 
        /// deserialized instead of simply stringifying it.
        /// </summary>
        /// <remarks>
        /// The spreading instruction (prefix <c>'@'</c>) tells the template formatter 
        /// to expand a corresponding value by using an appropriated serialization 
        /// method. Against this background, the resulting string representation 
        /// strictly depends on how the serializer converts an object.
        /// </remarks>
        /// <value>
        /// True if the <see cref="Symbol"/> is prefixed by <c>'@'</c> and false 
        /// otherwise.
        /// </value>
        /// <seealso cref="BaseToken.Symbol"/>
        /// <seealso cref="BaseToken.IsNumbering"/>
        /// <seealso cref="BaseToken.IsStringify"/>
        /// <seealso cref="BaseToken.ToIndex()"/>
        /// <seealso cref="BaseToken.ToLabel()"/>
        /// <seealso cref="TokenLiterals.SpreadToken"/>
        public Boolean IsSpreading { get; private set; } = false;

        /// <summary>
        /// Indicates if the value alignment is left justified. In such a case 
        /// the padding should be put behind the value.
        /// </summary>
        /// <remarks>
        /// This property is nothing else but a convenient accessor to determine 
        /// the alignment of an associated value.
        /// </remarks>
        /// <value>
        /// True is returned when the <see cref="BaseToken.Lining"/> is not empty 
        /// and it starts with a hyphen. Otherwise false is returned.
        /// </value>
        /// <seealso cref="BaseToken.Lining"/>
        /// <seealso cref="TokenLiterals.HyphenToken"/>
        public Boolean IsLeftJustified { get; private set; } = false;

        /// <summary>
        /// Indicates if the value alignment is right justified. In such a case 
        /// the padding should be put in front of the value.
        /// </summary>
        /// <remarks>
        /// This property is nothing else but a convenient accessor to determine 
        /// the alignment of an associated value.
        /// </remarks>
        /// <value>
        /// True is returned when the <see cref="BaseToken.Lining"/> is not empty 
        /// and it does not start with a hyphen. Otherwise false is returned.
        /// </value>
        /// <seealso cref="BaseToken.Lining"/>
        /// <seealso cref="TokenLiterals.HyphenToken"/>
        public Boolean IsRightJustified { get; private set; } = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the symbol into an index, if possible, and returns it.
        /// </summary>
        /// <remarks>
        /// The conversion may fail, for example, in case of the symbol 
        /// represents a label.
        /// </remarks>
        /// <returns>
        /// The zero based index of the corresponding value, or <c>-1</c> if 
        /// a conversion was impossible.
        /// </returns>
        /// <seealso cref="BaseToken.Symbol"/>
        /// <seealso cref="BaseToken.IsNumbering"/>
        /// <seealso cref="BaseToken.IsStringify"/>
        /// <seealso cref="BaseToken.IsSpreading"/>
        /// <seealso cref="BaseToken.ToLabel()"/>
        public abstract Int32 ToIndex();

        /// <summary>
        /// Converts the symbol into a label, if possible, and returns it.
        /// </summary>
        /// <remarks>
        /// The conversion may fail, for example, in case of the symbol 
        /// represents an index.
        /// </remarks>
        /// <returns>
        /// The label to be used for the corresponding value (but without the 
        /// stringify tag as well as without the spreading tag), or <c>empty</c> 
        /// if a conversion was impossible.
        /// </returns>
        /// <seealso cref="BaseToken.Symbol"/>
        /// <seealso cref="BaseToken.IsNumbering"/>
        /// <seealso cref="BaseToken.IsStringify"/>
        /// <seealso cref="BaseToken.IsSpreading"/>
        /// <seealso cref="BaseToken.ToIndex()"/>
        public abstract String ToLabel();

        /// <inheritdoc/>
        /// <remarks>
        /// The method returns a string containing the names and values of some 
        /// of its properties.
        /// </remarks>
        public override String ToString()
        {
            return String.Format(
                "Type: {0}, Offset: [{1}], Rating: [{2}], Symbol: [{3}], Lining: [{4}], Format: [{5}], Marker: [{6}]",
                this.GetType().Name, this.Offset, this.Rating, this.Symbol, this.Lining, this.Format, this.Marker);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Parses the provided marker and applies all properties accordingly.
        /// </summary>
        /// <remarks>
        /// This method parses the provided marker and applies all properties accordingly.
        /// </remarks>
        /// <param name="offset">
        /// The <see cref="Offset"/> within the whole source string.
        /// </param>
        /// <param name="rating">
        /// The <see cref="Rating"/> within the whole source string.
        /// </param>
        /// <param name="marker">
        /// The actual formatting statement consisting of the <see cref="Symbol"/>, 
        /// an optional <see cref="Lining"/> and/or an optional <see cref="Format"/>.
        /// </param>
        /// <seealso cref="BaseToken(Int32, StringBuilder)"/>
        /// <seealso cref="BaseToken(Int32, Int32, StringBuilder)"/>
        protected abstract void Parse(Int32 offset, Int32 rating, StringBuilder marker);

        #endregion

        #region Private Methods

        /// <summary>
        /// Applies the properties that depend on property <see cref="Symbol"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The properties that depend on property <see cref="Symbol"/> are 
        /// <see cref="IsNumbering"/>, <see cref="IsStringify"/> and 
        /// <see cref="IsSpreading"/>.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// Property <see cref="IsNumbering"/> becomes true as soon as the 
        /// <see cref="Symbol"/> can be successfully turned into an integer, 
        /// no matter if it starts with <c>'$'</c> or with <c>'@'</c>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="IsStringify"/> becomes true as soon as the
        /// <see cref="Symbol"/> starts with <c>'$'</c>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="IsSpreading"/> becomes true as soon as the
        /// <see cref="Symbol"/> starts with <c>'@'</c>.
        /// </description></item>
        /// </list>
        /// <para>
        /// This means that <see cref="IsStringify"/> and <see cref="IsSpreading"/> 
        /// are mutually exclusive.
        /// </para>
        /// </remarks>
        private void ApplySymbolDependencies()
        {
            String value = this.Symbol;

            if (value.Length > 0 && (value[0] == TokenLiterals.StringToken || value[0] == TokenLiterals.SpreadToken))
            {
                this.IsNumbering = Int32.TryParse(value.Substring(1), out _);
                this.IsStringify = value[0] == TokenLiterals.StringToken;
                this.IsSpreading = value[0] == TokenLiterals.SpreadToken;
            }
            else
            {
                this.IsNumbering = Int32.TryParse(value, out _);
                this.IsStringify = false;
                this.IsSpreading = false;
            }
        }

        /// <summary>
        /// Applies the properties that depend on property <see cref="Lining"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The properties that depend on property <see cref="Lining"/> are 
        /// <see cref="IsLeftJustified"/> and <see cref="IsRightJustified"/>.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// Property <see cref="IsLeftJustified"/> bcomes true as soon as property 
        /// <see cref="Lining"/> is valid and it starts with <c>'-'</c>.
        /// </description></item>
        /// <item><description>
        /// Property <see cref="IsRightJustified"/> bcomes true as soon as property 
        /// <see cref="Lining"/> is valid and it does not start with <c>'-'</c>.
        /// </description></item>
        /// </list>
        /// </remarks>
        private void ApplyLiningDependencies()
        {
            String value = this.Lining;

            if (value.Length < 1)
            {
                this.IsLeftJustified = false;
                this.IsRightJustified = false;
            }
            else
            {
                this.IsLeftJustified = value[0] == TokenLiterals.HyphenToken;
                this.IsRightJustified = value[0] != TokenLiterals.HyphenToken;
            }
        }

        #endregion
    }
}
