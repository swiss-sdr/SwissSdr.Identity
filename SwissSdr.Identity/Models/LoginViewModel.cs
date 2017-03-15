// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;

namespace SwissSdr.Identity.Models
{
    public class LoginViewModel : LoginInputModel
    {
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
    }

    public class ExternalProvider
    {
		public LoginType Type { get; set; }
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
    }

	public enum LoginType
	{
		SwitchAai,
		Social
	}
}