using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Codefix.Dataverse.Configs
{
    public class AttributeBasedConverter<T> : Newtonsoft.Json.JsonConverter<T>
    {
        public override bool CanRead => true;



        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            var objectType = value.GetType();
            var properties = objectType.GetProperties();
            writer.WriteStartObject();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                string propertyName = null;
                foreach (var attribute in attributes)
                {
                    if (attribute is AttributeLogicalNameAttribute logicalNameAttribute)
                    {
                        propertyName = logicalNameAttribute.LogicalName;
                        break;
                    }
                    if (attribute is JsonPropertyAttribute jsonPropertyAttribute)
                    {
                        propertyName = jsonPropertyAttribute.PropertyName;
                        break;
                    }
                    if (attribute is JsonPropertyNameAttribute jsonPropertyNameAttribute)
                    {
                        propertyName = jsonPropertyNameAttribute.Name;
                        break;
                    }
                    if (attribute is RelationshipSchemaNameAttribute relationshipSchemaNameAttribute)
                    {
                        propertyName = relationshipSchemaNameAttribute.SchemaName;
                        break;
                    }
                }

                if (propertyName != null)
                {
                    writer.WritePropertyName(propertyName);
                    var propertyValue = property.GetValue(value);
                    serializer.Serialize(writer, propertyValue);
                }
            }
            writer.WriteEndObject();
        }

        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return default;

            var jsonObject = JObject.Load(reader);
            var properties = objectType.GetProperties();
            dynamic instance = Activator.CreateInstance(objectType);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is AttributeLogicalNameAttribute logicalNameAttribute && jsonObject.TryGetValue(logicalNameAttribute.LogicalName, out var value))
                    {
                        var convertedValue = value.ToObject(property.PropertyType, serializer);
                        property.SetValue(instance, convertedValue);
                        break;
                    }
                    if (attribute is JsonPropertyAttribute jsonPropertyAttribute && jsonObject.TryGetValue(jsonPropertyAttribute.PropertyName, out var value2))
                    {
                        var convertedValue = value2.ToObject(property.PropertyType, serializer);
                        property.SetValue(instance, convertedValue);
                        break;
                    }
                    if (attribute is JsonPropertyNameAttribute jsonPropertyNameAttribute && jsonObject.TryGetValue(jsonPropertyNameAttribute.Name, out var value3))
                    {
                        var convertedValue = value3.ToObject(property.PropertyType, serializer);
                        property.SetValue(instance, convertedValue);
                        break;
                    }
                    if (attribute is RelationshipSchemaNameAttribute relationshipSchemaNameAttribute && jsonObject.TryGetValue(relationshipSchemaNameAttribute.SchemaName, out var value4))
                    {
                        var convertedValue = value4.ToObject(property.PropertyType, serializer);
                        property.SetValue(instance, convertedValue);
                        break;
                    }
                }
            }

            return instance;
        }
    }
}
