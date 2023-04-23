using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;

namespace ShipperService.Infrastructure.DependencyRegistrations
{
    public static class IdentityServerAuthRegistrator
    {
        public static void AddIdentityServer(
            this IServiceCollection services,
            IHostEnvironment env,
            string scheme,
            string identityOrigin)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddAuthentication(scheme)
            .AddJwtBearer(scheme, options =>
            {
                options.Authority = identityOrigin;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
                options.BackchannelHttpHandler =
                    env.IsDevelopment() ?
                    new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = delegate { return true; }
                    }
                    : null;
            });
        }
    }
}
