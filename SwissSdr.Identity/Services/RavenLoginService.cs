using IdentityModel;
using Kentor.AuthServices.AspNetCore;
using Raven.Client;
using SwissSdr.Identity.Datamodel;
using SwissSdr.Identity.Models;
using SwissSdr.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityServer4.Services;
using IdentityServer4.Models;
using IdentityServer4.Extensions;
using SwissSdr.Datamodel;

namespace SwissSdr.Identity.Services
{
	public class RavenLoginService
	{
		private readonly IAsyncDocumentSession _session;

		public RavenLoginService(IAsyncDocumentSession session)
		{
			_session = session;
		}

		public async Task<User> Find(string userId)
		{
			return await _session.LoadAsync<User>(userId);
		}
		public async Task<User> Find(string provider, string userId)
		{
			return await _session.Query<User>()
				.Customize(opt => opt.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(2)))
				.SingleOrDefaultAsync(u => u.Logins.Any(i => i.Provider == provider && i.UserId == userId));
		}
		public async Task<ProvisionalUser> FindProvisional(string provider, string userId)
		{
			return await _session.LoadAsync<ProvisionalUser>(ProvisionalUser.CreateId(provider, userId));
		}

		public async Task<User> ActivateProvisional(ProvisionalUser provisionalUser, RegisterInputModel model)
		{
			var user = new User()
			{
				Gender = model.Gender,
				Title = model.Title,
				Fullname = model.Fullname,
				EMail = model.EMail
			};

			user.Logins.Add(provisionalUser.Login);
			user.ExternalClaims = provisionalUser.Claims;
			AddDefaultCreateClaims(user);

			_session.Delete(provisionalUser);
			await _session.StoreAsync(user);
			await _session.SaveChangesAsync();

			return user;
		}

		public async Task<ProvisionalUser> CreateProvisional(string provider, string userId, IEnumerable<Claim> claims)
		{
			var provisionalUser = await FindProvisional(provider, userId);
			if (provisionalUser == null)
			{
				provisionalUser = new ProvisionalUser();
			}

			provisionalUser.Claims = claims.ToRavenClaims().ToList();
			provisionalUser.Login = new UserLogin()
			{
				Provider = provider,
				UserId = userId,
				AuthenticateVia = claims.GetClaimValue(IdentityConstants.ClaimTypes.AuthenticateVia),
				Email = claims.GetEmail()
			};

			await _session.StoreAsync(provisionalUser);
			await _session.SaveChangesAsync();

			return provisionalUser;
		}

		public async Task<User> AddExternalLogin(string subject, string provider, string userId, IEnumerable<Claim> claims)
		{
			var user = await _session.LoadAsync<User>(subject);
			if (user == null)
			{
				throw new InvalidOperationException($"Could not find user with id '{subject}'.");
			}

			if (user.Logins.Any(l => l.Provider == provider && l.UserId == userId))
			{
				throw new InvalidOperationException($"User '{subject}' already has a login for provider '{provider}' and userId '{userId}'.");
			}

			// add additional claims from external idp
			foreach (var claim in claims)
			{
				user.ExternalClaims.Add(claim.ToRavenClaim());
			}

			// create login and add any additional initial permissions that may come with the new auth method
			user.Logins.Add(new UserLogin()
			{
				Provider = provider,
				UserId = userId,
				AuthenticateVia = claims.GetClaimValue(IdentityConstants.ClaimTypes.AuthenticateVia),
				Email = claims.GetEmail()
			});
			AddDefaultCreateClaims(user);

			await _session.SaveChangesAsync();
			return user;
		}

		public async Task<User> RemoveExternalLogin(string subject, string provider, string userId)
		{
			var user = await _session.LoadAsync<User>(subject);
			if (user == null)
			{
				throw new InvalidOperationException($"Could not find user with id '{subject}'.");
			}

			var login = user.Logins.SingleOrDefault(l => l.Provider == provider && l.UserId == userId);
			if (login == null)
			{
				throw new InvalidOperationException($"No login with provider '{provider}' and userId '{userId}' found on user '{user.Id}'.");
			}

			if (user.Logins.Count <= 1)
			{
				throw new InvalidOperationException($"Can't delete the last external login, user would be unable to login afterwards.");
			}

			foreach (var claim in user.ExternalClaims.Where(c => c.Issuer == provider).ToList())
			{
				user.ExternalClaims.Remove(claim);
			}

			user.Logins.Remove(login);
			await _session.SaveChangesAsync();

			return user;
		}

		private void AddDefaultCreateClaims(User user)
		{
			// only users with switch aai get create permissions
			if (user.ExternalClaims.Any(c => c.Type == IdentityConstants.ClaimTypes.AuthenticateVia && c.Value == KentorAuthServicesDefaults.DefaultAuthenticationScheme))
			{
				var createObjectClaimValues = user.Claims.Where(c => c.Type == SwissSdr.Datamodel.ClaimTypes.CreateEntityOfType).Select(c => c.Value).ToList();

				if (!createObjectClaimValues.Contains(EntityTypeNames.Person))
				{
					user.Claims.Add(new RavenClaim()
					{
						Issuer = IdentityConstants.Authority,
						Type = SwissSdr.Datamodel.ClaimTypes.CreateEntityOfType,
						Value = EntityTypeNames.Person
					});
				}

				if (!createObjectClaimValues.Contains(EntityTypeNames.Project))
				{
					user.Claims.Add(new RavenClaim()
					{
						Issuer = IdentityConstants.Authority,
						Type = SwissSdr.Datamodel.ClaimTypes.CreateEntityOfType,
						Value = EntityTypeNames.Project
					});
				}

				if (!createObjectClaimValues.Contains(EntityTypeNames.Event))
				{
					user.Claims.Add(new RavenClaim()
					{
						Issuer = IdentityConstants.Authority,
						Type = SwissSdr.Datamodel.ClaimTypes.CreateEntityOfType,
						Value = EntityTypeNames.Event
					});
				}
			}
		}
	}
}
