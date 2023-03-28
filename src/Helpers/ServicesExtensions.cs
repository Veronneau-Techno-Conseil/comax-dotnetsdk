using CommunAxiom.DotnetSdk.Helpers.Certificates;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunAxiom.DotnetSdk.Helpers
{
    public static class ServicesExtensions
    {
        public static IServiceCollection SetupHelpers(this IServiceCollection services)
        {
            services.AddTransient<ICertGenerator, CertGenerator>();
            return services.AddCertificateManager();
        }
    }
}
