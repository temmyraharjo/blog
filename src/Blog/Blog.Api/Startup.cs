<<<<<<< Updated upstream:src/backend/backend/Startup.cs
﻿using AutoMapper;
=======
﻿using Blog.Api.Common;
using Blog.Core;
>>>>>>> Stashed changes:src/Blog/Blog.Api/Startup.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

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
<<<<<<< Updated upstream:src/backend/backend/Startup.cs
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new Features.User.MappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddMvc();
=======
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<CustomExceptionFilterAttribute>();
                });

            services.AddSimpleInjector(_container, options =>
            {
                options.AddAspNetCore()
                    .AddControllerActivation()
                    .AddPageModelActivation()
                    .AddTagHelperActivation()
                    .AddViewComponentActivation();
            });
>>>>>>> Stashed changes:src/Blog/Blog.Api/Startup.cs
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
