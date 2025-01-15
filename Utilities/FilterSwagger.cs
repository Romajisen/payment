using Bosnet.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace Bosnet.Utilities
{
    public class ModelsFilter : ISchemaFilter
    {
        private readonly Dictionary<Type, OpenApiObject> _examples = new()
        {
            {
                typeof(TransferRequest), new OpenApiObject
                {
                    ["sourceAccountId"] = new OpenApiString("000109999999"),
                    ["currencyId"] = new OpenApiString("USD"),
                    ["amount"] = new OpenApiDouble(100),
                    ["targetAccountIds"] = new OpenApiArray
                    {
                        new OpenApiString("000108888888"),
                        new OpenApiString("000108757484"),
                        new OpenApiString("000108757484")
                    },
                    ["note"] = new OpenApiString("TRANSFER")
                }
            },
            {
                typeof(TarikRequest), new OpenApiObject
                {
                    ["accountId"] = new OpenApiString("000109999999"),
                    ["currencyId"] = new OpenApiString("SGD"),
                    ["amount"] = new OpenApiDouble(10120),
                    ["note"] = new OpenApiString("TARIK")
                }
            },
            {
                typeof(SetorRequest), new OpenApiObject
                {
                    ["accountId"] = new OpenApiString("000109999999"),
                    ["currencyId"] = new OpenApiString("IDR"),
                    ["amount"] = new OpenApiDouble(10000),
                    ["note"] = new OpenApiString("SETOR")
                }
            },
            {
                typeof(TransactionHistory), new OpenApiObject
                {
                    ["accountId"] = new OpenApiString("000109999999"),
                    ["startDate"] = new OpenApiString("2024-01-01"),
                    ["startDate"] = new OpenApiString("2024-12-30")
                }
            }
        };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (_examples.TryGetValue(context.Type, out var example))
            {
                schema.Example = example;
            }
        }
    }
}