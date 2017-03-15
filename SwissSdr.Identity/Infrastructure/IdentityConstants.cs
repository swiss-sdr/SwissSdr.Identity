using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwissSdr.Identity
{
	public static class IdentityConstants
	{
		public const string Authority = "https://accounts.swiss-sdr.ch";

		public static class ClaimTypes
		{
			public const string AuthenticateVia = "AuthenticateVia";
		}

		public static class Scopes
		{
			public const string SwissSdrApi = "swisssdr-api";
		}
	}
}
