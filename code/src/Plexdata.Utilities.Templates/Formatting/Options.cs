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

using Plexdata.Utilities.Formatting.Helpers;
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Globalization;
using System.Text;

namespace Plexdata.Utilities.Formatting
{
    /// <summary>
    /// This class represents the options to configure 
    /// the template formatter.
    /// </summary>
    /// <remarks>
    /// This options class allows users to configure 
    /// the template formatter and its behaviour.
    /// </remarks>
    /// <seealso cref="Template.Truncate(Options, StringBuilder)"/>
    /// <seealso cref="Template.Format(Options, String, Object[])"/>
    public class Options
    {
        #region Public Fields

        /// <summary>
        /// Gets the default options instance.
        /// </summary>
        /// <remarks>
        /// The instance of default options is used in each case 
        /// if no other options are available.
        /// </remarks>
        public static readonly Options Default = new Options();

        #endregion

        #region Construction

        /// <summary>
        /// Default construction.
        /// </summary>
        /// <remarks>
        /// This constructor does nothing else but initializing all 
        /// properties with their default values.
        /// </remarks>
        public Options() : base() { }

        /// <summary>
        /// Default static construction.
        /// </summary>
        /// <remarks>
        /// This constructor does nothing else but to initialize all 
        /// static members.
        /// </remarks>
        /// <seealso cref="Options.Default"/>
        static Options() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets and sets the format provider to be used.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The format provider is used to format values appropriately.
        /// </para>
        /// <para>
        /// The default value is <see cref="CultureInfo.InvariantCulture"/>.
        /// </para>
        /// </remarks>
        /// <value>
        /// The used format provider.
        /// </value>
        public IFormatProvider Provider { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// Gets and sets the instance of a serializer used to process custom 
        /// types.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The serializer instance to be used to process custom types might be 
        /// implemented by users to handle custom specific data type conversions. 
        /// </para>
        /// <para>
        /// Please note, the provided custom serializer must be able to process 
        /// any of the possible custom types.
        /// </para>
        /// <para>
        /// The default serializer is used if this property is set to <c>null</c>.
        /// </para>
        /// <para>
        /// The default value is <c>null</c>.
        /// </para>
        /// </remarks>
        /// <value>
        /// An instance of <see cref="IArgumentSerializer"/> or <c>null</c> to enable 
        /// default serialization.
        /// </value>
        /// <seealso cref="IArgumentSerializer"/>
        /// <seealso cref="DefaultSerializer"/>
        public IArgumentSerializer Serializer { get; set; } = null;

        /// <summary>
        /// Indicates if string formatting should be treated as <em>limited</em>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any value of property <see cref="Options.Maximum"/> that 
        /// is greater than zero and less than <see cref="Int32.MaxValue"/> is 
        /// treated as <em>limited</em>.
        /// </para>
        /// </remarks>
        /// <value>
        /// True if formatted string should be truncated up to a length of 
        /// <see cref="Options.Maximum"/> and false if formatted 
        /// strings can be considered as <em>unlimited</em>.
        /// </value>
        /// <seealso cref="Options.Maximum"/>
        public Boolean IsLimited { get => this.Maximum > 0 && this.Maximum < Int32.MaxValue; }

        /// <summary>
        /// Gets and sets the maximum length of the formatted result string.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The maximum length of formatted result strings can be controlled 
        /// by this property. But keep in mind, any value less than one or 
        /// greater than <see cref="Int32.MaxValue"/> minus one will be treated 
        /// as "unlimited".
        /// </para>
        /// <para>
        /// Please pay attention, the value of this property should always be 
        /// greater than three because of otherwise the truncation indicator 
        /// (<c>...</c>) cannot be appended.
        /// </para>
        /// <para>
        /// The default value is <see cref="Int32.MaxValue"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// Below an illustration of what happens if the maximum is set to a 
        /// value within a range of [1...5].
        /// </para>
        /// <code>
        /// for (Int32 maximum = 1; maximum &lt;= 5; maximum++)
        /// {
        ///     Options options = new Options() { Maximum = maximum };
        /// 
        ///     String result = Template.Format(options, "format string", null);
        /// }
        /// 
        /// // Maximum = 1 : Result = "f"
        /// // Maximum = 2 : Result = "fo"
        /// // Maximum = 3 : Result = "for"
        /// // Maximum = 4 : Result = "f..."
        /// // Maximum = 5 : Result = "fo..."
        /// </code>
        /// </example>
        /// <value>
        /// The maximum length of formatted strings.
        /// </value>
        /// <seealso cref="Options.IsLimited"/>
        public Int32 Maximum { get; set; } = Int32.MaxValue;

        /// <summary>
        /// Indicates if fallback value should be used instead of rendering 
        /// the original token descriptor.
        /// </summary>
        /// <remarks>
        /// This is nothing else but a convenient property and depends on 
        /// the value of property <see cref="Options.Fallback"/>.
        /// </remarks>
        /// <value>
        /// True if the value of property <see cref="Options.Fallback"/> 
        /// should be used instead of rendering the original token descriptor 
        /// and false otherwise.
        /// </value>
        /// <seealso cref="Options.Fallback"/>
        public Boolean IsFallback { get => this.Fallback != null; }

        /// <summary>
        /// Gets and sets the rendering fallback value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The rendering fallback value is used in cases when an argument for 
        /// a corresponding hole token can be considered as invalid (e.g. if 
        /// such an argument is <c>null</c>).
        /// </para>
        /// <para>
        /// If the value of this property is <c>null</c> then the original token 
        /// descriptor is rendered. In any other case the value of this property 
        /// is rendered instead.
        /// </para>
        /// <para>
        /// The default value is <c>null</c>.
        /// </para>
        /// </remarks>
        /// <value>
        /// The value to be useed as rendering fallback.
        /// </value>
        /// <seealso cref="Options.IsFallback"/>
        public String Fallback { get; set; } = null;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates a new options instance and applies provided parameters.
        /// </summary>
        /// <remarks>
        /// This internal and static method just creates a new options instance and 
        /// applies given <paramref name="provider"/> and <paramref name="serializer"/>.
        /// </remarks>
        /// <param name="provider">
        /// The format provider to used or <c>null</c>.
        /// </param>
        /// <param name="serializer">
        /// The argument serializer to used or <c>null</c>.
        /// </param>
        /// <returns>
        /// A pre-initialized instance of class <see cref="Options"/>.
        /// </returns>
        internal static Options Create(IFormatProvider provider, IArgumentSerializer serializer)
        {
            return new Options()
            {
                Provider = provider,
                Serializer = serializer
            };
        }

        #endregion
    }
}
