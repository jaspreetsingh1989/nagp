using Read.Data.Helper;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;

namespace Read.Data.Services
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.apiVersionDescriptionProvider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }

            var xmlCommentsFile = $"{Constants.ApiName}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            options.IncludeXmlComments(xmlCommentsFullPath);

            options.DocInclusionPredicate((strVersion, apiDescription) =>
            {
                var actionApiVersionModel = apiDescription.ActionDescriptor.GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit | ApiVersionMapping.None);
                if (null == actionApiVersionModel || actionApiVersionModel.IsApiVersionNeutral)
                    return true;

                if (actionApiVersionModel.DeclaredApiVersions.Any())
                {
                    return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v}".Equals(strVersion, StringComparison.InvariantCultureIgnoreCase));
                }

                return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v}".Equals(strVersion, StringComparison.InvariantCultureIgnoreCase));
            });

            options.ResolveConflictingActions(c => c.FirstOrDefault());
            options.OperationFilter<SwaggerOperationFilter>();
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var apiInfo = new OpenApiInfo()
            {
                Title = $"{Constants.ApiName} API {description.GroupName}",
                Version = description.ApiVersion.ToString(),
                Description = $"This service provides a set of Documents mapped with any PIMS entity. The PIMS entity can be a Solution, Product Family, Base Model Number or Tag name. Internal name needs to be provided for PIMS entity name. This service can provide documents from Blob Storage and External source as well. Documents returned belong to given Country and Language. If for a given language data is not found, then data for English language is returned by default. Metadata information associated with corresponding item is also returned."
            };

            if (description.IsDeprecated)
            {
                apiInfo.Description += " This API version has been deprecated.";
            }

            return apiInfo;
        }
    }
}
