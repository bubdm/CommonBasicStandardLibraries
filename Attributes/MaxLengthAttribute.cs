using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxLengthAttribute : ValidationAttribute
    {
        //private const int MaxAllowableLength = -1;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MaxLengthAttribute" /> class.
        /// </summary>
        /// <param name="length">
        ///     The maximum allowable length of collection/string data.
        ///     Value must be greater than zero.
        /// </param>
        public MaxLengthAttribute(int length)
        {
            Length = length;
        }

        /// <summary>
        ///     Gets the maximum allowable length of the collection/string data.
        /// </summary>
        public int Length { get; }

        /// <summary>
        ///     Determines whether a specified object is valid. (Overrides <see cref="ValidationAttribute.IsValid(object)" />)
        /// </summary>
        /// <remarks>
        ///     This method returns <c>true</c> if the <paramref name="value" /> is null.
        ///     It is assumed the <see cref="RequiredAttribute" /> is used if the value may not be null.
        /// </remarks>
        /// <param name="value">The object to validate.</param>
        /// <returns>
        ///     <c>true</c> if the value is null or less than or equal to the specified maximum length, otherwise <c>false</c>
        /// </returns>
        /// <exception cref="InvalidOperationException">Length is zero or less than negative one.</exception>
        public override bool IsValid(object currentvalue)
        {
            // Check the lengths for legality
            EnsureLegalLengths();

            int length;
            // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
            if (currentvalue == null)
            {
                return true;
            }
            if (currentvalue is string str)
            {
                length = str.Length;
            }
            else if (CountPropertyHelper.TryGetCount(currentvalue, out var count))
            {
                length = count;
            }
            else
            {
                throw new InvalidCastException();
            }

            return -1 == Length || length <= Length;
        }


        /// <summary>
        ///     Checks that Length has a legal value.
        /// </summary>
        /// <exception cref="InvalidOperationException">Length is zero or less than negative one.</exception>
        private void EnsureLegalLengths()
        {
            if (Length == 0 || Length < -1)
            {
                throw new InvalidOperationException();
            }
        }
    }
    internal static class CountPropertyHelper
    {
        public static bool TryGetCount(object value, out int count)
        {
            Debug.Assert(value != null);

            if (value is ICollection collection)
            {
                count = collection.Count;
                return true;
            }

            PropertyInfo property = value.GetType().GetRuntimeProperty("Count");
            if (property != null && property.CanRead && property.PropertyType == typeof(int))
            {
                count = (int)property.GetValue(value);
                return true;
            }

            count = -1;
            return false;
        }
    }
}