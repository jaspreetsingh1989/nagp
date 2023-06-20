using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Read.Data.BLL;
using Read.Data.DAL;
using Read.Data.Helper;
using Read.Data.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Read.Data
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configurations

            //var dplConfig = Configuration.GetSection(Constants.ConfigurationKeys.DPL);
            //services.Configure<CosmosDBConfigurations>(dplConfig.GetSection(Constants.ConfigurationKeys.CosmosDb));

            //Caching
            //Add Distributed Cache in Memory
            services.AddDistributedMemoryCache();

            //Business Layer
            services.AddScoped<IDocumentsBL, DocumentsBL>();

            services.AddScoped<IDocumentsDAL, DocumentsDAL>();

            //To enable JObject 
            services.AddMvc().AddNewtonsoftJson();

            //Versioning
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VV";
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new QueryStringApiVersionReader();
            });

            //API Documentation
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            services.AddControllers(setupAction =>
            {
                //Gives 406 error if non supported accept header is passed in request header
                setupAction.ReturnHttpNotAcceptable = true;
                //Setup available responses on all APIs
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            }).AddXmlDataContractSerializerFormatters();// Adds XML type response capability to API
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Setup generic error in case of exception for production
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{Constants.ApiName} API {description.GroupName}");
                    }
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
