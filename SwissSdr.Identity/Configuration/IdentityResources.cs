using IdentityServer4.Models;
using SwissSdr.Datamodel;
using System.Collections.Generic;

namespace SwissSdr.Identity.Configuration
{
	public class IdentityResources
	{

		public static IEnumerable<IdentityResource> Get()
		{
			var swisssdrIdentity = new IdentityResource()
			{
				Name = "swisssdr",
				DisplayName = "Swiss SDR",
				UserClaims = new[] {
					ClaimTypes.CreateEntityOfType,
					ClaimTypes.BypassObjectPermissions,
					ClaimTypes.AdministerUsers
				}
			};

			return new[]
			{
				new IdentityServer4.Models.IdentityResources.OpenId(),
				new IdentityServer4.Models.IdentityResources.Email(),
				new IdentityServer4.Models.IdentityResources.Profile(),
				swisssdrIdentity
			};
		}
	}
}
