using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Otb.Autofac.Modules;
using Otb.Configuration;
using RabbitMeddling.Services;
using Serilog;
using System;
using System.IO;
using Otb.AspNetCore.Utilities.Middleware;
using Otb.RabbitMq.Utilities.Conventions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace RabbitMeddling
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public ILogger Logger { get; set; }

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            Configuration = configuration;
            Log.Logger = ConfigureSerilog(env);
            loggerFactory.AddConsole();
            loggerFactory.AddSerilog();
            Logger = loggerFactory.CreateLogger("startup");
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var builder = new ContainerBuilder();
            var configurationProvider = new MicrosoftConfigurationProvider("RabbitMeddling", Configuration);
            builder.RegisterInstance(Log.Logger).As<Serilog.ILogger>();
            builder.RegisterInstance(configurationProvider).As<IProvideConfiguration>();
            builder.RegisterModule(new ConventionRegistrationModule(typeof(Startup).Assembly));
            builder.RegisterModule(new RabbitMqModule(configurationProvider));
            builder.Populate(services);
            return builder.Build().Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLogger(Logger);
            app.UseMvc();
        }

        private Serilog.ILogger ConfigureSerilog(IHostingEnvironment hostingEnvironment)
        {
            var config = new LoggerConfiguration();
            var configurationProvider = new MicrosoftConfigurationProvider("RabbitMeddling", Configuration);
            config = hostingEnvironment.IsDevelopment() ? config.MinimumLevel.Verbose() : config.MinimumLevel.Information();
            return config
                .WriteTo.RollingFile(Path.Combine(hostingEnvironment.ContentRootPath, configurationProvider.GetValue<string>("Logging:Path")))
                .CreateLogger();
        }
    }
}
