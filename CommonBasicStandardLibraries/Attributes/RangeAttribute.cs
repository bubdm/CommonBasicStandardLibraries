using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]

    public class RangeAttribute : ValidationAttribute
    {
        public object Minimum { get; private set; }
        public object Maximum { get; private set; }
        /// <summary>
        ///     Gets the type of the <see cref="Minimum" /> and <see cref="Maximum" /> values (e.g. Int32, Double, or some custom
        ///     type)
        /// </summary>
        public Type OperandType { get; }
        public RangeAttribute(Type Type, string Minimum, string Maximum)
        {
            OperandType = Type;
            this.Minimum = Minimum;
            this.Maximum = Maximum;
        }

        public RangeAttribute(int Minimum, int Maximum)
        {
            this.Minimum = Minimum;
            this.Maximum = Maximum;
        }
        public RangeAttribute(double Minimum, double Maximum)
        {
            this.Minimum = Minimum;
            this.Maximum = Maximum;
        }


        /// <summary>
        ///     Returns true if the value falls between min and max, inclusive.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> means the <paramref name="value" /> is valid</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override bool IsValid(object value)
        {
            // Validate our properties and create the conversion function
            SetupConversion();

            // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
            if (value == null || (value as string)?.Length == 0)
            {
                return true;
            }

            object convertedValue;

            try
            {
                convertedValue = Conversion(value);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }

            var min = (IComparable)Minimum;
            var max = (IComparable)Maximum;
            return min.CompareTo(convertedValue) <= 0 && max.CompareTo(convertedValue) >= 0;
        }

        private Func<object, object> Conversion { get; set; }

        /// <summary>
        /// Determines whether string values for <see cref="Minimum"/> and <see cref="Maximum"/> are parsed in the invariant
        /// culture rather than the current culture in effect at the time of the validation.
        /// </summary>
        private bool ParseLimitsInInvariantCulture { get; set; }

        private void Initialize(IComparable minimum, IComparable maximum, Func<object, object> conversion)
        {
            if (minimum.CompareTo(maximum) > 0)
            {
                throw new InvalidOperationException();
            }

            Minimum = minimum;
            Maximum = maximum;
            Conversion = conversion;
        }

        /// <summary>
        ///     Validates the properties of this attribute and sets up the conversion function.
        ///     This method throws exceptions if the attribute is not configured properly.
        ///     If it has once determined it is properly configured, it is a NOP.
        /// </summary>
        private void SetupConversion()
        {

            object minimum = Minimum;
            object maximum = Maximum;

            if (minimum == null || maximum == null)
            {
                throw new InvalidOperationException();
            }

            // Careful here -- OperandType could be int or double if they used the long form of the ctor.
            // But the min and max would still be strings.  Do use the type of the min/max operands to condition
            // the following code.
            Type operandType = minimum.GetType();

            if (operandType == typeof(int))
            {
                Initialize((int)minimum, (int)maximum, v => Convert.ToInt32(v, CultureInfo.InvariantCulture));
            }
            else if (operandType == typeof(double))
            {
                Initialize((double)minimum, (double)maximum,
                    v => Convert.ToDouble(v, CultureInfo.InvariantCulture));
            }
            else
            {
                Type type = OperandType;
                if (type == null)
                {
                    throw new InvalidOperationException();
                }
                Type comparableType = typeof(IComparable);
                if (!comparableType.IsAssignableFrom(type))
                {
                    throw new InvalidOperationException();
                }

                TypeConverter converter = TypeDescriptor.GetConverter(type);
                IComparable min = (IComparable)(ParseLimitsInInvariantCulture
                    ? converter.ConvertFromInvariantString((string)minimum)
                    : converter.ConvertFromString((string)minimum));
                IComparable max = (IComparable)(ParseLimitsInInvariantCulture
                    ? converter.ConvertFromInvariantString((string)maximum)
                    : converter.ConvertFromString((string)maximum));

                object conversion(object value) => value.GetType() == type ? value : converter.ConvertFrom(value);
                Initialize(min, max, conversion);
            }


          
        }
    }
}
