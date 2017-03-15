// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using SwissSdr.Identity.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using SwissSdr.Identity.Services;
using IdentityServer4;
using Kentor.AuthServices;
using SwissSdr.Datamodel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using SwissSdr.Identity.Resources;

namespace SwissSdr.Identity.Controllers
{
	/// <summary>
	/// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
	/// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
	/// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
	/// </summary>
	public class AccountController : Controller
	{
		private readonly RavenLoginService _loginService;
		private readonly IIdentityServerInteractionService _interaction;
		private readonly IClientStore _clientStore;
		private readonly IStringLocalizer<LoginViewModel> _localizer;

		public AccountController(
			RavenLoginService loginService,
			IIdentityServerInteractionService interaction,
			IClientStore clientStore,
			IStringLocalizer<LoginViewModel> localizer)
		{
			_loginService = loginService;
			_interaction = interaction;
			_clientStore = clientStore;
			_localizer = localizer;
		}

		[HttpGet]
		public async Task<IActionResult> Status()
		{
			var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(HttpContext.Authentication.GetIdentityServerAuthenticationScheme());
			return Ok(info);
		}

		/// <summary>
		/// Show login page
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Login(string returnUrl)
		{
			var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
			if (context?.IdP != null)
			{
				// if IdP is passed, then bypass showing the login screen
				return ExternalLogin(context.IdP, returnUrl);
			}

			var vm = await BuildLoginViewModelAsync(returnUrl, context);
			return View(vm);
		}

		/// <summary>
		/// initiate roundtrip to external authentication provider
		/// </summary>
		[HttpGet]
		public IActionResult ExternalLogin(string provider, string returnUrl)
		{
			if (returnUrl != null)
			{
				returnUrl = UrlEncoder.Default.Encode(returnUrl);
			}
			returnUrl = "/account/externallogincallback?returnUrl=" + returnUrl;

			// start challenge and roundtrip the return URL
			var props = new AuthenticationProperties
			{
				RedirectUri = returnUrl,
				Items = { { "scheme", provider } }
			};
			return new ChallengeResult(provider, props);
		}

		/// <summary>
		/// Post processing of external authentication
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
		{
			var claims = await GetClaimsFromExternalAuthentication();
			var userIdClaim = claims.GetUserIdClaim();
			claims.Remove(userIdClaim);

			var provider = userIdClaim.Issuer;
			var userId = userIdClaim.Value;

			var user = await _loginService.Find(provider, userId);
			if (user == null)
			{
				// check if the user already has an authentication cookie with idsrv, if he has, this request adds a new login to an existing user
				var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(HttpContext.Authentication.GetIdentityServerAuthenticationScheme());
				if (info.Principal?.Identity?.IsAuthenticated == true)
				{
					user = await _loginService.AddExternalLogin(info.Principal.GetSubjectId(), provider, userId, claims);
				}
				else
				{
					var provisionalUser = await _loginService.CreateProvisional(provider, userId, claims);
					return Redirect(Url.Action<AccountController>(c => c.Register(returnUrl)));
				}
			}

			return await FinishExternalLogin(user, claims, returnUrl);
		}

