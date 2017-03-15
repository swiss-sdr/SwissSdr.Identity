using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Datamodel {
	public static class SwitchAaiAttributes
	{
		public static class Core
		{
			/// <summary>
			/// A unique identifier for a person, mainly used for user identification within the user's home organization.
			/// </summary>
			public const string Uid = "urn:oid:0.9.2342.19200300.100.1.1";

			/// <summary>
			/// A persistent, non-reassigned, opaque identifier for a principal. An abstracted version of the SAML V2.0 Name Identifier format of "urn:oasis:names:tc:SAML:2.0:nameidformat:persistent".
			/// </summary>
			public const string TargetedId = "urn:oid:1.3.6.1.4.1.5923.1.1.1.10";

			/// <summary>
			/// A long-lived, non re-assignable, omnidirectional identifier suitable for use as a unique external key by applications. International version of the swissEduPerson Unique ID.
			/// </summary>
			public const string EduPersonUniqueId = "urn:oid:1.3.6.1.4.1.5923.1.1.1.13";

			/// <summary>
			/// A unique identifier for a person, mainly for inter-institutional user identification on personalized services.
			/// </summary>
			public const string UniqueId = "urn:oid:2.16.756.1.2.5.1.1.1";

			/// <summary>
			/// Type of affiliation.
			/// </summary>
			public const string EduPersonAffiliation = "urn:oid:1.3.6.1.4.1.5923.1.1.1.1";

			/// <summary>
			/// Specifies the person's affiliation within a particular security domain in broad categories such as student, faculty, staff, alum, etc.
			/// </summary>
			public const string EduPersonScopedAffiliation = "urn:oid:1.3.6.1.4.1.5923.1.1.1.9";

			/// <summary>
			/// Common Name or CN according to RFC4519. It is typically the person's full name.
			/// </summary>
			public const string CommonName = "urn:oid:2.5.4.3";

			/// <summary>
			/// Given name of a person.
			/// </summary>
			public const string GivenName = "urn:oid:2.5.4.42";

			/// <summary>
			/// Surname or family name.
			/// </summary>
			public const string Surname = "urn:oid:2.5.4.4";

			/// <summary>
			/// The name(s) that should appear in white-pages-like applications for this person.
			/// </summary>
			public const string DisplayName = "urn:oid:2.16.840.1.113730.3.1.241";

			/// <summary>
			/// Preferred address for the "To:" field of e-mail to be sent to this person.
			/// </summary>
			public const string EMail = "urn:oid:0.9.2342.19200300.100.1.3";

			/// <summary>
			/// Domain name of a home organization.
			/// </summary>
			public const string HomeOrganization = "urn:oid:2.16.756.1.2.5.1.1.4";

			/// <summary>
			/// Type of a home organization.
			/// </summary>
			public const string HomeOrganizationType = "urn:oid:2.16.756.1.2.5.1.1.5";
		}

		public static class SwissEduPerson
		{
			/// <summary>
			/// Card unique identifier.
			/// </summary>
			public const string CardUid = "urn:oid:2.16.756.1.2.5.1.1.12";

			/// <summary>
			/// The date of birth of the person.
			/// </summary>
			public const string DateOfBirth = "urn:oid:2.16.756.1.2.5.1.1.2";

			/// <summary>
			/// The state of being male or female.
			/// </summary>
			public const string Gender = "urn:oid:2.16.756.1.2.5.1.1.3";

			/// <summary>
			/// Matriculation number of a student.
			/// </summary>
			public const string MatriculationNumber = "urn:oid:2.16.756.1.2.5.1.1.11";

			/// <summary>
			/// Workbranch of a staff member.
			/// </summary>
			public const string StaffCategory = "urn:oid:2.16.756.1.2.5.1.1.10";

			/// <summary>
			/// Study branch of a student, first level of classification.
			/// </summary>
			public const string StudyBranch1 = "urn:oid:2.16.756.1.2.5.1.1.6";

			/// <summary>
			/// Study branch of a student, intermediate level of classification.
			/// </summary>
			public const string StudyBranch2 = "urn:oid:2.16.756.1.2.5.1.1.7";

			/// <summary>
			/// Study branch 3 of a student.
			/// </summary>
			public const string StudyBranch3 = "urn:oid:2.16.756.1.2.5.1.1.8";

			/// <summary>
			/// Study level of a student in a particular study branch .
			/// </summary>
			public const string StudyLevel = "urn:oid:2.16.756.1.2.5.1.1.9";
		}

		public static class International
		{
			/// <summary>
			/// Set of URIs that assert compliance with specific standards for identity assurance.
			/// </summary>
			public const string Assurance = "urn:oid:1.3.6.1.4.1.5923.1.1.1.11";

			/// <summary>
			/// Office/campus phone number.
			/// </summary>
			public const string TelephoneNumber = "urn:oid:2.5.4.20";

			/// <summary>
			/// Campus or office address.
			/// </summary>
			public const string PostalAddress = "urn:oid:2.5.4.16";

			/// <summary>
			/// Identifies an employee within an organization.
			/// </summary>
			public const string EmployeeNumber = "urn:oid:2.16.840.1.113730.3.1.3";

			/// <summary>
			/// URI (either URL or URN) that indicates a set of rights to specific resources.
			/// </summary>
			public const string Entitlement = "urn:oid:1.3.6.1.4.1.5923.1.1.1.7";

			/// <summary>
			/// Home address of the user.
			/// </summary>
			public const string HomePostalAddress = "urn:oid:0.9.2342.19200300.100.1.39";

			/// <summary>
			/// Private phone number.
			/// </summary>
			public const string HomePhone = "urn:oid:0.9.2342.19200300.100.1.20";

			/// <summary>
			/// Mobile phone number.
			/// </summary>
			public const string Mobile = "urn:oid:0.9.2342.19200300.100.1.41";

			/// <summary>
			/// Person's nickname, or the informal name by which they are accustomed to be hailed.
			/// </summary>
			public const string Nickname = "urn:oid:1.3.6.1.4.1.5923.1.1.1.2";

			/// <summary>
			/// Preferred language of a user.
			/// </summary>
			public const string PreferredLanguage = "urn:oid:2.16.840.1.113730.3.1.39";

			/// <summary>
			/// Specifies the person's primary relationship to the institution in broad categories such as student, faculty, staff, alum, etc. (See controlled vocabulary). 
			/// </summary>
			public const string PrimaryAffiliation = "urn:oid:1.3.6.1.4.1.5923.1.1.1.5";

			/// <summary>
			/// The distinguished name (DN) of the directory entry representing the organization with which the person is associate.
			/// </summary>
			public const string OrganisationDN = "urn:oid:1.3.6.1.4.1.5923.1.1.1.3";

			/// <summary>
			/// The distinguished name (DN) of the directory entries representing the person's Organizational Unit(s).
			/// </summary>
			public const string OrganisationalUnitDN = "urn:oid:1.3.6.1.4.1.5923.1.1.1.4";

			/// <summary>
			/// The distinguished name (DN) of the directory entry representing the person's primary Organizational Unit(s).
			/// </summary>
			public const string PrimaryOrganisationalUnitDN = "urn:oid:1.3.6.1.4.1.5923.1.1.1.8";
		}

		public static class SwissEduId
		{
			/// <summary>
			/// The Swiss edu-ID persistent identifier for Swiss Higher Education users. The identifier is associated to a user for her/his entire life. The swissEduID should only be used internally to link personal data over a long period of time between services or applications and across institutional boundaries. The swissEduID SHOULD NOT be exposed to users.
			/// </summary>
			public const string Id = "urn:oid:2.16.756.1.2.5.1.1.13";
		}
	}
}
