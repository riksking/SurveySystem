using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag;
using Serilog;
using Serilog.Extensions.Logging;
using SurveySystem.WebApi.Common;
using SurveySystem.WebApi.Context;
using SurveySystem.WebApi.Services;

namespace SurveySystem.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public bool SwaggerIsEnabled => Configuration?.GetValue(nameof(SwaggerIsEnabled), false) ?? false;

        private readonly string _swaggerDocVersion;
        private readonly string _swaggerDocTitle;
        private readonly string _swaggerDocDescription;
        private readonly string _swaggerDocCompany;
        private readonly string _swaggerDocCopyright;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var attributes = Assembly.GetExecutingAssembly().CustomAttributes;

            _swaggerDocVersion = ReflectionHelper.AttributeReader<AssemblyVersionAttribute>(attributes)
                                 ?? ReflectionHelper.AttributeReader<AssemblyFileVersionAttribute>(attributes);
            _swaggerDocTitle = ReflectionHelper.AttributeReader<AssemblyTitleAttribute>(attributes);
            _swaggerDocDescription = ReflectionHelper.AttributeReader<AssemblyDescriptionAttribute>(attributes);
            _swaggerDocCompany = ReflectionHelper.AttributeReader<AssemblyCompanyAttribute>(attributes);
            _swaggerDocCopyright = ReflectionHelper.AttributeReader<AssemblyCopyrightAttribute>(attributes);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var providers = new LoggerProviderCollection();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            ConfigureCors(services);
            ConfigureSwagger(services);

            services.AddSingleton(providers);
            services.AddSingleton<ILoggerFactory>(sc =>
            {
                var providerCollection = sc.GetService<LoggerProviderCollection>();
                var factory = new SerilogLoggerFactory(null, true, providerCollection);

                foreach (var provider in sc.GetServices<ILoggerProvider>())
                {
                    factory.AddProvider(provider);
                }

                return factory;
            });

            services.AddControllers();

            ConfigureDatabase(services);
            ConfigureCustomServices(services);
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var surveyCS = Configuration.GetConnectionString("ConnectionString");

            services.AddDbContext<SurveySystemDbContext>(builder =>
            {
                builder.UseSqlServer(surveyCS);
            });

            services.AddScoped<ISurveySystemDbContext, SurveySystemDbContext>();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (SwaggerIsEnabled)
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            // подключаем CORS
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.SetIsOriginAllowed(host => true);
                policy.AllowCredentials();
            });

            app.UseCors("CORS"); // обязательно до UseMVC!!!
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddScoped<ISessionBase, SessionBase>();
            services.AddScoped<IAnswerService, AnswerService>();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            if (SwaggerIsEnabled)
            {
                services.AddSwaggerDocument(config =>
                {
                    config.PostProcess = document =>
                    {
                        document.Info.Version = _swaggerDocVersion;
                        document.Info.Title = _swaggerDocTitle;
                        document.Info.Description = _swaggerDocDescription;
                        document.Info.Contact = new OpenApiContact
                        {
                            Name = _swaggerDocCompany,
                            Email = string.Empty,
                            Url = "https://www.google.com/"
                        };
                        document.Info.License = new OpenApiLicense
                        {
                            Name = _swaggerDocCopyright
                        };
                    };
                });
            }
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
    }
}
