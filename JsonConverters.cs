using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class BodyConverter : Newtonsoft.Json.JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Body);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        return serializer.Deserialize<Body>(reader);
    }

    public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
    {
        // logic to determine if I should be serializing as Body or List<Error>
        var body = value as Body;

        if(body.errors != null && body.errors.Count > 0)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("errors");

            serializer.Serialize(writer, body.errors);
            writer.WriteEndObject();
        }
        else
        {
            writer.WriteStartObject();
            writer.WritePropertyName("query");
            writer.WriteValue(body.query);

            writer.WritePropertyName("operationName");
            writer.WriteValue(body.operationName);
            writer.WriteEndObject();
        }
    }
}

public class ControlConverter : Newtonsoft.Json.JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(object);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if(reader.TokenType == JsonToken.String)
        {
            return reader.Value.ToString();
        }
        return serializer.Deserialize<Control>(reader);
    }

    public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
    {
        if(value is string controlString)
        {
            writer.WriteValue(controlString);
        }
        else if(value is Control controlObject)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("break");

            serializer.Serialize(writer, controlObject.Break);

            writer.WriteEndObject();
        }
        else
        {
            // Default to "continue" if not doing a break or if a new value is not passed
            writer.WriteValue("continue");
        }
    }
}