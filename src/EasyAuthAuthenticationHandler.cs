using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MaximeRouiller.Azure.AppService.EasyAuth
{
    public class EasyAuthAuthenticationHandler : AuthenticationHandler<EasyAuthAuthenticationOptions>
    {
        public EasyAuthAuthenticationHandler(
            IOptionsMonitor<EasyAuthAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                bool easyAuthEnabled = String.Equals(Environment.GetEnvironmentVariable("WEBSITE_AUTH_ENABLED", EnvironmentVariableTarget.Process), "True", StringComparison.InvariantCultureIgnoreCase);
                if (!easyAuthEnabled) return AuthenticateResult.NoResult();
                
                string easyAuthProvider = Context.Request.Headers["X-MS-CLIENT-PRINCIPAL-IDP"].FirstOrDefault();
                string msClientPrincipalEncoded = Context.Request.Headers["X-MS-CLIENT-PRINCIPAL"].FirstOrDefault();
                if (String.IsNullOrWhiteSpace(msClientPrincipalEncoded)) return AuthenticateResult.NoResult();

                byte[] decodedBytes = Convert.FromBase64String(msClientPrincipalEncoded);
                string msClientPrincipalDecoded = System.Text.Encoding.Default.GetString(decodedBytes);
                MsClientPrincipal clientPrincipal = JsonConvert.DeserializeObject<MsClientPrincipal>(msClientPrincipalDecoded);

                ClaimsPrincipal principal = new ClaimsPrincipal();
                IEnumerable<Claim> standardClaims = clientPrincipal.Claims.Select(x => new Claim(x.Type, x.Value));
                IEnumerable<Claim> claims = await Options.Events.OnClaimsReceived(standardClaims);

                principal.AddIdentity(new ClaimsIdentity(claims, clientPrincipal.AuthenticationType, clientPrincipal.NameType, clientPrincipal.RoleType));
                
                AuthenticationTicket ticket = new AuthenticationTicket(principal, easyAuthProvider);
                AuthenticateResult success = AuthenticateResult.Success(ticket);
                Context.User = principal;

                return success;
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
