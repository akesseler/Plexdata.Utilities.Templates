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

using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Plexdata.Utilities.Formatting.Helpers
{
    /// <summary>
    /// Acts as default respectively as fallback custom type converter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class represents the default or in other words the fallback converter for 
    /// any custom type.
    /// </para>
    /// <para>
    /// Task of this class is the conversion of custom types into their <em>flat</em> 
    /// string representation. Flat means in detail that property parsing is not done 
    /// recursively. See enumeration below to get an impression about additional rules.
    /// </para>
    /// <list type="bullet">
    /// <item><description>
    /// Each top-level object is surrounded by square brackets (<c>[...]</c>).
    /// </description></item>
    /// <item><description>
    /// Each affected property is formatted as combination of property name and property 
    /// value (<c>&lt;label&gt;: &lt;value&gt;</c>).
    /// </description></item>
    /// <item><description>
    /// Each combination of property name and value is separated by a semicolon followed 
    /// by one space (<c>; </c>).
    /// </description></item>
    /// <item><description>
    /// Strings as well as single characters are surrounded by double quotes (<c>"some string"</c>).
    /// </description></item>
    /// <item><description>
    /// Values of properties of type <see cref="Char"/> that represent a control character 
    /// are converted into their hexadecimal unicode representation (<c>\u0000</c>).
    /// </description></item>
    /// <item><description>
    /// Objects with a value of <c>null</c> are converted into the string <em>null</em>.
    /// </description></item>
    /// <item><description>
    /// Objects of type <c>class</c> are serialized by their own method <see cref="Object.ToString()"/>. 
    /// Additionally, such classes are put into surrounding square brackets (<c>[...]</c>).
    /// </description></item>
    /// </list>
    /// </remarks>
    /// <seealso cref="IArgumentSerializer"/>
    internal class DefaultSerializer : IArgumentSerializer
    {
        #region Construction

        /// <summary>
        /// Default construction.
        /// </summary>
        /// <remarks>
        /// This constructor actually does nothing.
        /// </remarks>
        public DefaultSerializer() : base() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The depth of recursions for serialization.
        /// </summary>
        /// <remarks>
        /// The depth of recursions is never used by this class.
        /// </remarks>
        /// <value>
        /// Always <c>0</c> (zero).
        /// </value>
        public Int32 Recursions { get; } = 0;

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public String Serialize(IFormatProvider provider, String format, String lining, Object argument)
        {
            if (argument == null)
            {
                return String.Empty;
            }

            StringBuilder result = new StringBuilder(512);

            result.Append("[");

            // Be aware, it may cause a crash right here in case of argument is a system type.
            // But it should not happen here because of method "ToSpreadingValue()" of class 
            // "TemplateWeaver" does a filtering for those system types.
            foreach (PropertyInfo current in this.GetProperties(argument.GetType()))
            {
                this.Render(result, provider, current.Name, current.GetValue(argument));
            }

            if (result.Length > 1)
            {
                result.Length -= 2;
            }

            result.Append("]");

            return result.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Renders the <paramref name="value"/> into <paramref name="result"/>.
        /// </summary>
        /// <remarks>
        /// This method converts the <paramref name="value"/> into its string 
        /// representation and puts it into the <paramref name="result"/>.
        /// </remarks>
        /// <param name="result">
        /// A string-builder instance that records the formatted value.
        /// </param>
        /// <param name="provider">
        /// A format provider instance that helps formatting the value.
        /// </param>
        /// <param name="label">
        /// The label (property name) of current value.
        /// </param>
        /// <param name="value">
        /// The value to be formatted.
        /// </param>
        private void Render(StringBuilder result, IFormatProvider provider, String label, Object value)
        {
            Boolean caption = (value is String) || (value is Char);
            Boolean complex = !(value is String) && (value?.GetType()?.IsClass ?? false);

            String prefix = String.Empty;
            String suffix = String.Empty;

            if (caption)
            {
                prefix = "\"";
                suffix = "\"";
            }
            else if (complex)
            {
                prefix = "[";
                suffix = "]";
            }

            if (value is null)
            {
                value = "null";
            }
            else if (value is Char other && Char.IsControl(other))
            {
                value = String.Format("\\u{0:x4}", (Int32)other);
            }

            // Do not remove the semicolon and the space because 
            // of the caller cuts off the last both characters!
            result.AppendFormat(provider, "{0}: {1}{2}{3}; ", label, prefix, value, suffix);
        }

        /// <summary>
        /// Loads affected properties from provided <paramref name="type"/>>
        /// </summary>
        /// <remarks>
        /// This method loads all affected properties from provided type 
        /// and returns them.
        /// </remarks>
        /// <param name="type">
        /// The type to load all affected properties from.
        /// </param>
        /// <returns>
        /// A list of resolved property details.
        /// </returns>
        /// <seealso cref="IsProcessable(PropertyInfo)"/>
        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            foreach (var current in type.GetRuntimeProperties())
            {
                if (this.IsProcessable(current))
                {
                    yield return current;
                }
            }
        }

        /// <summary>
        /// Validates whether a particular property can be processed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method validates if a particular property can be processed.
        /// </para>
        /// <para>
        /// Such a property must have a public and non-static value getter 
        /// and should be just one of the available scalar types.
        /// </para>
        /// </remarks>
        /// <param name="property">
        /// The property to be evaluated.
        /// </param>
        /// <returns>
        /// True if current property can be processed and false otherwise.
        /// </returns>
        /// <seealso cref="GetProperties(Type)"/>
        private Boolean IsProcessable(PropertyInfo property)
        {
            if (property.CanRead && property.GetMethod.IsPublic && !property.GetMethod.IsStatic)
            {
                if (property.PropertyType == typeof(String))
                {
                    return true;
                }

                if (property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                {
                    return false;
                }

                if (property.PropertyType == typeof(Object))
                {
                    return true;
                }

                if (property.PropertyType.IsClass)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
