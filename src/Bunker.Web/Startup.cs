﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunker.Business.Interfaces.Services;
using Bunker.Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bunker.Web
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            var connectionString = Configuration.GetConnectionString("BunkerContext");
            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<BunkerDbContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IChallangeService, ChallangeService>();

            services.AddScoped<IInitLoaderService, InitLoaderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IInitLoaderService initLoaderService)
        {
            initLoaderService.Init();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Auth}/{action=Login}/{id?}");
            });
        }
    }
}