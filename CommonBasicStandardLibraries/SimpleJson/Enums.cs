using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJson
{
    /// <summary>
    /// Specifies reference loop handling options for the <see cref="JsonSerializer"/>.
    /// </summary>
    internal enum ReferenceLoopHandling
    {
        /// <summary>
        /// Throw a <see cref="JsonSerializationException"/> when a loop is encountered.
        /// </summary>
        Error = 0,

        /// <summary>
        /// Ignore loop references and do not serialize.
        /// </summary>
        Ignore = 1,

        /// <summary>
        /// Serialize loop references.
        /// </summary>
        Serialize = 2
    }

    /// <summary>
    /// Specifies missing member handling options for the <see cref="JsonSerializer"/>.
    /// </summary>
    internal enum MissingMemberHandling
    {
        /// <summary>
        /// Ignore a missing member and do not attempt to deserialize it.
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// Throw a <see cref="JsonSerializationException"/> when a missing member is encountered during deserialization.
        /// </summary>
        Error = 1
    }

    internal enum NullValueHandling
    {
        /// <summary>
        /// Include null values when serializing and deserializing objects.
        /// </summary>
        Include = 0,

        /// <summary>
        /// Ignore null values when serializing and deserializing objects.
        /// </summary>
        Ignore = 1
    }

    [Flags]
    internal enum DefaultValueHandling
    {
        /// <summary>
        /// Include members where the member value is the same as the member's default value when serializing objects.
        /// Included members are written to JSON. Has no effect when deserializing.
        /// </summary>
        Include = 0,

        /// <summary>
        /// Ignore members where the member value is the same as the member's default value when serializing objects
        /// so that it is not written to JSON.
        /// This option will ignore all default values (e.g. <c>null</c> for objects and nullable types; <c>0</c> for integers,
        /// decimals and floating point numbers; and <c>false</c> for booleans). The default value ignored can be changed by
        /// placing the <see cref="DefaultValueAttribute"/> on the property.
        /// </summary>
        Ignore = 1,

        /// <summary>
        /// Members with a default value but no JSON will be set to their default value when deserializing.
        /// </summary>
        Populate = 2,

        /// <summary>
        /// Ignore members where the member value is the same as the member's default value when serializing objects
        /// and set members to their default value when deserializing.
        /// </summary>
        IgnoreAndPopulate = Ignore | Populate
    }

    /// <summary>
    /// Specifies how object creation is handled by the <see cref="JsonSerializer"/>.
    /// </summary>
    internal enum ObjectCreationHandling
    {
        /// <summary>
        /// Reuse existing objects, create new objects when needed.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Only reuse existing objects.
        /// </summary>
        Reuse = 1,

        /// <summary>
        /// Always create new objects.
        /// </summary>
        Replace = 2
    }

    [Flags]
    internal enum PreserveReferencesHandling
    {
        /// <summary>
        /// Do not preserve references when serializing types.
        /// </summary>
        None = 0,

        /// <summary>
        /// Preserve references when serializing into a JSON object structure.
        /// </summary>
        Objects = 1,

        /// <summary>
        /// Preserve references when serializing into a JSON array structure.
        /// </summary>
        Arrays = 2,

        /// <summary>
        /// Preserve references when serializing.
        /// </summary>
        All = Objects | Arrays
    }

    /// <summary>
    /// Specifies how constructors are used when initializing objects during deserialization by the <see cref="JsonSerializer"/>.
    /// </summary>
    internal enum ConstructorHandling
    {
        /// <summary>
        /// First attempt to use the public default constructor, then fall back to a single parameterized constructor, then to the non-public default constructor.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Json.NET will use a non-public default constructor before falling back to a parameterized constructor.
        /// </summary>
        AllowNonPublicDefaultConstructor = 1
    }

    [Flags]
    internal enum TypeNameHandling
    {
        /// <summary>
        /// Do not include the .NET type name when serializing types.
        /// </summary>
        None = 0,

        /// <summary>
        /// Include the .NET type name when serializing into a JSON object structure.
        /// </summary>
        Objects = 1,

        /// <summary>
        /// Include the .NET type name when serializing into a JSON array structure.
        /// </summary>
        Arrays = 2,

        /// <summary>
        /// Always include the .NET type name when serializing.
        /// </summary>
        All = Objects | Arrays,

        /// <summary>
        /// Include the .NET type name when the type of the object being serialized is not the same as its declared type.
        /// Note that this doesn't include the root serialized object by default. To include the root object's type name in JSON
        /// you must specify a root type object with <see cref="JsonConvert.SerializeObject(object, Type, JsonSerializerSettings)"/>
        /// or <see cref="JsonSerializer.Serialize(JsonWriter, object, Type)"/>.
        /// </summary>
        Auto = 4
    }

    /// <summary>
    /// Specifies metadata property handling options for the <see cref="JsonSerializer"/>.
    /// </summary>
    internal enum MetadataPropertyHandling
    {
        /// <summary>
        /// Read metadata properties located at the start of a JSON object.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Read metadata properties located anywhere in a JSON object. Note that this setting will impact performance.
        /// </summary>
        ReadAhead = 1,

        /// <summary>
        /// Do not try to read metadata properties.
        /// </summary>
        Ignore = 2
    }

    /// <summary>
    /// Specifies formatting options for the <see cref="JsonTextWriter"/>.
    /// </summary>
    internal enum Formatting
    {
        /// <summary>
        /// No special formatting is applied. This is the default.
        /// </summary>
        None = 0,

        /// <summary>
        /// Causes child objects to be indented according to the <see cref="JsonTextWriter.Indentation"/> and <see cref="JsonTextWriter.IndentChar"/> settings.
        /// </summary>
        Indented = 1
    }

    /// <summary>
    /// Specifies how dates are formatted when writing JSON text.
    /// </summary>
    public enum DateFormatHandling //this was forced to be public
    {
        /// <summary>
        /// Dates are written in the ISO 8601 format, e.g. <c>"2012-03-21T05:40Z"</c>.
        /// </summary>
        IsoDateFormat,

        /// <summary>
        /// Dates are written in the Microsoft JSON format, e.g. <c>"\/Date(1198908717056)\/"</c>.
        /// </summary>
        MicrosoftDateFormat
    }

    /// <summary>
    /// Specifies how to treat the time value when converting between string and <see cref="DateTime"/>.
    /// </summary>
    public enum DateTimeZoneHandling //this was forced to be public
    {
        /// <summary>
        /// Treat as local time. If the <see cref="DateTime"/> object represents a Coordinated Universal Time (UTC), it is converted to the local time.
        /// </summary>
        Local = 0,

        /// <summary>
        /// Treat as a UTC. If the <see cref="DateTime"/> object represents a local time, it is converted to a UTC.
        /// </summary>
        Utc = 1,

        /// <summary>
        /// Treat as a local time if a <see cref="DateTime"/> is being converted to a string.
        /// If a string is being converted to <see cref="DateTime"/>, convert to a local time if a time zone is specified.
        /// </summary>
        Unspecified = 2,

        /// <summary>
        /// Time zone information should be preserved when converting.
        /// </summary>
        RoundtripKind = 3
    }

    /// <summary>
    /// Specifies how date formatted strings, e.g. <c>"\/Date(1198908717056)\/"</c> and <c>"2012-03-21T05:40Z"</c>, are parsed when reading JSON text.
    /// </summary>
    internal enum DateParseHandling
    {
        /// <summary>
        /// Date formatted strings are not parsed to a date type and are read as strings.
        /// </summary>
        None = 0,

        /// <summary>
        /// Date formatted strings, e.g. <c>"\/Date(1198908717056)\/"</c> and <c>"2012-03-21T05:40Z"</c>, are parsed to <see cref="DateTime"/>.
        /// </summary>
        DateTime = 1,
#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Date formatted strings, e.g. <c>"\/Date(1198908717056)\/"</c> and <c>"2012-03-21T05:40Z"</c>, are parsed to <see cref="DateTimeOffset"/>.
        /// </summary>
        DateTimeOffset = 2
#endif
    }

    /// <summary>
    /// Specifies how floating point numbers, e.g. 1.0 and 9.9, are parsed when reading JSON text.
    /// </summary>
    internal enum FloatParseHandling
    {
        /// <summary>
        /// Floating point numbers are parsed to <see cref="Double"/>.
        /// </summary>
        Double = 0,

        /// <summary>
        /// Floating point numbers are parsed to <see cref="Decimal"/>.
        /// </summary>
        Decimal = 1
    }

    /// <summary>
    /// Specifies float format handling options when writing special floating point numbers, e.g. <see cref="Double.NaN"/>,
    /// <see cref="Double.PositiveInfinity"/> and <see cref="Double.NegativeInfinity"/> with <see cref="JsonWriter"/>.
    /// </summary>
    internal enum FloatFormatHandling
    {
        /// <summary>
        /// Write special floating point values as strings in JSON, e.g. <c>"NaN"</c>, <c>"Infinity"</c>, <c>"-Infinity"</c>.
        /// </summary>
        String = 0,

        /// <summary>
        /// Write special floating point values as symbols in JSON, e.g. <c>NaN</c>, <c>Infinity</c>, <c>-Infinity</c>.
        /// Note that this will produce non-valid JSON.
        /// </summary>
        Symbol = 1,

        /// <summary>
        /// Write special floating point values as the property's default value in JSON, e.g. 0.0 for a <see cref="Double"/> property, <c>null</c> for a <see cref="Nullable{T}"/> of <see cref="Double"/> property.
        /// </summary>
        DefaultValue = 2
    }

    /// <summary>
    /// Specifies how strings are escaped when writing JSON text.
    /// </summary>
    public enum StringEscapeHandling
    {
        /// <summary>
        /// Only control characters (e.g. newline) are escaped.
        /// </summary>
        Default = 0,

        /// <summary>
        /// All non-ASCII and control characters (e.g. newline) are escaped.
        /// </summary>
        EscapeNonAscii = 1,

        /// <summary>
        /// HTML (&lt;, &gt;, &amp;, &apos;, &quot;) and control characters (e.g. newline) are escaped.
        /// </summary>
        EscapeHtml = 2
    }

    /// <summary>
    /// Indicates the method that will be used during deserialization for locating and loading assemblies.
    /// </summary>
    internal enum TypeNameAssemblyFormatHandling
    {
        /// <summary>
        /// In simple mode, the assembly used during deserialization need not match exactly the assembly used during serialization. Specifically, the version numbers need not match as the <c>LoadWithPartialName</c> method of the <see cref="System.Reflection.Assembly"/> class is used to load the assembly.
        /// </summary>
        Simple = 0,

        /// <summary>
        /// In full mode, the assembly used during deserialization must match exactly the assembly used during serialization. The <c>Load</c> method of the <see cref="System.Reflection.Assembly"/> class is used to load the assembly.
        /// </summary>
        Full = 1
    }

    /// <summary>
    /// Specifies the member serialization options for the <see cref="JsonSerializer"/>.
    /// </summary>
    internal enum MemberSerialization
    {
#pragma warning disable 1584,1711,1572,1581,1580,1574
        /// <summary>
        /// All public members are serialized by default. Members can be excluded using <see cref="JsonIgnoreAttribute"/> or <see cref="NonSerializedAttribute"/>.
        /// This is the default member serialization mode.
        /// </summary>
        OptOut = 0,

        /// <summary>
        /// Only members marked with <see cref="JsonPropertyAttribute"/> or <see cref="DataMemberAttribute"/> are serialized.
        /// This member serialization mode can also be set by marking the class with <see cref="DataContractAttribute"/>.
        /// </summary>
        OptIn = 1,

        /// <summary>
        /// All public and private fields are serialized. Members can be excluded using <see cref="JsonIgnoreAttribute"/> or <see cref="NonSerializedAttribute"/>.
        /// This member serialization mode can also be set by marking the class with <see cref="SerializableAttribute"/>
        /// and setting IgnoreSerializableAttribute on <see cref="DefaultContractResolver"/> to <c>false</c>.
        /// </summary>
        Fields = 2
#pragma warning restore 1584,1711,1572,1581,1580,1574
    }

    /// <summary>
    /// Indicating whether a property is required.
    /// </summary>
    public enum Required
    {
        /// <summary>
        /// The property is not required. The default state.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The property must be defined in JSON but can be a null value.
        /// </summary>
        AllowNull = 1,

        /// <summary>
        /// The property must be defined in JSON and cannot be a null value.
        /// </summary>
        Always = 2,

        /// <summary>
        /// The property is not required but it cannot be a null value.
        /// </summary>
        DisallowNull = 3
    }

    /// <summary>
    /// Specifies the type of JSON token.
    /// </summary>
    public enum JsonToken //this was forced to be public
    {
        /// <summary>
        /// This is returned by the <see cref="JsonReader"/> if a read method has not been called.
        /// </summary>
        None = 0,

        /// <summary>
        /// An object start token.
        /// </summary>
        StartObject = 1,

        /// <summary>
        /// An array start token.
        /// </summary>
        StartArray = 2,

        /// <summary>
        /// A constructor start token.
        /// </summary>
        StartConstructor = 3,

        /// <summary>
        /// An object property name.
        /// </summary>
        PropertyName = 4,

        /// <summary>
        /// A comment.
        /// </summary>
        Comment = 5,

        /// <summary>
        /// Raw JSON.
        /// </summary>
        Raw = 6,

        /// <summary>
        /// An integer.
        /// </summary>
        Integer = 7,

        /// <summary>
        /// A float.
        /// </summary>
        Float = 8,

        /// <summary>
        /// A string.
        /// </summary>
        String = 9,

        /// <summary>
        /// A boolean.
        /// </summary>
        Boolean = 10,

        /// <summary>
        /// A null token.
        /// </summary>
        Null = 11,

        /// <summary>
        /// An undefined token.
        /// </summary>
        Undefined = 12,

        /// <summary>
        /// An object end token.
        /// </summary>
        EndObject = 13,

        /// <summary>
        /// An array end token.
        /// </summary>
        EndArray = 14,

        /// <summary>
        /// A constructor end token.
        /// </summary>
        EndConstructor = 15,

        /// <summary>
        /// A Date.
        /// </summary>
        Date = 16,

        /// <summary>
        /// Byte data.
        /// </summary>
        Bytes = 17
    }

    /// <summary>
    /// Specifies the state of the <see cref="JsonWriter"/>.
    /// </summary>
    internal enum WriteState
    {
        /// <summary>
        /// An exception has been thrown, which has left the <see cref="JsonWriter"/> in an invalid state.
        /// You may call the <see cref="JsonWriter.Close()"/> method to put the <see cref="JsonWriter"/> in the <c>Closed</c> state.
        /// Any other <see cref="JsonWriter"/> method calls result in an <see cref="InvalidOperationException"/> being thrown.
        /// </summary>
        Error = 0,

        /// <summary>
        /// The <see cref="JsonWriter.Close()"/> method has been called.
        /// </summary>
        Closed = 1,

        /// <summary>
        /// An object is being written. 
        /// </summary>
        Object = 2,

        /// <summary>
        /// An array is being written.
        /// </summary>
        Array = 3,

        /// <summary>
        /// A constructor is being written.
        /// </summary>
        Constructor = 4,

        /// <summary>
        /// A property is being written.
        /// </summary>
        Property = 5,

        /// <summary>
        /// A <see cref="JsonWriter"/> write method has not been called.
        /// </summary>
        Start = 6
    }

    
}
