using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

[JsonSerializable(typeof(SupergraphRequestPayload))]
public partial class SourceGenerationContext : JsonSerializerContext {}

public class SupergraphRequestPayload
{
    public int version { get; set; }
    public string stage { get; set; }
    [Newtonsoft.Json.JsonConverter(typeof(ControlConverter))]
    public object control { get; set; }
    public string id { get; set; }
    public Dictionary <string, string[]> headers { get; set; } = new Dictionary<string, string[]>();
    [Newtonsoft.Json.JsonConverter(typeof(BodyConverter))]
    public Body body { get; set; } = new Body();
    public Context context { get; set; }
    public string sdl { get; set; }
    public string path { get; set; }
    public string method { get; set; }

    public void AddError(Error error)
    {
        if(body.errors == null)
        {
            body.errors = new List<Error>();
        }
        body.errors.Add(error);
    }
}

public class Body
{
    public string query { get; set; }
    public string operationName { get; set; }
    public List<Error> errors { get; set; } = new List<Error>();
}

public record Error(string message, Extensions extensions);

public record Extensions(string code);

public record Control(int Break);

public class Context
{
    public Dictionary <string, string> entries { get; set; } = new Dictionary<string, string>();
}
