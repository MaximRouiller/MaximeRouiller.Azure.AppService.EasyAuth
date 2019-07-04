using Microsoft.AspNetCore.Authentication;

namespace MaximeRouiller.Azure.AppService.EasyAuth
{
    public class EasyAuthAuthenticationOptions : AuthenticationSchemeOptions
    {
        public EasyAuthAuthenticationOptions()
        {
            Events = new object();
        }
    }
}