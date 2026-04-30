namespace NotesServer.Services.GraphQL;

using System.Text.Json;

public static class GraphQLVariableConverter
{
    public static Dictionary<string, object> ConvertVariables(Dictionary<string, object> variables)
    {
        var result = new Dictionary<string, object>();
        foreach (var kvp in variables)
        {
            if (kvp.Value is JsonElement je)
            {
                result[kvp.Key] = je.ValueKind switch
                {
                    JsonValueKind.String => je.GetString(),
                    JsonValueKind.Number => je.TryGetInt32(out int i) ? i : je.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    JsonValueKind.Null => null,
                    _ => je.ToString()
                };
            }
            else
            {
                result[kvp.Key] = kvp.Value;
            }
        }
        return result;
    }
}