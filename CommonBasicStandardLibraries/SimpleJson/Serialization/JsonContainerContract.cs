using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleJson.Utilities;
using System.Collections;
using System.Linq;


namespace SimpleJson.Serialization
{
    /// <summary>
    /// Contract details for a <see cref="System.Type"/> used by the <see cref="JsonSerializer"/>.
    /// </summary>
    internal class JsonContainerContract : JsonContract
    {
        private JsonContract _itemContract;
        private JsonContract _finalItemContract;

        // will be null for containers that don't have an item type (e.g. IList) or for complex objects
        internal JsonContract ItemContract
        {
            get => _itemContract;
            set
            {
                _itemContract = value;
                if (_itemContract != null)
                {
                    _finalItemContract = (_itemContract.UnderlyingType.IsSealed()) ? _itemContract : null;
                }
                else
                {
                    _finalItemContract = null;
                }
            }
        }

        // the final (i.e. can't be inherited from like a sealed class or valuetype) item contract
        internal JsonContract FinalItemContract => _finalItemContract;

        /// <summary>
        /// Gets or sets the default collection items <see cref="JsonConverter" />.
        /// </summary>
        /// <value>The converter.</value>
        internal JsonConverter ItemConverter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the collection items preserve object references.
        /// </summary>
        /// <value><c>true</c> if collection items preserve object references; otherwise, <c>false</c>.</value>
        public bool? ItemIsReference { get; set; }

        /// <summary>
        /// Gets or sets the collection item reference loop handling.
        /// </summary>
        /// <value>The reference loop handling.</value>
        internal ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

        /// <summary>
        /// Gets or sets the collection item type name handling.
        /// </summary>
        /// <value>The type name handling.</value>
        internal TypeNameHandling? ItemTypeNameHandling { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContainerContract"/> class.
        /// </summary>
        /// <param name="underlyingType">The underlying type for the contract.</param>
        internal JsonContainerContract(Type underlyingType)
            : base(underlyingType)
        {
            JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);

            if (jsonContainerAttribute != null)
            {
                if (jsonContainerAttribute.ItemConverterType != null)
                {
                    ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(
                        jsonContainerAttribute.ItemConverterType,
                        jsonContainerAttribute.ItemConverterParameters);
                }

                ItemIsReference = jsonContainerAttribute._itemIsReference;
                ItemReferenceLoopHandling = jsonContainerAttribute._itemReferenceLoopHandling;
                ItemTypeNameHandling = jsonContainerAttribute._itemTypeNameHandling;
            }
        }
    }
}