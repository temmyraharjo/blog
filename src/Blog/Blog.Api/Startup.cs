using Blog.Api.Common;
using Blog.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Okta.AspNetCore;
using SimpleInjector;
using OktaWebApiOptions = Okta.AspNet.Abstractions.OktaWebApiOptions;

namespace Blog.Api
{
    public class Startup
    {
        private readonly Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<CustomExceptionFilterAttribute>();
                });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
                })
                .AddOktaWebApi(new Okta.AspNetCore.OktaWebApiOptions
                {
                    OktaDomain = Configuration["Okta:Domain"],
                    AuthorizationServerId = Configuration["Okta:AuthorizationServerId"],
                    Audience = Configuration["Okta:Audience"]
                });

            services.AddSimpleInjector(_container, options =>
            {
                options.AddAspNetCore()
                    .AddControllerActivation()
                    .AddPageModelActivation()
                    .AddTagHelperActivation()
                    .AddViewComponentActivation();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            new Bootstrap(_container).Handle();
            _container.Verify();
            app.UseMvc();
        }
    }
}
