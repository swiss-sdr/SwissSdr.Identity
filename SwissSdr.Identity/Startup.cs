using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using IdentityServer4;
using Kentor.AuthServices.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raven.Client;
using Raven.Client.Document;
using SwissSdr.Identity.Configuration;
using SwissSdr.Identity.Options;
using SwissSdr.Identity.Services;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SwissSdr.Identity
{
	public class Startup
	{
		private IList<CultureInfo> _supportedCultures = new[]
		{
			new CultureInfo("en"),
			new CultureInfo("de")
		};

		public IConfigurationRoot Configuration { get; private set; }
		public IContainer Container { get; private set; }

		public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName.ToLowerInvariant()}.json", optional: true)
				.AddEnvironmentVariables();

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddDataProtection();
			services.AddOptions();

			services.Configure<RavenDbOptions>(Configuration.GetSection(RavenDbOptions.Name));
			services.Configure<IdentityProviderOptions>(Configuration.GetSection(IdentityProviderOptions.Name));
			services.AddSingleton<IConfigureOptions<FacebookOptions>, ConfigureFacebookOptions>();
			services.AddSingleton<IConfigureOptions<GoogleOptions>, ConfigureGoogleOptions>();
			services.AddSingleton<IConfigureOptions<KentorAuthServicesOptions>, ConfigureKentorAuthServicesOptions>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

			services.AddScoped<RavenLoginService>();

			services.AddIdentityServer()
				.AddProfileService<RavenProfileService>()
				.AddSigningCredential(Certificates.GetSigningCertificate())
				.AddInMemoryApiResources(ApiResources.Get())
				.AddInMemoryIdentityResources(IdentityResources.Get())
				.AddInMemoryClients(Clients.Get());

			services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

			var mvc = services.AddMvc();
			mvc.AddFluentValidation(o =>
			{
				o.RegisterValidatorsFromAssemblyContaining<Startup>();
			});
			mvc.AddTypedRouting();
			mvc.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; });
			mvc.AddDataAnnotationsLocalization();

			var builder = new ContainerBuilder();
			builder.Populate(services);

			builder.Register(c => CreateDocumentStore(c)).SingleInstance();
			builder.Register(c => c.Resolve<IDocumentStore>().OpenAsyncSession()).InstancePerLifetimeScope();

			Container = builder.Build();
			return new AutofacServiceProvider(Container);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				loggerFactory.AddConsole(Configuration.GetSection("Logging"));
				loggerFactory.AddDebug();
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}

			// idrsv
			app.UseIdentityServer();
			app.UseGoogleAuthentication();
			app.UseFacebookAuthentication();
			app.UseKentorAuthServices();

			// mvc
			app.UseRequestLocalization(new RequestLocalizationOptions()
			{
				DefaultRequestCulture = new RequestCulture("en"),
				SupportedCultures = _supportedCultures,
				SupportedUICultures = _supportedCultures
			});

			app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();
		}

		private IDocumentStore CreateDocumentStore(IComponentContext context)
		{
			var options = context.Resolve<IOptions<RavenDbOptions>>().Value;

			var documentStore = new DocumentStore()
			{
				Url = options.Url,
				ApiKey = options.ApiKey
			};
			documentStore.Initialize();
			documentStore.Conventions.CustomizeJsonSerializer = opt =>
			{
				opt.Binder = new NetCoreCompatRavenSerializationBinder();
				opt.ReferenceLoopHandling = Raven.Imports.Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			};

			return documentStore;
		}
	}
}
