using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice
{
    //public class Startup
    //{
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public IConfiguration Configuration { get; }

    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddControllersWithViews();
    //        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    //        services.AddTransient<IUnitOfWork, UnitOfWork>();
    //        //services.AddServices(Configuration);
    //        services.AddMvc();
    //        //For SignalR ...............................
    //        services.AddSingleton<SignalRListenerFarms>();
    //        services.AddSingleton<FirebaseNotificationService>();
    //        services.AddScoped<SQL>();
    //        services.AddMemoryCache();
    //        services.AddScoped<TimeCacheService>();
    //        string connectionString = Configuration.GetConnectionString("MazraeatiConnString");
    //        services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
    //        services.AddScoped<IAdminService, AdminService>();
    //        services.AddScoped<IUserService, UserService>();
    //        services.AddScoped<IPermissionService, PermissionService>();
    //        services.AddScoped<PermissionFilter>(); 
    //        services.AddHttpContextAccessor();
    //        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //        //.AddCookie(options =>
    //        //{
    //        //    options.LoginPath = "/Account/Login";
    //        //    options.AccessDeniedPath = "/Account/Login";

    //        //    options.Cookie.Name = "MazraeatiAuth";

    //        //    options.ExpireTimeSpan = TimeSpan.FromDays(30);

    //        //    options.SlidingExpiration = true;
    //        //});
    //        .AddCookie(options =>
    //        {
    //            options.LoginPath = "/Account/Login";
    //            options.AccessDeniedPath = "/Account/AccessDenied";
    //            options.LogoutPath = "/Account/Logout";

    //            options.Cookie.Name = "MazraeatiAuth";
    //            options.Cookie.HttpOnly = true;
    //            options.Cookie.SameSite = SameSiteMode.Lax;
    //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

    //            // ========== مهم: نخليها SlidingExpiration بس منغير ExpireTimeSpan ثابت ==========
    //            options.SlidingExpiration = true;
    //            // options.ExpireTimeSpan = TimeSpan.FromDays(30); // نشيلها من هنا عشان نخلي التحكم في الـ Login Action

    //            options.Cookie.MaxAge = TimeSpan.FromDays(30); // أقصى عمر للـ Cookie
    //        });

    //        services.AddSession(options =>
    //        {
    //            options.IdleTimeout = TimeSpan.FromDays(30); // الـ Session timeout 30 دقيقة
    //            options.Cookie.HttpOnly = true;
    //            options.Cookie.IsEssential = true;
    //            options.Cookie.SameSite = SameSiteMode.Lax;
    //        });

    //        services.AddControllersWithViews(options =>
    //        {
    //            options.Filters.Add<PermissionFilter>();  // هيشتغل على كل الـ Requests
    //        });
    //    }
    //    // ===== IMPORTANT: Add Permission Filter Globally =====

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }
    //        else
    //        {
    //            app.UseDeveloperExceptionPage();
    //            app.UseExceptionHandler("/Home/Error");
    //            app.UseHsts();
    //        }

    //        //For SignalR .....
    //        //stop the second code for make stop for signal R ......
    //        using (var scope = app.ApplicationServices.CreateScope()) // ? fixed
    //        {
    //            var listener = scope.ServiceProvider.GetRequiredService<SignalRListenerFarms>();
    //            listener.StartAsync().GetAwaiter().GetResult(); // ? fixed
    //        }


    //        app.UseHttpsRedirection();
    //        app.UseStaticFiles();
    //        app.UseRouting();

    //        app.UseAuthentication();
    //        app.UseAuthorization();
    //        app.UseSession();

    //        var cookiePolicyOptions = new CookiePolicyOptions
    //        {
    //            MinimumSameSitePolicy = SameSiteMode.Strict,
    //        };
    //        app.UseCookiePolicy(cookiePolicyOptions);
    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapControllers();
    //            endpoints.MapRazorPages();
    //            endpoints.MapControllerRoute(
    //                name: "default",
    //                pattern: "{controller=Account}/{action=Login}/{id?}");
    //        });
    //    }
    //}


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
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddMvc();
            //For SignalR ...............................
            services.AddSingleton<SignalRListenerFarms>();
            services.AddSingleton<SignalRListenerPrices>();
            services.AddSingleton<FirebaseNotificationService>();
            services.AddScoped<SQL>();
            services.AddMemoryCache();
            services.AddScoped<TimeCacheService>();
            string connectionString = Configuration.GetConnectionString("MazraeatiConnString");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<PermissionFilter>();
            //services.AddScoped<LoyaltyService>();
            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LogoutPath = "/Account/Logout";

                    options.Cookie.Name = "MazraeatiAuth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                    options.SlidingExpiration = true;
                    options.Cookie.MaxAge = TimeSpan.FromDays(30);
                });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            // ===== IMPORTANT: Add Permission Filter Globally =====
            //For Permissions Screen ...

            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add<PermissionFilter>();
            //});

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
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            //For SignalR .....
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var listener = scope.ServiceProvider.GetRequiredService<SignalRListenerFarms>();
                listener.StartAsync().GetAwaiter().GetResult();

                var priceListener = scope.ServiceProvider.GetRequiredService<SignalRListenerPrices>();
                priceListener.StartAsync().GetAwaiter().GetResult();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // ===== مهم: ترتيب الـ Middleware =====
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
