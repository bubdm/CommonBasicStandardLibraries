﻿using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization.Formatters;
using SimpleJson.Utilities;
using System.Runtime.Serialization;

namespace SimpleJson.Serialization
{
    internal class JsonSerializerProxy : JsonSerializer
    {
        private readonly JsonSerializerInternalReader _serializerReader;
        private readonly JsonSerializerInternalWriter _serializerWriter;
        private readonly JsonSerializer _serializer;

        public override event EventHandler<ErrorEventArgs> Error
        {
            add => _serializer.Error += value;
            remove => _serializer.Error -= value;
        }

        public override IReferenceResolver ReferenceResolver
        {
            get => _serializer.ReferenceResolver;
            set => _serializer.ReferenceResolver = value;
        }

        public override ITraceWriter TraceWriter
        {
            get => _serializer.TraceWriter;
            set => _serializer.TraceWriter = value;
        }

        public override IEqualityComparer EqualityComparer
        {
            get => _serializer.EqualityComparer;
            set => _serializer.EqualityComparer = value;
        }

        internal override JsonConverterCollection Converters => _serializer.Converters;

        internal override DefaultValueHandling DefaultValueHandling
        {
            get => _serializer.DefaultValueHandling;
            set => _serializer.DefaultValueHandling = value;
        }

        internal override IContractResolver ContractResolver
        {
            get => _serializer.ContractResolver;
            set => _serializer.ContractResolver = value;
        }

        internal override MissingMemberHandling MissingMemberHandling
        {
            get => _serializer.MissingMemberHandling;
            set => _serializer.MissingMemberHandling = value;
        }

        internal override NullValueHandling NullValueHandling
        {
            get => _serializer.NullValueHandling;
            set => _serializer.NullValueHandling = value;
        }

        internal override ObjectCreationHandling ObjectCreationHandling
        {
            get => _serializer.ObjectCreationHandling;
            set => _serializer.ObjectCreationHandling = value;
        }

        internal override ReferenceLoopHandling ReferenceLoopHandling
        {
            get => _serializer.ReferenceLoopHandling;
            set => _serializer.ReferenceLoopHandling = value;
        }

        internal override PreserveReferencesHandling PreserveReferencesHandling
        {
            get => _serializer.PreserveReferencesHandling;
            set => _serializer.PreserveReferencesHandling = value;
        }

        internal override TypeNameHandling TypeNameHandling
        {
            get => _serializer.TypeNameHandling;
            set => _serializer.TypeNameHandling = value;
        }

        internal override MetadataPropertyHandling MetadataPropertyHandling
        {
            get => _serializer.MetadataPropertyHandling;
            set => _serializer.MetadataPropertyHandling = value;
        }

        [Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
        public override FormatterAssemblyStyle TypeNameAssemblyFormat
        {
            get => _serializer.TypeNameAssemblyFormat;
            set => _serializer.TypeNameAssemblyFormat = value;
        }

        internal override TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
        {
            get => _serializer.TypeNameAssemblyFormatHandling;
            set => _serializer.TypeNameAssemblyFormatHandling = value;
        }

        internal override ConstructorHandling ConstructorHandling
        {
            get => _serializer.ConstructorHandling;
            set => _serializer.ConstructorHandling = value;
        }

        [Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
        public override SerializationBinder Binder
        {
            get => _serializer.Binder;
            set => _serializer.Binder = value;
        }

        public override ISerializationBinder SerializationBinder
        {
            get => _serializer.SerializationBinder;
            set => _serializer.SerializationBinder = value;
        }

        public override StreamingContext Context
        {
            get => _serializer.Context;
            set => _serializer.Context = value;
        }

        internal override Formatting Formatting
        {
            get => _serializer.Formatting;
            set => _serializer.Formatting = value;
        }

        internal override DateFormatHandling DateFormatHandling
        {
            get => _serializer.DateFormatHandling;
            set => _serializer.DateFormatHandling = value;
        }

        internal override DateTimeZoneHandling DateTimeZoneHandling
        {
            get => _serializer.DateTimeZoneHandling;
            set => _serializer.DateTimeZoneHandling = value;
        }

        internal override DateParseHandling DateParseHandling
        {
            get => _serializer.DateParseHandling;
            set => _serializer.DateParseHandling = value;
        }

        internal override FloatFormatHandling FloatFormatHandling
        {
            get => _serializer.FloatFormatHandling;
            set => _serializer.FloatFormatHandling = value;
        }

        internal override FloatParseHandling FloatParseHandling
        {
            get => _serializer.FloatParseHandling;
            set => _serializer.FloatParseHandling = value;
        }

        internal override StringEscapeHandling StringEscapeHandling
        {
            get => _serializer.StringEscapeHandling;
            set => _serializer.StringEscapeHandling = value;
        }

        public override string DateFormatString
        {
            get => _serializer.DateFormatString;
            set => _serializer.DateFormatString = value;
        }

        public override CultureInfo Culture
        {
            get => _serializer.Culture;
            set => _serializer.Culture = value;
        }

        public override int? MaxDepth
        {
            get => _serializer.MaxDepth;
            set => _serializer.MaxDepth = value;
        }

        public override bool CheckAdditionalContent
        {
            get => _serializer.CheckAdditionalContent;
            set => _serializer.CheckAdditionalContent = value;
        }

        internal JsonSerializerInternalBase GetInternalSerializer()
        {
            if (_serializerReader != null)
            {
                return _serializerReader;
            }
            else
            {
                return _serializerWriter;
            }
        }

        public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
        {
            ValidationUtils.ArgumentNotNull(serializerReader, nameof(serializerReader));

            _serializerReader = serializerReader;
            _serializer = serializerReader.Serializer;
        }

        public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
        {
            ValidationUtils.ArgumentNotNull(serializerWriter, nameof(serializerWriter));

            _serializerWriter = serializerWriter;
            _serializer = serializerWriter.Serializer;
        }

        internal override object DeserializeInternal(JsonReader reader, Type objectType)
        {
            if (_serializerReader != null)
            {
                return _serializerReader.Deserialize(reader, objectType, false);
            }
            else
            {
                return _serializer.Deserialize(reader, objectType);
            }
        }

        internal override void PopulateInternal(JsonReader reader, object target)
        {
            if (_serializerReader != null)
            {
                _serializerReader.Populate(reader, target);
            }
            else
            {
                _serializer.Populate(reader, target);
            }
        }

        internal override void SerializeInternal(JsonWriter jsonWriter, object value, Type rootType)
        {
            if (_serializerWriter != null)
            {
                _serializerWriter.Serialize(jsonWriter, value, rootType);
            }
            else
            {
                _serializer.Serialize(jsonWriter, value);
            }
        }
    }
}