using IdentityServer4;
using Kentor.AuthServices;
using Kentor.AuthServices.AspNetCore;
using Kentor.AuthServices.Configuration;
using Kentor.AuthServices.Saml2P;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Options
{
	public class ConfigureKentorAuthServicesOptions : IConfigureOptions<KentorAuthServicesOptions>
	{
		public void Configure(KentorAuthServicesOptions options)
		{
			var spOptions = new SPOptions();
			spOptions.EntityId = new EntityId("https://accounts.swiss-sdr.ch/shibboleth");
			spOptions.ServiceCertificates.Add(GetCertificate("A864EB32718B6A100325ACDC1196D10F187AB3DB"));
			spOptions.ServiceCertificates.Add(GetCertificate("87291795F51C054F0A5203CC0A816A21B6253310"));
			spOptions.NameIdPolicy = new Saml2NameIdPolicy(true, NameIdFormat.Persistent);
			spOptions.SystemIdentityModelIdentityConfiguration.ClaimsAuthenticationManager = new SwitchAaiClaimsAuthenticationManager();

			spOptions.Contacts.AddAdministrative("Hansjörg", "Lauener", "Universität Bern", "lauener@ilub.unibe.ch");
			spOptions.Contacts.AddSupport("Michael", "Wagner", "brainpark", "mwagner@brainpark.ch");
			spOptions.Contacts.AddTechnical("Michael", "Wagner", "brainpark", "mwagner@brainpark.ch");

			spOptions.DiscoveryServiceUrl = new Uri("https://wayf.switch.ch/SWITCHaai/WAYF");

			options.SPOptions = spOptions;
			options.SignInAsAuthenticationType = IdentityServerConstants.ExternalCookieAuthenticationScheme;
			options.DisplayName = "SWITCHaai";
			options.AutomaticAuthenticate = false;

			var switchAaiFederation = new Federation("http://metadata.aai.switch.ch/metadata.switchaai.xml", false, options);
		}

		private X509Certificate2 GetCertificate(string thumbprint)
		{
			var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);

			var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
			return certs[0];
		}
	}
}
