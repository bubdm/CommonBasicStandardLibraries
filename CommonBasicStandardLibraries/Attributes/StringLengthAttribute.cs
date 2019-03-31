using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringLengthAttribute : ValidationAttribute
    {
        public StringLengthAttribute(int MaximumLength)
        {
            this.MaximumLength = MaximumLength;
        }
        /// <summary>
        ///     Gets the maximum acceptable length of the string
        /// </summary>
        public int MaximumLength { get; }

        /// <summary>
        ///     Gets or sets the minimum acceptable length of the string
        /// </summary>
        public int MinimumLength { get; set; }

        public override bool IsValid(object CurrentValue)
        {
            EnsureLegalLengths();
            if (CurrentValue == null)
            {
                return true;
            }

            int length = ((string)CurrentValue).Length;
            return length >= MinimumLength && length <= MaximumLength;
        }

        /// <summary>
        ///     Checks that MinimumLength and MaximumLength have legal values.  Throws InvalidOperationException if not.
        /// </summary>
        private void EnsureLegalLengths()
        {
            if (MaximumLength < 0)
            {
                throw new InvalidOperationException();
            }

            if (MaximumLength < MinimumLength)
            {
                throw new InvalidOperationException();
            }
        }


    }
}
