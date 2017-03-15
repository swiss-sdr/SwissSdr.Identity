using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Web;
using SwissSdr.Identity.Models;
using SwissSdr.Identity.Datamodel;
using IdentityModel;
using SwissSdr.Datamodel;

namespace SwissSdr.Identity
{
	public static class ClaimExtensions
	{
		public static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
		{
			return claims.FirstOrDefault(c => c.Type == claimType)?.Value;
		}

		public static bool HasClaim(this IEnumerable<Claim> claims, string claimType)
		{
			return claims.Any(c => c.Type == claimType);
		}

		public static bool HasNonEmptyClaim(this IEnumerable<Claim> claims, string claimType)
		{
			return claims.Any(c => c.Type == claimType && !string.IsNullOrEmpty(c.Value));
		}

		public static Claim ToClaim(this RavenClaim claim)
		{
			return new Claim(claim.Type, claim.Value, null, claim.Issuer);
		}
		public static RavenClaim ToRavenClaim(this Claim claim)
		{
			return new RavenClaim()
			{
				Issuer = claim.Issuer,
				Type = claim.Type,
				Value = claim.Value
			};
		}

		public static IEnumerable<Claim> ToClaims(this IEnumerable<RavenClaim> ravenClaims)
		{
			return ravenClaims.Select(rc => rc.ToClaim());
		}
		public static IEnumerable<RavenClaim> ToRavenClaims(this IEnumerable<Claim> claims)
		{
			return claims.Select(c => c.ToRavenClaim());
		}

		public static string GetFullname(this ClaimsPrincipal claimsPrincipal)
		{
			return claimsPrincipal.Claims.GetFullname();
		}
		public static string GetFullname(this IEnumerable<Claim> claims)
		{
			if (claims.Any(c => c.Type == SwitchAaiAttributes.Core.GivenName) && claims.Any(c => c.Type == SwitchAaiAttributes.Core.Surname))
			{
				return $"{claims.GetClaimValue(SwitchAaiAttributes.Core.GivenName)} {claims.GetClaimValue(SwitchAaiAttributes.Core.Surname)}";
			}

			if (claims.HasNonEmptyClaim(JwtClaimTypes.Name))
			{
				return claims.GetClaimValue(JwtClaimTypes.Name);
			}

			if (claims.HasNonEmptyClaim(System.IdentityModel.Claims.ClaimTypes.Name))
			{
				return claims.GetClaimValue(System.IdentityModel.Claims.ClaimTypes.Name);
			}

			if (claims.HasNonEmptyClaim(JwtClaimTypes.GivenName) && claims.HasNonEmptyClaim(JwtClaimTypes.FamilyName))
			{
				return $"{claims.GetClaimValue(JwtClaimTypes.GivenName)} {claims.GetClaimValue(JwtClaimTypes.FamilyName)}";
			}

			if (claims.HasNonEmptyClaim(System.IdentityModel.Claims.ClaimTypes.GivenName) && claims.HasNonEmptyClaim(System.IdentityModel.Claims.ClaimTypes.Surname))
			{
				return $"{claims.GetClaimValue(System.IdentityModel.Claims.ClaimTypes.GivenName)} {claims.GetClaimValue(System.IdentityModel.Claims.ClaimTypes.Surname)}";
			}

			return null;
		}

		public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
		{
			return claimsPrincipal.Claims.GetEmail();
		}
		public static string GetEmail(this IEnumerable<Claim> claims)
		{
			var emailClaim = claims.FirstOrDefault(c => c.Type == SwitchAaiAttributes.Core.EMail);
			if (emailClaim == null)
			{
				emailClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email);
			}
			if (emailClaim == null)
			{
				emailClaim = claims.FirstOrDefault(c => c.Type == System.IdentityModel.Claims.ClaimTypes.Email);
			}

			return emailClaim?.Value;
		}

		public static Claim GetUserIdClaim(this IEnumerable<Claim> claims)
		{
			var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
			if (userIdClaim == null)
			{
				userIdClaim = claims.FirstOrDefault(x => x.Type == System.IdentityModel.Claims.ClaimTypes.NameIdentifier);
			}

			if (userIdClaim == null)
			{
				throw new Exception("Unknown userid");
			}

			return userIdClaim;
		}
	}
}
