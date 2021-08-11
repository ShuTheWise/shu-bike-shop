using shu_bike_shop.Areas.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Npgsql;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using shu_bike_shop.Data;
using DataAccessLibrary;
using Microsoft.AspNetCore.Components.Authorization;

namespace shu_bike_shop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var databaseURL = Configuration["DATABASE_URL"];

            var uri = new Uri(databaseURL);
            var username = uri.UserInfo.Split(':')[0];
            var password = uri.UserInfo.Split(':')[1];

            var connectionString =
             "; Database=" + uri.AbsolutePath.Substring(1) +
             "; Username=" + username +
             "; Password=" + password +
             "; Port=" + uri.Port +
             "; SSL Mode=Require; Trust Server Certificate=true;";

            var builder = new NpgsqlConnectionStringBuilder(connectionString) { Host = uri.Host };

            var connectionString2 = builder.ToString();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString2));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpContextAccessor();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<WeatherForecastService>();
            
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddTransient<IProductData, ProductData>();
            services.AddTransient<IBikesData, BikesData>();
            services.AddTransient<IUserData, UserData>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOrderData, OrderData>();
            services.AddTransient<IPaymentService, PaymentService>();

            services.AddSingleton<IBasketService, BasketService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
