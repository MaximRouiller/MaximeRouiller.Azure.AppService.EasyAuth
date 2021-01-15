using Microsoft.AspNetCore.Authentication;

namespace MaximeRouiller.Azure.AppService.EasyAuth
{
    public class EasyAuthAuthenticationOptions : AuthenticationSchemeOptions
    {
        public new EasyAuthEvents Events
        {
            get => (EasyAuthEvents)base.Events;
            set => base.Events = value;
        }

        public EasyAuthAuthenticationOptions()
        {
            Events = new EasyAuthEvents();
        }
    }
}