		[HttpGet]
		public async Task<IActionResult> RemoveExternalLogin(string provider, string userId, string returnUrl)
		{
			if (User.Identity.IsAuthenticated == false)
			{
				throw new InvalidOperationException($"Need to be logged in to try to remove external login.");
			}

			var user = await _loginService.Find(User.GetSubjectId());

			var login = user.Logins.SingleOrDefault(l => l.Provider == provider && l.UserId == userId);
			if (login == null)
			{
				throw new InvalidOperationException($"No such login found on logged in user.");
			}

			if (!_interaction.IsValidReturnUrl(returnUrl))
			{
				returnUrl = "~/";
			}

			var vm = new RemoveExternalLoginViewModel()
			{
				Provider = provider,
				UserId = userId,
				ReturnUrl = returnUrl
			};
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveExternalLoginCallback(string returnUrl, RemoveExternalLoginViewModel vm)
		{
			if (User.Identity.IsAuthenticated == false)
			{
				throw new InvalidOperationException($"Need to be logged in to try to remove external login.");
			}

			await _loginService.RemoveExternalLogin(User.GetSubjectId(), vm.Provider, vm.UserId);

			if (_interaction.IsValidReturnUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return Redirect("~/");
		}

		[HttpGet]
		public async Task<IActionResult> Register(string returnUrl)
		{
			var claims = await GetClaimsFromExternalAuthentication();
			var userIdClaim = claims.GetUserIdClaim();

			var provider = userIdClaim.Issuer;
			var userId = userIdClaim.Value;

			var provisionalUser = await _loginService.FindProvisional(provider, userId);
			if (provisionalUser == null)
			{
				throw new Exception("No provisional user found");
			}

			var model = new RegisterViewModel()
			{
				Fullname = claims.GetFullname(),
				EMail = claims.GetEmail(),
				ReturnUrl = returnUrl
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(string returnUrl, RegisterViewModel model)
		{
			if (model == null)
			{
				return View("Error");
			}

			var claims = await GetClaimsFromExternalAuthentication();
			var userIdClaim = claims.GetUserIdClaim();
			var provider = userIdClaim.Issuer;
			var userId = userIdClaim.Value;

			var provisionalUser = await _loginService.FindProvisional(provider, userId);
			if (provisionalUser == null)
			{
				throw new Exception("No provisional user found");
			}

			if (ModelState.IsValid)
			{
				var user = await _loginService.ActivateProvisional(provisionalUser, model);
				return await FinishExternalLogin(user, claims, returnUrl);
			}

			return View(model);
		}

		private async Task<IActionResult> FinishExternalLogin(User user, List<Claim> claims, string returnUrl)
		{
			var authInfo = await HttpContext.Authentication.GetAuthenticateInfoAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
			var externalAuthScheme = authInfo.Properties.Items["scheme"];

			// if the external system sent a session id claim, copy it over
			var additionalClaims = new List<Claim>();
			var sessionIdClaim = CreateSessionIdClaim(claims);
			if (sessionIdClaim != null)
			{
				additionalClaims.Add(sessionIdClaim);
			}

			// issue authentication cookie for user
			await HttpContext.Authentication.SignInAsync(user.Id, user.Fullname, externalAuthScheme, additionalClaims.ToArray());

			// delete temporary cookie used during external authentication
			await HttpContext.Authentication.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

			// validate return URL and redirect back to authorization endpoint
			if (_interaction.IsValidReturnUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return Redirect("~/");
		}

		/// <summary>
		/// Show logout page
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Logout(string logoutId)
		{
			if (User.Identity.IsAuthenticated == false)
			{
				// if the user is not authenticated, then just show logged out page
				return await Logout(new LogoutViewModel { LogoutId = logoutId });
			}

			var context = await _interaction.GetLogoutContextAsync(logoutId);
			if (context?.ShowSignoutPrompt == false)
			{
				// it's safe to automatically sign-out
				return await Logout(new LogoutViewModel { LogoutId = logoutId });
			}

			// show the logout prompt. this prevents attacks where the user
			// is automatically signed out by another malicious web page.
			var vm = new LogoutViewModel
			{
				LogoutId = logoutId
			};

			return View(vm);
		}

		/// <summary>
		/// Handle logout page postback
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout(LogoutViewModel model)
		{
			try
			{
				// hack to retrieve and set kentor auth services claims to enable logout
				var user = await _loginService.Find(User.GetSubjectId());
				if (user != null)
				{
					var claimsIdentity = (ClaimsIdentity)User.Identity;

					var sessionIndex = user.ExternalClaims.SingleOrDefault(x => x.Type == AuthServicesClaimTypes.SessionIndex);
					if (sessionIndex != null)
					{
						claimsIdentity.AddClaim(sessionIndex.ToClaim());
					}
					var logoutNameIdentifier = user.ExternalClaims.SingleOrDefault(x => x.Type == AuthServicesClaimTypes.LogoutNameIdentifier);
					if (logoutNameIdentifier != null)
					{
						claimsIdentity.AddClaim(logoutNameIdentifier.ToClaim());
					}
				}
			}
			catch (Exception) { }

			await LogoutFromExternalIdp(model.LogoutId);

			// delete authentication cookie
			await HttpContext.Authentication.SignOutAsync();

			// set this so UI rendering sees an anonymous user
			HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

			// get context information (client name, post logout redirect URI and iframe for federated signout)
			var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

			var vm = new LoggedOutViewModel
			{
				PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
				ClientName = logout?.ClientId,
				SignOutIframeUrl = logout?.SignOutIFrameUrl
			};

			return View("LoggedOut", vm);
		}

		private async Task LogoutFromExternalIdp(string logoutId)
		{
			var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
			if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
			{
				// idp is passed through from saml2, but logout needs to use local KentorAuthServices
				var viaIdp = User?.FindFirst(IdentityConstants.ClaimTypes.AuthenticateVia)?.Value;
				if (viaIdp != null)
				{
					idp = viaIdp;
				}

				if (logoutId == null)
				{
					// if there's no current logout context, we need to create one
					// this captures necessary info from the current logged in user
					// before we signout and redirect away to the external IdP for signout
					logoutId = await _interaction.CreateLogoutContextAsync();
				}

				string url = "/Account/Logout?logoutId=" + logoutId;
				try
				{
					// hack: try/catch to handle social providers that throw
					await HttpContext.Authentication.SignOutAsync(idp, new AuthenticationProperties { RedirectUri = url });
				}
				catch (NotSupportedException)
				{
				}
			}
		}

		private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
		{
			var providers = HttpContext.Authentication.GetAuthenticationSchemes()
				.Where(x => x.DisplayName != null)
				.Select(x => new ExternalProvider
				{
					Type = x.AuthenticationScheme == "KentorAuthServices" ? LoginType.SwitchAai : LoginType.Social,
					DisplayName = x.DisplayName,
					AuthenticationScheme = x.AuthenticationScheme
				});

			if (context?.ClientId != null)
			{
				var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
				if (client != null)
				{
					if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
					{
						providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme));
					}
				}
			}

			return new LoginViewModel
			{
				ReturnUrl = returnUrl,
				Username = context?.LoginHint,
				ExternalProviders = providers.ToArray()
			};
		}

		private async Task<List<Claim>> GetClaimsFromExternalAuthentication()
		{
			var authInfo = await HttpContext.Authentication.GetAuthenticateInfoAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
			if (authInfo?.Principal == null)
			{
				throw new Exception("External authentication error");
			}

			return authInfo.Principal.Claims.ToList();
		}

		private Claim CreateSessionIdClaim(List<Claim> claims)
		{
			var sid = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
			if (sid == null)
			{
				sid = claims.FirstOrDefault(x => x.Type == AuthServicesClaimTypes.SessionIndex);
			}
			if (sid != null)
			{
				return new Claim(JwtClaimTypes.SessionId, sid.Value);
			}

			return null;
		}
	}
}
