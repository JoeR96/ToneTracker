using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ToneTracker.Filters;

public class GuidSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type == typeof(Guid))
        {
            model.Format = "uuid";
        }
    }
}
