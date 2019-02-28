﻿using System;


namespace SimpleJson.Serialization
{
    /// <summary>
    /// Contract details for a <see cref="Type"/> used by the <see cref="JsonSerializer"/>.
    /// </summary>
    internal class JsonStringContract : JsonPrimitiveContract
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringContract"/> class.
        /// </summary>
        /// <param name="underlyingType">The underlying type for the contract.</param>
        public JsonStringContract(Type underlyingType)
            : base(underlyingType)
        {
            ContractType = JsonContractType.String;
        }
    }
}