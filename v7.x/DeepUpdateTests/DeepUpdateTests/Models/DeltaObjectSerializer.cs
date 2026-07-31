//------------------------------------------------------------------------------------------------- 
// <copyright file="DeltaObjectSerializer.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------

using Microsoft.AspNet.OData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Reflection;
namespace DeepUpdateTests.Models
{

    /// <summary>
    /// Provides serialization capabilities for EdmDelta objects
    /// </summary>
    public static class DeltaObjectSerializer
    {
        /// <summary>
        /// Serializes an EdmDeltaEntityObject to a JSON string
        /// </summary>
        /// <param name="deltaObject">The delta object to serialize</param>
        /// <returns>A JSON string representation of the delta object</returns>
        public static string AsSerializedDeltaObject(EdmDeltaEntityObject deltaObject)
        {
            if (deltaObject == null)
            {
                throw new ArgumentNullException(nameof(deltaObject));
            }

            var jsonObject = new JObject();
            SerializeDeltaObject(deltaObject, jsonObject);
            return jsonObject.ToString(Formatting.None);
        }

        /// <summary>
        /// Serializes an EdmDeltaComplexObject to a JSON string
        /// </summary>
        /// <param name="deltaObject">The delta complex object to serialize</param>
        /// <returns>A JSON string representation of the complex object</returns>
        public static string AsSerializedDeltaObject(EdmDeltaComplexObject deltaObject)
        {
            if (deltaObject == null)
            {
                throw new ArgumentNullException(nameof(deltaObject));
            }

            var jsonObject = new JObject();
            SerializeDeltaObject(deltaObject, jsonObject);
            return jsonObject.ToString(Formatting.None);
        }

        private static void SerializeDeltaObject(IDelta deltaObject, JObject jsonObject)
        {
            // Serialize properties
            foreach (var propertyName in deltaObject.GetChangedPropertyNames())
            {
                object propertyValue;
                if (deltaObject.TryGetPropertyValue(propertyName, out propertyValue))
                {
                    JToken valueToken = SerializePropertyValue(propertyValue);
                    jsonObject.Add(propertyName, valueToken);
                }
            }
        }

        private static JToken SerializePropertyValue(object value)
        {
            if (value == null)
            {
                return JValue.CreateNull();
            }

            // Handle any IDelta (both EdmDeltaEntityObject and EdmDeltaComplexObject) recursively
            if (value is IDelta delta)
            {
                var deltaJson = new JObject();
                SerializeDeltaObject(delta, deltaJson);
                return deltaJson;
            }

            // Handle collections
            if (value is IEnumerable enumerable && !(value is string))
            {
                var arrayJson = new JArray();
                foreach (var item in enumerable)
                {
                    arrayJson.Add(SerializePropertyValue(item));
                }

                return arrayJson;
            }

            // Handle primitive types with fallback
            return ConvertPrimitiveWithFallback(value);
        }

        private static JToken ConvertPrimitiveWithFallback(object value)
        {
            if (value == null)
            {
                return JValue.CreateNull();
            }

            switch (value)
            {
                case Guid g:
                    return new JValue(g.ToString());
                default:
                    return JToken.FromObject(value);
            }
        }
    }
}