using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace SwissSdr.Identity.Configuration
{
	internal class Clients
	{
		internal static IEnumerable<Client> Get()
		{
			return new List<Client>
			{
				new Client
				{
					ClientId = "56c38386-b84a-4b8c-8f1b-5ac37f31903a",
					ClientName = "Swiss SDR Frontend App",
					RequireConsent = false,
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,

					AllowedCorsOrigins =
					{
						"https://app.swiss-sdr.ch",
						"https://swisssdr-development.azurewebsites.net",
						"https://swisssdr.novu.io:3000"
					},

					RedirectUris =
					{
						"https://app.swiss-sdr.ch",
						"https://app.swiss-sdr.ch/app/authRedirectUri.html",
						"https://app.swiss-sdr.ch/app/authSilentRedirectUri.html",
						"https://swisssdr-development.azurewebsites.net",
						"https://swisssdr-development.azurewebsites.net/app/authRedirectUri.html",
						"https://swisssdr-development.azurewebsites.net/app/authSilentRedirectUri.html",
						"https://swisssdr.novu.io:3000",
						"https://swisssdr.novu.io:3000/app/authRedirectUri.html",
						"https://swisssdr.novu.io:3000/app/authSilentRedirectUri.html"
					},

					PostLogoutRedirectUris =
					{
						"https://app.swiss-sdr.ch/app/authPostLogoutRedirectUri.html",
						"https://swisssdr-development.azurewebsites.net/app/authPostLogoutRedirectUri.html",
						"https://swisssdr.novu.io:3000/app/authPostLogoutRedirectUri.html"
					},

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						IdentityConstants.Scopes.SwissSdrApi
					}
				},
				new Client()
				{
					ClientId = "swaggerui",
					ClientName = "api.swiss-sdr.ch Swagger UI",
					RequireConsent = false,
					AllowAccessTokensViaBrowser = true,
					AllowedGrantTypes = GrantTypes.Implicit,

					RedirectUris =
					{
						"http://localhost:5000/swagger/o2c.html",
						"https://localhost:44356/swagger/o2c.html",
						"https://swisssdr-api.azurewebsites.net/swagger/o2c.html",
						"https://swisssdr-api-development.azurewebsites.net/swagger/o2c.html"
					},

					AllowedScopes = 
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						IdentityConstants.Scopes.SwissSdrApi
					}
				},
				new Client
				{
					ClientId = "testclient",
					ClientName ="Identity Testclient",
					RequireConsent = false,
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,

					RedirectUris =
					{
						"http://localhost:52046",
						"http://localhost:52046/popup.html",
						"http://localhost:52046/redirect.html",
						"http://localhost:52046/silentredirect.html",
						"http://localhost:52046/renew.html"
					},
					AllowedCorsOrigins =
					{
						"http://localhost:52046"
					},
					PostLogoutRedirectUris =
					{
						"http://localhost:52046",
						"http://localhost:52046/signout.html"
					},
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						IdentityConstants.Scopes.SwissSdrApi
					}
				}
			};
		}
	}
}
