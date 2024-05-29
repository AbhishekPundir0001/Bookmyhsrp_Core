using BookMyHsrp.ExceptionHandling;
using BookMyHsrp.RequestResponseLoggingMiddleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using BookMyHsrp.Dapper;
using System.IO.Compression;
using System.Text;
using WebMarkupMin.AspNetCore7;
using BookMyHsrp.Libraries.HsrpState.Services;
using BookMyHsrp.ReportsLogics.HsrpWithColorSticker;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using BookMyHsrp.Libraries;
using BookMyHsrp.Libraries.GenerateOtp.Services;
using System.ComponentModel.DataAnnotations;
using BookMyHsrp.Controllers.CommonController;
using BookMyHsrp.ReportsLogics.Common;
using BookMyHsrp.Libraries.HomeDelivery.Services;
using BookMyHsrp.ReportsLogics.Sticker;
using BookMyHsrp.Libraries.Sticker.Services;
using BookMyHsrp.Libraries.HomeDeliverySticker.Services;
namespace BookMyHsrp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            ConfigureControllers(services);
            ConfigureSwagger(services);
            ConfigureDependencies(services);
            ConfigureAuthentication(services);
            services.AddSession(options =>
            {
                // Set options here
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        }

        private void ConfigureControllers(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            // Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Book My Hsrp",
                    Version = "v1"
                });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiExplorer = apiDesc.ActionDescriptor as ControllerActionDescriptor;

                    // Ignore actions that do not have any HttpMethod attribute
                    if (actionApiExplorer != null)
                    {
                        var attributes = actionApiExplorer.MethodInfo.GetCustomAttributes(true);
                        if (!attributes.OfType<HttpMethodAttribute>().Any())
                        {
                            return false;
                        }
                    }

                    return true;
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
           
        }
        private void ConfigureDependencies(IServiceCollection services)
        {
           
           
            services.Configure<ConnectionString>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<DynamicDataDto>(Configuration.GetSection("Api"));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("Redis");
            });
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<IHsrpWithColorStickerService, HsrpWithColorStickerService>();
            services.AddTransient<IExceptionHandler, ExceptionMiddleWare>();
            services.AddTransient<IGenerateOtpService, GenerateOtpService>();
            services.AddTransient<IStickerService, StickerService>();
            services.AddTransient<IHomeDeliveryStickerService, HomeDeliveryStickerService>();
            services.AddScoped<HsrpWithColorStickerConnector>();
            services.AddScoped<FileUploadConnector>();
            services.AddScoped<HsrpWithColorStickerService>();
            services.AddScoped<FetchDataAndCache, FetchDataAndCache>();
            services.AddScoped<HomeDeliveryService>();
            services.AddScoped<StickerConnector>();

            // services.AddSingleton<HSRP.Redis.ConnectionHelper>();

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            // services.AddSingleton(provider =>
            // {
            //     var connectionHelper = provider.GetRequiredService<ConnectionHelper>();
            //     var db = connectionHelper.Connection.GetDatabase();
            //     return new CacheService(db);
            // });
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });
            services.AddHttpClient();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddWebMarkupMin(options =>
            {
                options.AllowMinificationInDevelopmentEnvironment = true;
                options.AllowCompressionInDevelopmentEnvironment = true;
            }).AddHtmlMinification(options =>
            {
                options.MinificationSettings.RemoveHtmlComments = false;

            }).AddXmlMinification();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            // Authentication configuration
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
            })
                .AddJwtBearer("Bearer", options =>
                {
                    const string JwtKey = "JWT:Key";
                    const string JwtAudience = "JWT:Audience";
                    const string JwtIssuer = "JWT:Issuer";
                    var Key = Encoding.UTF8.GetBytes(Configuration[JwtKey]);
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration[JwtIssuer],
                        ValidAudience = Configuration[JwtAudience],
                        IssuerSigningKey = new SymmetricSecurityKey(Key)
                    };
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/login"; // The path to your custom unauthorized page
                    // options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            if (context.Request.Path.StartsWithSegments("/api") &&
                                context.Response.StatusCode == StatusCodes.Status200OK)
                            {
                                context.Response.Clear();
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return context.Response.WriteAsync("Unauthorized access.");
                            }
                            context.Response.Redirect(context.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        string authorization = context.Request.Headers[HeaderNames.Authorization];
                        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }

                        return CookieAuthenticationDefaults.AuthenticationScheme;
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureEnvironment(app, env);
            ConfigureExceptionHandler(app);
            ConfigureCommonMiddleware(app);
        }

        private void ConfigureEnvironment(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSession();
                app.UseSwaggerUI(c =>
                {
                    c.DisplayRequestDuration();
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DisplayRequestDuration();
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
                app.UseHsts();
            }
        }

        private void ConfigureExceptionHandler(IApplicationBuilder app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;
                    var exceptionHandler = appBuilder.ApplicationServices.GetRequiredService<BookMyHsrp.Libraries.ExceptionHandling.Services.IExceptionHandler>();

                    if (exception != null)
                        await exceptionHandler.TryHandleAsync(context, exception, CancellationToken.None);
                });
            });
        }

        private void ConfigureCommonMiddleware(IApplicationBuilder app)
        {

            // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseResponseCompression();
            // check Swagger authentication
            app.Use(async (context, next) =>
            {
                var path = context.Request.Path;
                if (path.Value.Contains("/swagger/", StringComparison.OrdinalIgnoreCase))
                {
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        //context.Response.StatusCode = 401;
                        //await context.Response.WriteAsync("Unauthorized");
                        context.Response.Redirect("/login");
                        return;
                    }
                }
                await next();
            });


            app.UseMiddleware<RequestResponseLoggingMiddleware.RequestResponseLoggingMiddleware>();

            app.UseWebMarkupMin();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}