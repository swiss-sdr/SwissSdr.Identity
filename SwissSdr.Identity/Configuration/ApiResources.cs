using IdentityServer4.Models;
using SwissSdr.Datamodel;
using System.Collections.Generic;

namespace SwissSdr.Identity.Configuration
{
	public class ApiResources
	{
		public static IEnumerable<ApiResource> Get()
		{
			var swisssdrApi = new ApiResource()
			{
				Name = "swisssdr-api",
				DisplayName = "Swiss SDR API",
				UserClaims = new[] {
					ClaimTypes.CreateEntityOfType,
					ClaimTypes.BypassObjectPermissions,
					ClaimTypes.AdministerUsers
				},
				Scopes =
				{
					new Scope()
					{
						Name = IdentityConstants.Scopes.SwissSdrApi,
						DisplayName = "Swiss SDR API Access"
					}
				}
			};

			return new[] { swisssdrApi };
		}
	}
}
