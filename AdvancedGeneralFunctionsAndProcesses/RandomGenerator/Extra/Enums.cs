using System;
using System.Collections.Generic;
using System.Text;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public partial class RandomGenerator
    {
        /// <summary>
        /// Age ranges.
        /// </summary>
        public enum EnumAgeRanges
        {
            /// <summary>
            /// Child range.
            /// </summary>
            Child,

            /// <summary>
            /// Teen range.
            /// </summary>
            Teen,

            /// <summary>
            /// Adult range.
            /// </summary>
            Adult,

            /// <summary>
            /// Senior range.
            /// </summary>
            Senior,

            /// <summary>
            /// All range.
            /// </summary>
            All
        }

        /// <summary>
        /// Casing rules.
        /// </summary>
        internal enum EnumCasingRules
        {
            /// <summary>
            /// Lower case.
            /// </summary>
            LowerCase,

            /// <summary>
            /// Upper case.
            /// </summary>
            UpperCase,

            /// <summary>
            /// Mixed case.
            /// </summary>
            MixedCase
        }

    }
}
