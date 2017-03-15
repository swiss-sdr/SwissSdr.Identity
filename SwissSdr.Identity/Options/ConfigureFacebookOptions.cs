using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Options
{
	public class ConfigureFacebookOptions : IConfigureOptions<FacebookOptions>
	{
		private readonly IOptions<IdentityProviderOptions> _optionsAccessor;

		public ConfigureFacebookOptions(IOptions<IdentityProviderOptions> optionsAccessor)
		{
			_optionsAccessor = optionsAccessor;
		}

		public void Configure(FacebookOptions options)
		{
			options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

			options.AppId = "310661252650819";
			options.AppSecret = _optionsAccessor.Value.FacebookAppSecret;

			options.Scope.Add("public_profile");
			options.Scope.Add("email");
		}
	}
}
