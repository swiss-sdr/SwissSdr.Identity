using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;

namespace SwissSdr.Identity
{
	public static class MetadataHelpers
	{
		private static CultureInfo _swissCulture = new CultureInfo("de-ch");

		public static LocalizedName CreateSwissLocalizedName(string name)
		{
			return new LocalizedName(name, _swissCulture);
		}

		public static LocalizedEntryCollection<T> CreateLocalizedEntryCollection<T>(params T[] items) where T : LocalizedEntry
		{
			var collection = new LocalizedEntryCollection<T>();

			foreach (var item in items)
			{
				collection.Add(item);
			}

			return collection;
		}

		public static void AddAdministrative(this ICollection<ContactPerson> collection, string surname, string givenName, string company, string email)
		{
			var person = new ContactPerson(ContactType.Administrative)
			{
				Company = company,
				Surname = surname,
				GivenName = givenName
			};
			person.EmailAddresses.Add(email);

			collection.Add(person);
		}
		public static void AddSupport(this ICollection<ContactPerson> collection, string surname, string givenName, string company, string email)
		{
			var person = new ContactPerson(ContactType.Support)
			{
				Company = company,
				Surname = surname,
				GivenName = givenName
			};
			person.EmailAddresses.Add(email);

			collection.Add(person);
		}
		public static void AddTechnical(this ICollection<ContactPerson> collection, string surname, string givenName, string company, string email)
		{
			var person = new ContactPerson(ContactType.Technical)
			{
				Company = company,
				Surname = surname,
				GivenName = givenName
			};
			person.EmailAddresses.Add(email);

			collection.Add(person);
		}
	}
}
