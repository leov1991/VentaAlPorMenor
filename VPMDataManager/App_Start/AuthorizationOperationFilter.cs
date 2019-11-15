using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace VPMDataManager.App_Start
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            // Asegurarse de que la operación tiene una lista de parametros
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                description = "access_token",
                required = false,
                type = "string",
                @default = "bearer "
            });

        }
    }
}