using IdentityModel;
using Kentor.AuthServices.AspNetCore;
using System.Linq;
using System.Security.Claims;

namespace SwissSdr.Identity
{
	internal class SwitchAaiClaimsAuthenticationManager : ClaimsAuthenticationManager
	{
		public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
		{
			var principal = base.Authenticate(resourceName, incomingPrincipal);

			var claimsIdentity = principal.Identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				claimsIdentity.AddClaim(new Claim(IdentityConstants.ClaimTypes.AuthenticateVia, KentorAuthServicesDefaults.DefaultAuthenticationScheme));
			}

			return principal;
		}
	}
}
