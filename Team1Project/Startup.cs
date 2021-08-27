using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Team1Project.Data;
using Team1Project.Services;

namespace Team1Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       
        public IConfiguration Configuration { get; }

        public static string ConvertHerokuStringToASPNETString(string herokuConnectionString)
        {
            var databaseUri = new Uri(herokuConnectionString);
            var split = databaseUri.UserInfo.Split(':');
            var username = split[0];
            var password = split[1];
            var database = databaseUri.LocalPath.Split('/')[1];
            return $"Server={databaseUri.Host};Port={databaseUri.Port};Database={database};SslMode=Require;Trust Server Certificate=true;Integrated Security=true;User Id={username};Password={password};";
        }

        // returns dbConnectionString from DATABASE_URL environment variable, or the PostgresHerokuConnection if the variable is not initialized
        private string ObtainConnectionString()
        {
            string envVarDbString = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (envVarDbString == null)
            {
                // return Configuration.GetConnectionString("PostgresHerokuConnection");
                return Configuration.GetConnectionString("DefaultConnection");
            }

            return ConvertHerokuStringToASPNETString(envVarDbString);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var herokuConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    //ConvertHerokuStringToASPNETString(herokuConnectionString)));
                    this.ObtainConnectionString()));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            services.AddSignalR();
            services.AddSingleton<ITeamBroadcastService, TeamBroadcastService>();
            services.AddSingleton<IInternBroadcastService, InternBroadcastService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Team 1 Project", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            AssignRoleProgramaticaly(services.BuildServiceProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Team 1 Project v1"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<TeamMessageHub>("/teamMessageHub");
                endpoints.MapHub<InternMessageHub>("/internMessageHub");
            });
        }

        private async void AssignRoleProgramaticaly(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            try
            {
                var user = await userManager.FindByNameAsync("tudor.pop@principal33.com");
                await userManager.AddToRoleAsync(user, "Administrator");
            }
            catch (Exception e)
            {
                Console.WriteLine("Edit the string connection in appsettings.json");
            }
        }
    }
}
