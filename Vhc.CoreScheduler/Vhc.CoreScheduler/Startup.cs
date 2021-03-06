﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vhc.CoreScheduler.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vhc.CoreScheduler.Common.Services;
using System.Threading;
using Vhc.CoreScheduler.Services;
using Microsoft.Extensions.Logging;

namespace Vhc.CoreScheduler
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
            services.AddLogging(config =>
            {
                config.AddLog4Net();
                config.SetMinimumLevel(LogLevel.Information);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddSingleton<ISchedulerService, SchedulerService>();
            services.AddTransient<IUnitService, UnitService>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddScoped<IVariableService, VariableService>();
            services.AddSingleton<TriggerService>();

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            Task.Run(async () =>
            {
                var service = app.ApplicationServices.GetService<ISchedulerService>();
                var cts = new CancellationTokenSource();
                await service.StartAsync(cts.Token);

                var triggerService = app.ApplicationServices.GetService<TriggerService>();
                await triggerService.RegisterAllTriggers();
                //await service.RegisterTriggerAsync(new Common.Models.TriggerDefinition
                //{
                //    Id = 1,
                //    Name = "A",
                //    CronExpression = "30 * * * * ?",
                //    Environment = new Common.Models.ExecutionEnvironment
                //    {
                //        Id = 1,
                //        Name = "ENV",
                //        ConnectionString = "Data Source=env.db"
                //    },
                //    JobDefinition = new Common.Models.JobDefinition
                //    {
                //        Id = 2,
                //        Name = "JA",
                //        Group = new Common.Models.JobGroup
                //        {
                //            Id = 1, Name = "G"
                //        },
                //        UnitCollectionId = 345
                //    }
                //});

                
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
