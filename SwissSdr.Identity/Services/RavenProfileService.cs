using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel;

namespace SwissSdr.Identity.Services
{
	public class RavenProfileService : IProfileService
	{
		private readonly RavenLoginService _loginService;

		public RavenProfileService(RavenLoginService loginService)
		{
			_loginService = loginService;
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var user = await _loginService.Find(context.Subject.GetSubjectId());

			if (user != null)
			{
				context.AddFilteredClaims(user.Claims.ToClaims());

				context.IssuedClaims.Add(new Claim(JwtClaimTypes.Name, user.Fullname));
				context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.EMail));
				context.IssuedClaims.Add(new Claim(JwtClaimTypes.Gender, user.Gender.ToString()));

				if (!string.IsNullOrEmpty(user.Title))
				{
					context.IssuedClaims.Add(new Claim("title", user.Title));
				}

				if (!string.IsNullOrEmpty(user.ProfileImageUrl))
				{
					context.IssuedClaims.Add(new Claim("profileImage", user.ProfileImageUrl));
				}
			}
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var user = await _loginService.Find(context.Subject.GetSubjectId());
			context.IsActive = user != null;
		}
	}
}
