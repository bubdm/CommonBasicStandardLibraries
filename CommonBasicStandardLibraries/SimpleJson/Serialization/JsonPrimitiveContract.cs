﻿using System;
using System.Collections.Generic;
using SimpleJson.Utilities;

namespace SimpleJson.Serialization
{
    /// <summary>
    /// Contract details for a <see cref="Type"/> used by the <see cref="JsonSerializer"/>.
    /// </summary>
    internal class JsonPrimitiveContract : JsonContract
    {
        internal PrimitiveTypeCode TypeCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPrimitiveContract"/> class.
        /// </summary>
        /// <param name="underlyingType">The underlying type for the contract.</param>
        public JsonPrimitiveContract(Type underlyingType)
            : base(underlyingType)
        {
            ContractType = JsonContractType.Primitive;

            TypeCode = ConvertUtils.GetTypeCode(underlyingType);
            IsReadOnlyOrFixedSize = true;

            if (ReadTypeMap.TryGetValue(NonNullableUnderlyingType, out ReadType readType))
            {
                InternalReadType = readType;
            }
        }

        private static readonly Dictionary<Type, ReadType> ReadTypeMap = new Dictionary<Type, ReadType>
        {
            [typeof(byte[])] = ReadType.ReadAsBytes,
            [typeof(byte)] = ReadType.ReadAsInt32,
            [typeof(short)] = ReadType.ReadAsInt32,
            [typeof(int)] = ReadType.ReadAsInt32,
            [typeof(decimal)] = ReadType.ReadAsDecimal,
            [typeof(bool)] = ReadType.ReadAsBoolean,
            [typeof(string)] = ReadType.ReadAsString,
            [typeof(DateTime)] = ReadType.ReadAsDateTime,
#if HAVE_DATE_TIME_OFFSET
            [typeof(DateTimeOffset)] = ReadType.ReadAsDateTimeOffset,
#endif
            [typeof(float)] = ReadType.ReadAsDouble,
            [typeof(double)] = ReadType.ReadAsDouble,
            [typeof(long)] = ReadType.ReadAsInt64
        };
    }
}