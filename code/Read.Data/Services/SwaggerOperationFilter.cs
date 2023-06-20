using Read.Data.Helper;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Read.Data.Services
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string deprecatedText = "[deprecated]";
            if (!string.IsNullOrEmpty(operation.Summary) && operation.Summary.Contains(deprecatedText))
            {
                operation.Summary = operation.Summary.Replace(deprecatedText, string.Empty);
                operation.Deprecated = true;
            }

            if (!string.IsNullOrEmpty(operation.OperationId))
            {
                operation.Description = operation.Summary;
                operation.Summary = operation.OperationId;
                operation.OperationId = operation.OperationId.ToLowerInvariant().Replace(" ", "-");
            }

            if (operation.Parameters.Count > 0)
            {
                string requiredText = "[required]";
                foreach (var param in operation.Parameters)
                {
                    if (!string.IsNullOrEmpty(param.Description))
                    {
                        if (param.Description.Contains(requiredText))
                        {
                            param.Description = param.Description.Replace(requiredText, string.Empty);
                            param.Required = true;
                        }
                        if (param.Description.Contains(deprecatedText))
                        {
                            param.Description = param.Description.Replace(deprecatedText, string.Empty);
                            param.Deprecated = true;
                        }
                        param.Description = param.Description.Trim();
                    }
                }
            }

            //Sort Response by Response Codes
            if (operation.Responses != null && operation.Responses.Count > 0)
            {
                if (operation.Responses.All(r => int.TryParse(r.Key, out int val)))
                {
                    List<KeyValuePair<int, OpenApiResponse>> resTypes = new List<KeyValuePair<int, OpenApiResponse>>();
                    resTypes = operation.Responses.Select(r => new KeyValuePair<int, OpenApiResponse>(int.Parse(r.Key), r.Value)).ToList();
                    resTypes.Sort((a, b) => a.Key.CompareTo(b.Key));

                    operation.Responses.Clear();
                    foreach (var res in resTypes)
                    {
                        operation.Responses.Add(res.Key.ToString(), res.Value);
                    }
                }
            }
        }
    }
}
