using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Options
{
    public class IdentityProviderOptions
    {
		public const string Name = "IdentityProviders";

		public string FacebookAppSecret { get; set; }
		public string GoogleClientSecret { get; set; }
	}
}
