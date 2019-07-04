using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaximeRouiller.Azure.AppService.EasyAuth
{
    public static class EasyAuthAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddEasyAuthAuthentication(
            this AuthenticationBuilder builder,
            Action<EasyAuthAuthenticationOptions> configure) =>
                builder.AddScheme<EasyAuthAuthenticationOptions, EasyAuthAuthenticationHandler>(
                    "EasyAuth",
                    "EasyAuth",
                    configure);
    }
}
