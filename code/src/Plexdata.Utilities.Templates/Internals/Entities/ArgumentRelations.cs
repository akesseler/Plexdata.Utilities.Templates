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
using System.Collections.Generic;

namespace Plexdata.Utilities.Formatting.Entities
{
    /// <summary>
    /// Represents implementation of interface <see cref="IArgumentRelations"/>.
    /// </summary>
    /// <remarks>
    /// This class represents nothing else but the implementation of interface 
    /// <see cref="IArgumentRelations"/>.
    /// </remarks>
    internal class ArgumentRelations : IArgumentRelations
    {
        #region Private Classes 

        /// <summary>
        /// Private class for internal relation management.
        /// </summary>
        /// <remarks>
        /// This private class is just for internal use and provides 
        /// functionality for relation management.
        /// </remarks>
        private class ArgumentRelation : IEquatable<String>
        {
            #region Construction

            /// <summary>
            /// Class construction.
            /// </summary>
            /// <remarks>
            /// This constructor takes the label and value as they are.
            /// </remarks>
            /// <param name="label">
            /// The label to be assigned.
            /// </param>
            /// <param name="value">
            /// The value to be assigned.
            /// </param>
            public ArgumentRelation(String label, Object value)
                : base()
            {
                this.Label = label;
                this.Value = value;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Get currently assigned label.
            /// </summary>
            /// <remarks>
            /// This property allows to get currently assigned label.
            /// </remarks>
            /// <value>
            /// The label to be used.
            /// </value>
            public String Label { get; private set; }

            /// <summary>
            /// Get currently assigned value.
            /// </summary>
            /// <remarks>
            /// This property allows to get currently assigned value.
            /// </remarks>
            /// <value>
            /// The value to be used.
            /// </value>
            public Object Value { get; private set; }

            #endregion

            #region Public Methods

            /// <summary>
            /// Checks the equality of the label.
            /// </summary>
            /// <remarks>
            /// This method just checks if current label is equal to <paramref name="other"/>.
            /// </remarks>
            /// <param name="other">
            /// The other label to be checked.
            /// </param>
            /// <returns>
            /// True if current label is equal to the other label and false otherwise.
            /// </returns>
            public Boolean Equals(String other)
            {
                return String.Equals(this.Label, other);
            }

            /// <summary>
            /// Turns label and value into a tuple.
            /// </summary>
            /// <remarks>
            /// This method turns the <see cref="ArgumentRelation.Label"/> and the 
            /// <see cref="ArgumentRelation.Value"/> into its  tuple representation.
            /// </remarks>
            /// <returns>
            /// A tuple consisting of <see cref="ArgumentRelation.Label"/> and 
            /// <see cref="ArgumentRelation.Value"/>.
            /// </returns>
            public (String Label, Object Value) ToTuple()
            {
                return (this.Label, this.Value);
            }

            /// <inheritdoc/>
            /// <remarks>
            /// The method returns a string containing the value of property 
            /// <see cref="ArgumentRelation.Label"/> as well as the value of 
            /// property <see cref="ArgumentRelation.Value"/>.
            /// </remarks>
            public override String ToString()
            {
                return $"{nameof(this.Label)}: [{this.Label ?? "null"}], {nameof(this.Value)}: [{this.Value?.ToString() ?? "null"}]";
            }

            #endregion
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// This field holds the list of argument relations.
        /// </summary>
        /// <remarks>
        /// The default value is an empty list.
        /// </remarks>
        private readonly List<ArgumentRelation> relations;

        #endregion

        #region Construction

        /// <summary>
        /// Default construction.
        /// </summary>
        /// <remarks>
        /// This constructor initializes its fields and properties 
        /// with their default values.
        /// </remarks>
        public ArgumentRelations()
            : base()
        {
            this.relations = new List<ArgumentRelation>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of available relations.
        /// </summary>
        /// <remarks>
        /// This property gets the number of currently 
        /// available relations.
        /// </remarks>
        /// <value>
        /// The number of available relations.
        /// </value>
        public Int32 Count
        {
            get
            {
                return this.relations.Count;
            }
        }

        /// <summary>
        /// Gets the object at given index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property gets the object at given index. A value of 
        /// <c>null</c> is returned if the index is out of range.
        /// </para>
        /// <para>
        /// It shouldn't be necessary to validate an index beforehand, 
        /// because of an exception is not thrown if an index is out of 
        /// range. 
        /// </para>
        /// </remarks>
        /// <param name="index">
        /// The index to get an object for.
        /// </param>
        /// <value>
        /// The returned value might be <c>null</c> for two reasons; 
        /// as first, the index is actually out of range and secondly, 
        /// the object at index is really <c>null</c>.
        /// </value>
        /// <returns>
        /// The object at index or <c>null</c>.
        /// </returns>
        /// <seealso cref="ArgumentRelations.this[String]"/>
        public Object this[Int32 index]
        {
            get
            {
                if (index < 0 || index > this.relations.Count - 1)
                {
                    return null;
                }

                return this.relations[index].Value;
            }
        }

        /// <summary>
        /// Gets the object for a specific label.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property gets the object for a specific label. A value 
        /// of <c>null</c> is returned if the label label couldn't be 
        /// found.
        /// </para>
        /// <para>
        /// It shouldn't be necessary to validate a label's existence 
        /// beforehand, because of an exception is not thrown if a label 
        /// couldn't be found. 
        /// range. 
        /// </para>
        /// </remarks>
        /// <param name="label">
        /// The label to get an object for.
        /// </param>
        /// <value>
        /// The returned value might be <c>null</c> for two reasons; 
        /// as first, the label couldn't be found and secondly, the 
        /// object at label is really <c>null</c>.
        /// </value>
        /// <returns>
        /// The object for label or <c>null</c>.
        /// </returns>
        /// <seealso cref="ArgumentRelations.this[Int32]"/>
        /// <seealso cref="ArgumentRelations.IndexOf(String)"/>
        /// <seealso cref="ArgumentRelations.Contains(String)"/>
        public Object this[String label]
        {
            get
            {
                return this[this.IndexOf(label)];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the index for provided label or <c>-1</c> if not found.
        /// </summary>
        /// <remarks>
        /// This method returns the index for provided label or <c>-1</c> 
        /// if not found.
        /// </remarks>
        /// <param name="label">
        /// The label of the element to get the index for.
        /// </param>
        /// <returns>
        /// The index of the first occurrence within the available range 
        /// of elements or <c>-1</c> if not found.
        /// </returns>
        public Int32 IndexOf(String label)
        {
            Int32 count = this.relations.Count;

            for (Int32 index = 0; index < count; index++)
            {
                if (this.relations[index].Equals(label))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds out if an object exists for provided label.
        /// </summary>
        /// <remarks>
        /// This method allows to find out if an object exists for provided 
        /// label.
        /// </remarks>
        /// <param name="label">
        /// The label to check an object existence for.
        /// </param>
        /// <returns>
        /// True is returned if an object could be found and false otherwise.
        /// </returns>
        /// <seealso cref="ArgumentRelations.IndexOf(String)"/>
        public Boolean Contains(String label)
        {
            foreach (ArgumentRelation relation in this.relations)
            {
                if (relation.Equals(label))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an array of tuples of all assigned Label-Value relations.
        /// </summary>
        /// <remarks>
        /// This method creates an array of tuples of all assigned Label-Value 
        /// relations and returns it.
        /// </remarks>
        /// <returns>
        /// An array of tuples of all Label-Value relations.
        /// </returns>
        public (String Label, Object Value)[] ToArray()
        {
            Int32 count = this.relations.Count;

            (String Label, Object Value)[] result = new (String Label, Object Value)[count];

            for (Int32 index = 0; index < count; index++)
            {
                result[index] = this.relations[index].ToTuple();
            }

            return result;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The method returns a string containing the value of 
        /// property <see cref="ArgumentRelations.Count"/>.
        /// </remarks>
        public override String ToString()
        {
            return $"{nameof(this.Count)}: {this.Count}";
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Appends the combination of label and value to the list of assigned 
        /// relations.
        /// </summary>
        /// <remarks>
        /// This method creates a new combination of Label-Value relation and 
        /// appends it to the list of assigned relations.
        /// </remarks>
        /// <param name="label">
        /// The label to be used. Each <paramref name="value"/> is possible, 
        /// because of value validation is not performed.
        /// </param>
        /// <param name="value">
        /// The value to be assigned to current <paramref name="label"/>.
        /// </param>
        internal void Append(String label, Object value)
        {
            this.relations.Add(new ArgumentRelation(label, value));
        }

        #endregion
    }
}
