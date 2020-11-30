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
using System.Collections;
using System.Collections.Generic;

namespace Plexdata.Utilities.Formatting.Extensions
{
    /// <summary>
    /// Extension methods related to type <see cref="Object"/>.
    /// </summary>
    /// <remarks>
    /// This class contains extension methods that are related to type 
    /// <see cref="Object"/>.
    /// </remarks>
    internal static class ArgumentExtension
    {
        #region Public Methods

        /// <summary>
        /// Tries to expand every sub-array contained in <paramref name="arguments"/>.
        /// </summary>
        /// <remarks>
        /// This method tries to expand every sub-array contained in <paramref name="arguments"/>.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here are some examples of what kind of input will be expanded to <em>results</em>.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// Something like <em>empty</em> is expanded to <c>[]</c>.
        /// </description></item>
        /// <item><description>
        /// Something like <c>null</c> is expanded to <c>[null]</c>.
        /// </description></item>
        /// <item><description>
        /// Something like <c>null, null</c> is expanded to <c>[null, null]</c>.
        /// </description></item>
        /// <item><description>
        /// Something like <c>1, "arg", false</c> is expanded to <c>[1, "arg", false]</c>.
        /// </description></item>
        /// <item><description>
        /// Something like <c>new[] { 1, 2, 3 }</c> is expanded to <c>[1, 2, 3]</c>.
        /// </description></item>
        /// <item><description>
        /// Something like <c>new[] { "arg1", "arg2" }</c> is expanded to <c>["arg1", "arg2"]</c>.
        /// </description></item>
        /// <item><description>
        /// Something like <c>true, new[] { "arg1", "arg2" }</c> is expanded to <c>[true, arg1", "arg2]</c>.
        /// </description></item>
        /// <item><description>
        /// Something like <c>42, new[] { "arg1", "arg2" }, true, new[] { 1, 2 }, "arg3"</c> is expanded 
        /// to <c>[42, "arg1", "arg2", true, 1, 2, "arg3"]</c>.
        /// </description></item>
        /// </list>
        /// </example>
        /// <param name="arguments">
        /// The source array to be expanded.
        /// </param>
        /// <returns>
        /// An expanded array of objects or an empty array if parameter 
        /// <paramref name="arguments"/> does not contain any item.
        /// </returns>
        /// <seealso cref="CanExpand(Object)"/>
        public static Object[] ExpandArguments(this Object[] arguments)
        {
            List<Object> result = new List<Object>();

            if (arguments == null)
            {
                arguments = new Object[] { arguments };
            }

            foreach (Object argument in arguments)
            {
                if (argument.CanExpand())
                {
                    foreach (Object current in argument as IEnumerable)
                    {
                        result.Add(current);
                    }
                }
                else
                {
                    result.Add(argument);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Determines whether an <paramref name="argument"/> is of a 
        /// build-in system type.
        /// </summary>
        /// <remarks>
        /// This Method determines whether an <paramref name="argument"/> 
        /// is of a build-in system type. This determination is done by 
        /// checking if the type's namespaces starts with <c>System</c> or 
        /// if the <em>Scope Name</em> of the type's property <c>Module</c> 
        /// equals <em>CommonLanguageRuntimeLibrary</em>.
        /// </remarks>
        /// <param name="argument">
        /// The object to determine its type for.
        /// </param>
        /// <returns>
        /// True is returned if the <paramref name="argument"/> can be 
        /// considered as a build-in system type. False is returned if 
        /// either the <paramref name="argument"/> is <c>null</c> or it 
        /// is not a build-in system type.
        /// </returns>
        public static Boolean IsSystemType(this Object argument)
        {
            if (argument == null)
            {
                return false;
            }

            Type type = argument.GetType();

            return type.Namespace.StartsWith("System", StringComparison.OrdinalIgnoreCase) ||
                   String.Equals(type.Module.ScopeName, "CommonLanguageRuntimeLibrary", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the <paramref name="argument"/> can be expanded.
        /// </summary>
        /// <remarks>
        /// This method determines whether the <paramref name="argument"/> 
        /// can be expanded. For example (almost) each type derived from 
        /// <see cref="IEnumerable"/> can be expanded.
        /// </remarks>
        /// <param name="argument">
        /// The object to be checked.
        /// </param>
        /// <returns>
        /// True is <paramref name="argument"/> if the argument is derived 
        /// from <see cref="IEnumerable"/>. False is returned if either the 
        /// <paramref name="argument"/> is <c>null</c> or it is an instance 
        /// of class <see cref="String"/>.
        /// </returns>
        private static Boolean CanExpand(this Object argument)
        {
            if (argument == null)
            {
                return false;
            }

            Type type = argument.GetType();

            if (type == typeof(String))
            {
                return false;
            }

            return type.GetInterface(nameof(IEnumerable)) != null;
        }

        #endregion
    }
}
