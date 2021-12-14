using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SBD_Projekt.Models;
using SBDProjekt.Infrastructure;
using SBDProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SBD_Projekt
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
            services.AddControllersWithViews();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(3600);
            });

            services.AddDbContext<MyDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MyDB"));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Clients/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnSigningIn = async context =>
                        {
                            var principal = context.Principal;
                            if (principal.HasClaim(c => c.Type == ClaimTypes.Email
                                && principal.Claims.FirstOrDefault(
                                    c => c.Type == ClaimTypes.Email).Value == "test@test.pl"))
                            {
                                var claimsIdentity = principal.Identity as ClaimsIdentity;
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Tester"));
                            }
                            await Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(config =>
            {
                var userAuthPolicyBuilder = new AuthorizationPolicyBuilder();
                config.DefaultPolicy = userAuthPolicyBuilder
                                    .RequireAuthenticatedUser()
                                    .RequireClaim(ClaimTypes.DateOfBirth)
                                    .Build();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
