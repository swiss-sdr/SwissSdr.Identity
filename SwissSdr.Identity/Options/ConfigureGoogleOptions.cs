using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Options
{
	public class ConfigureGoogleOptions : IConfigureOptions<GoogleOptions>
	{
		private readonly IOptions<IdentityProviderOptions> _optionsAccessor;

		public ConfigureGoogleOptions(IOptions<IdentityProviderOptions> optionsAccessor)
		{
			_optionsAccessor = optionsAccessor;
		}

		public void Configure(GoogleOptions options)
		{
			options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

			options.ClientId = "400131811515-8tf0c4ou250vac7h3se4v2am3acibdp6.apps.googleusercontent.com";
			options.ClientSecret = _optionsAccessor.Value.GoogleClientSecret;

			options.Scope.Add("openid");
			options.Scope.Add("profile");
			options.Scope.Add("email");
		}
	}
}
