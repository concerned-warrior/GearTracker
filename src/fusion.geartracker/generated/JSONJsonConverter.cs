namespace fusion.geartracker.graphql;

public class JSONJsonConverter : JsonConverter<JSON>
{
    public override JSON? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonElement.ParseValue(ref reader);

        var scalar = new JSON
        {
            Value = value.ToString(),
        };

        return scalar;
    }

    public override void Write(Utf8JsonWriter writer, JSON value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}