using System.Security.Claims;
using Booking.Authorization;
using Booking.Components;
using Booking.Data;
using Booking.Services;
using Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning() // Log Warnings and above
                .WriteTo.File(
                    path: Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"),
                    rollingInterval: RollingInterval.Day,      // New file each day
                    retainedFileCountLimit: 14,               // Keep logs for 14 days
                    fileSizeLimitBytes: 10_000_000,           // 10 MB max per file
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            // Use Serilog
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents(options => options.DetailedErrors = true)
                .AddInteractiveWebAssemblyComponents();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContextFactory<BookingDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            // Authentication setup
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/accessdenied";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://api.ivao.aero";
                options.ProtocolValidator.RequireNonce = true;
                options.UsePkce = true;
                options.ClientId = builder.Configuration["Authentication:OIDC:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:OIDC:ClientSecret"];
                options.ResponseType = "code";
                options.SaveTokens = false;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                // Add other scopes as needed

                options.GetClaimsFromUserInfoEndpoint = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = ClaimTypes.Role,
                    ValidAlgorithms = [SecurityAlgorithms.RsaSha512]
                };

                options.CallbackPath = "/signin-oidc";
                options.SignedOutCallbackPath = "/signout-callback-oidc";

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var userId = ctx.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId is not null && ctx.TokenEndpointResponse is not null)
                        {
                            var expiresInString = ctx.TokenEndpointResponse.ExpiresIn;
                            DateTime expiresAt;

                            if (int.TryParse(expiresInString, out int expiresInSeconds))
                            {
                                expiresAt = DateTime.UtcNow.AddSeconds(expiresInSeconds);
                            }
                            else
                            {
                                expiresAt = DateTime.UtcNow.AddHours(1);
                            }

                            var tokenCacheService = ctx.HttpContext.RequestServices.GetRequiredService<ITokenCacheService>();
                            await tokenCacheService.StoreTokensAsync(userId, new TokenData()
                            {
                                AccessToken = ctx.TokenEndpointResponse.AccessToken,
                                RefreshToken = ctx.TokenEndpointResponse.RefreshToken,
                                ExpiresAt = expiresAt
                            });
                        }
                    }
                };
            });

            // Authorization
            builder.Services.AddAuthorizationBuilder().AddPolicy("Administrator", policy => policy.Requirements.Add(new AdministratorRequirement()));
            
            builder.Services.AddSingleton<ITokenCacheService, InMemoryTokenCacheService>();
            builder.Services.AddScoped<IAdministratorService, DbAdministratorService>();
            builder.Services.AddScoped<IEventService, DbEventService>();
            builder.Services.AddScoped<IAtcPositionService, DbAtcPositionService>();
            builder.Services.AddScoped<IEventAtcPositionService, DbEventAtcPositionService>();
            builder.Services.AddScoped<IAtcPositionBookingService, DbAtcPositionBookingService>();
            builder.Services.AddScoped<OidcConfigurationService>();
            builder.Services.AddScoped<HttpClient>();
            builder.Services.AddScoped<IvaoUserService>();

            // Register handler for Administrator protection
            builder.Services.AddScoped<IAuthorizationHandler, AdministratorAuthorizationHandler>();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();

            app.UseAntiforgery();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
            ;
            app.MapGet("/login", async context =>
            {
                var returnUrl = context.Request.Query["returnUrl"].FirstOrDefault() ?? "/";

                if (!returnUrl.StartsWith('/'))
                {
                    returnUrl = "/";
                }
                await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                });
            });
            app.MapGet("/logout", async context =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    RedirectUri = "/" // Redirect after logout
                });
            });
            app.MapGet("/accessdenied", async context =>
            {
                await context.Response.WriteAsync("Access Denied: You do not have permission to view this page.");
            });
            using (var scope = app.Services.CreateScope())
            {
                var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BookingDbContext>>();
                using var dbContext = dbFactory.CreateDbContext();
                dbContext.Database.Migrate();
            }
            app.Run();
        }
    }
}
