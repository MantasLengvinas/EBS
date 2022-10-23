using System;
using EBSAuthenticationHandler.Defaults;
using EBSAuthenticationHandler.Events;
using EBSAuthenticationHandler.Options;
using EBSAuthenticationHandler.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EBSAuthenticationHandler.Extensions
{
    public static class EBSAuthenticationExtension
    {

        private static IServiceCollection AddUserAuthService(this IServiceCollection services, Action<EBSAuthenticationSchemeOptions> options)
        {
            EBSAuthenticationSchemeOptions configuredOptions = new();
            options(configuredOptions);

            return services.AddScoped<IUserAuthService, UserAuthService>(sp =>
            {
                return new UserAuthService(
                    sp.GetRequiredService<ILogger<UserAuthService>>(),
                    configuredOptions
                    );
            });
        }

        public static AuthenticationBuilder AddEBSAuthentication(
            this IServiceCollection services,
            Action<EBSAuthenticationSchemeOptions> ebsauthOptions)
        {
            return services
                .AddUserAuthService(ebsauthOptions)
                .AddTransient<EBSAuthCookieEvents>()
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = EBSAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                    options.SlidingExpiration = false;
                    options.EventsType = typeof(EBSAuthCookieEvents);
                });
        }

        public static AuthenticationBuilder AddEBSAuthentication(
            this IServiceCollection services,
            Action<CookieAuthenticationOptions> cookieOptions,
            Action<EBSAuthenticationSchemeOptions> ebsauthOptions)
        {
            return services
                .AddUserAuthService(ebsauthOptions)
                .AddTransient<EBSAuthCookieEvents>()
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = EBSAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(cookieOptions);
        }


        public static AuthenticationBuilder AddEBSAuthentication(
            this IServiceCollection services,
            Action<AuthenticationOptions> authOptions,
            Action<CookieAuthenticationOptions> cookieOptions,
            Action<EBSAuthenticationSchemeOptions> ebsauthOptions)
        {
            return services
                .AddUserAuthService(ebsauthOptions)
                .AddTransient<EBSAuthCookieEvents>()
                .AddAuthentication(authOptions)
                .AddCookie(cookieOptions);
        }
    }
}

