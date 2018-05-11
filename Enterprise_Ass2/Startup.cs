using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Enterprise_Ass2.Data;
using Enterprise_Ass2.Models;
using Enterprise_Ass2.Services;

namespace Enterprise_Ass2
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<VehicleContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("VehicleConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(services).Wait();
        }

        private object CreateUserRoles(object services)
        {
            throw new NotImplementedException();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult, roleResult2, roleResult3, roleResult4;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Approver");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Approver"));
                roleResult2 = await RoleManager.CreateAsync(new IdentityRole("Editor"));
                roleResult3 = await RoleManager.CreateAsync(new IdentityRole("Editor2"));
                roleResult4 = await RoleManager.CreateAsync(new IdentityRole("DataMaintenance"));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            ApplicationUser user = await UserManager.FindByEmailAsync("Approver@gmail.com");
            ApplicationUser user2 = await UserManager.FindByEmailAsync("Editor@gmail.com");
            ApplicationUser user3 = await UserManager.FindByEmailAsync("Editor2@gmail.com");
            ApplicationUser user4 = await UserManager.FindByEmailAsync("DataMaintenance@gmail.com");
            var User = new ApplicationUser();
            await UserManager.AddToRoleAsync(user, "Approver");
            await UserManager.AddToRoleAsync(user2, "Editor");
            await UserManager.AddToRoleAsync(user3, "Editor2");
            await UserManager.AddToRoleAsync(user4, "DataMaintenance");
        }
    }
}
