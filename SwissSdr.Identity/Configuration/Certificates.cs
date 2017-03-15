using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace SwissSdr.Identity.Configuration
{
	internal class Certificates
	{

		internal static X509Certificate2 GetSigningCertificate()
		{
			var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);

			var certs = store.Certificates.Find(X509FindType.FindByThumbprint, "4DDDA0BDD54984BD333677B37D95BA307998A5E4", false);
			return certs[0];
		}
	}
}