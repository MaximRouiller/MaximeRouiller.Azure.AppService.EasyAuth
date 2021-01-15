using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MaximeRouiller.Azure.AppService.EasyAuth
{
    public class EasyAuthEvents
    {
        /// <summary>
        /// Invoked after the EasyAuth claims have been unwrapped
        /// </summary>
        public Func<IEnumerable<Claim>, Task<IEnumerable<Claim>>> OnClaimsReceived { get; set; } = claims => Task.FromResult(claims);
    }
}
