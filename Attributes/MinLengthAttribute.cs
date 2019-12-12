using System;
namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinLengthAttribute : ValidationAttribute
    {
        public MinLengthAttribute(int length)
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
        ///     <c>true</c> if the value is null or greater than or equal to the specified minimum length, otherwise
        ///     <c>false</c>
        /// </returns>
        /// <exception cref="InvalidOperationException">Length is less than zero.</exception>
        public override bool IsValid(object value)
        {
            // Check the lengths for legality
            EnsureLegalLengths();
            int length;
            // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
            if (value == null)
            {
                return true;
            }
            if (value is string str)
            {
                length = str.Length;
            }
            else if (CountPropertyHelper.TryGetCount(value, out var count))
            {
                length = count;
            }
            else
            {
                throw new InvalidCastException();
            }
            return length >= Length;
        }
        /// <summary>
        ///     Checks that Length has a legal value.
        /// </summary>
        /// <exception cref="InvalidOperationException">Length is less than zero.</exception>
        private void EnsureLegalLengths()
        {
            if (Length < 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}