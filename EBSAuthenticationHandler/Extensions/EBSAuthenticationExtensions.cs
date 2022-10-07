using System;
using EBSAuthenticationHandler.Defaults;
using EBSAuthenticationHandler.Handlers;
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

        private static IServiceCollection AddTokenRefreshService(this IServiceCollection services, Action<EBSAuthenticationSchemeOptions> options)
        {
            EBSAuthenticationSchemeOptions configuredOptions = new();
            options(configuredOptions);

            return services.AddScoped<ITokenRefreshService, TokenRefreshService>(sp =>
            {
                return new TokenRefreshService(
                    sp.GetRequiredService<ILogger<TokenRefreshService>>(),
                    configuredOptions.AuthUrl ?? configuredOptions.AuthApiUrl,
                    configuredOptions.ApiKey,
                    configuredOptions.TOkenExpirationInSeconds);
            });
        }

        private static IServiceCollection AddUserAuthService(this IServiceCollection services, Action<EBSAuthenticationSchemeOptions> options)
        {
            EBSAuthenticationSchemeOptions configuredOptions = new();
            options(configuredOptions);

            return services.AddScoped<IUserAuthService, UserAuthService>(sp =>
            {
                return new UserAuthService(
                    sp.GetRequiredService<ILogger<UserAuthService>>(),
                    configuredOptions.AuthApiUrl,
                    configuredOptions.ApiKey
                    );
            });
        }

        public static AuthenticationBuilder AddEBSAuthentication(
            this IServiceCollection services,
            Action<EBSAuthenticationSchemeOptions> ebsauthOptions)
        {
            return services
                .AddTokenRefreshService(ebsauthOptions)
                .AddUserAuthService(ebsauthOptions)
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
                    options.SlidingExpiration = true;
                })
                .AddRemoteScheme<EBSAuthenticationSchemeOptions, EBSAuthHandler>(
                    EBSAuthenticationDefaults.AuthenticationScheme,
                    null,
                    ebsauthOptions);
        }

        public static AuthenticationBuilder AddEBSAuthentication(
            this IServiceCollection services,
            Action<CookieAuthenticationOptions> cookieOptions,
            Action<EBSAuthenticationSchemeOptions> ebsauthOptions)
        {
            return services
                .AddTokenRefreshService(ebsauthOptions)
                .AddUserAuthService(ebsauthOptions)
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = EBSAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(cookieOptions)
                .AddRemoteScheme<EBSAuthenticationSchemeOptions, EBSAuthHandler>(
                    EBSAuthenticationDefaults.AuthenticationScheme,
                    null,
                    ebsauthOptions);
        }


        public static AuthenticationBuilder AddEBSAuthentication(
            this IServiceCollection services,
            Action<AuthenticationOptions> authOptions,
            Action<CookieAuthenticationOptions> cookieOptions,
            Action<EBSAuthenticationSchemeOptions> ebsauthOptions)
        {
            return services
                .AddTokenRefreshService(ebsauthOptions)
                .AddUserAuthService(ebsauthOptions)
                .AddAuthentication(authOptions)
                .AddCookie(cookieOptions)
                .AddRemoteScheme<EBSAuthenticationSchemeOptions, EBSAuthHandler>(
                    EBSAuthenticationDefaults.AuthenticationScheme,
                    null,
                    ebsauthOptions);
        }
    }
}

