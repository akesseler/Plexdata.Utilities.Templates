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

namespace Plexdata.Utilities.Formatting.Defines
{
    /// <summary>
    /// Provides all constant characters internally used.
    /// </summary>
    /// <remarks>
    /// This static class provides all constant characters to be 
    /// used internally.
    /// </remarks>
    internal static class TokenLiterals
    {
        /// <summary>
        /// The formatting tag open character.
        /// </summary>
        /// <remarks>
        /// This constant serves as format statement opening indicator.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the open token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{0}", 42);
        /// Template.Format("{SomeName}", 42);
        /// </code>
        /// </example>
        /// <seealso cref="ClosedToken"/>
        public const Char OpenedToken = '{';

        /// <summary>
        /// The formatting tag close character.
        /// </summary>
        /// <remarks>
        /// This constant serves as format statement closing indicator.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the closed token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{0}", 42);
        /// Template.Format("{SomeName}", 42);
        /// </code>
        /// </example>
        /// <seealso cref="OpenedToken"/>
        public const Char ClosedToken = '}';

        /// <summary>
        /// The lining separator character.
        /// </summary>
        /// <remarks>
        /// This constant serves as start tag of the value alignment and 
        /// separates it from the rest of the format statement.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the lining token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{0,10}", 42);
        /// Template.Format("{SomeName,10}", 42);
        /// Template.Format("{0,10:N2}", 42);
        /// Template.Format("{SomeName,10:N2}", 42);
        /// </code>
        /// <para>
        /// Please keep in mind, the optional value alignment must always 
        /// be put in front of the optional value formatting.
        /// </para>
        /// </example>
        /// <seealso cref="FormatToken"/>
        public const Char LiningToken = ',';

        /// <summary>
        /// The format separator character.
        /// </summary>
        /// <remarks>
        /// This constant serves as start tag of the value format  and 
        /// separates it from the rest of the format statement.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the format token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{0:N2}", 42);
        /// Template.Format("{SomeName:N2}", 42);
        /// Template.Format("{0,10:N2}", 42);
        /// Template.Format("{SomeName,10:N2}", 42);
        /// </code>
        /// <para>
        /// Please keep in mind, the optional value format must always be 
        /// put at the end of the whole formatting statement.
        /// </para>
        /// </example>
        /// <seealso cref="LiningToken"/>
        public const Char FormatToken = ':';

        /// <summary>
        /// The stringify prefix character.
        /// </summary>
        /// <remarks>
        /// This constant tells the template parser just to convert the 
        /// belonging value into a string.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the string token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{$SomeName}", 42);
        /// </code>
        /// </example>
        /// <seealso cref="SpreadToken"/>
        public const Char StringToken = '$';

        /// <summary>
        /// The expanding prefix character. 
        /// </summary>
        /// <remarks>
        /// This constant tells the template parser to expand the belonging 
        /// value.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the spread token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{@SomeName}", 42);
        /// </code>
        /// </example>
        /// <seealso cref="StringToken"/>
        public const Char SpreadToken = '@';

        /// <summary>
        /// The hollow character. 
        /// </summary>
        /// <remarks>
        /// This constant is one of allowed value name characters.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the hollow token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{Some_Name_007}", 42);
        /// </code>
        /// </example>
        public const Char HollowToken = '_';

        /// <summary>
        /// The hyphen character. 
        /// </summary>
        /// <remarks>
        /// This constant is used to find out how the alignment has to 
        /// applied; padding is added to the left or to the right.
        /// </remarks>
        /// <example>
        /// <para>
        /// Here an example of how the hyphen token is used.
        /// </para>
        /// <code language="cs">
        /// Template.Format("{0,-10}", 42);
        /// Template.Format("{SomeName,-10}", 42);
        /// </code>
        /// </example>
        /// <seealso cref="LiningToken"/>
        public const Char HyphenToken = '-';

        /// <summary>
        /// The expand character. 
        /// </summary>
        /// <remarks>
        /// This constant is used to apply padding and is used together 
        /// the alignment part of the value format.
        /// </remarks>
        /// <seealso cref="LiningToken"/>
        public const Char ExpandToken = ' ';
    }
}
