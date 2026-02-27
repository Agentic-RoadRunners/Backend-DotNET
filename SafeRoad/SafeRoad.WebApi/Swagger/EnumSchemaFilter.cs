using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SafeRoad.WebApi.Swagger;

/// <summary>
/// Appends human-readable enum member names to every enum schema
/// so Swagger UI shows "0 = Pending, 1 = Verified, …" instead of bare integers.
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        var names = Enum.GetNames(context.Type);
        var values = Enum.GetValues(context.Type);

        var entries = new List<string>();
        for (var i = 0; i < names.Length; i++)
            entries.Add($"{Convert.ToInt32(values.GetValue(i))} = {names[i]}");

        schema.Description = string.IsNullOrEmpty(schema.Description)
            ? string.Join(" | ", entries)
            : $"{schema.Description} ({string.Join(" | ", entries)})";

        // Also provide the enum string names so Swagger UI shows them
        schema.Enum.Clear();
        foreach (var val in values)
            schema.Enum.Add(new OpenApiInteger(Convert.ToInt32(val)));
    }
}
