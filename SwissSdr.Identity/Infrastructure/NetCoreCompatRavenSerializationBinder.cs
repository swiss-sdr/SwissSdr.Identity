using Raven.Imports.Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity
{
	/// <summary>
	/// Rewrites assembly references for "System.Private.CoreLib" to "mscorlib" to
	/// enable JSON serialization interop between .NET Full Framework and .NET Core
	/// </summary>
	public class NetCoreCompatRavenSerializationBinder : DefaultSerializationBinder
	{
		public override Type BindToType(string assemblyName, string typeName)
		{
			assemblyName = assemblyName.Replace("System.Private.CoreLib", "mscorlib");
			typeName = typeName.Replace("System.Private.CoreLib", "mscorlib");
			return base.BindToType(assemblyName, typeName);
		}
	}
}
