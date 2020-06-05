using System;
using System.Reflection;

namespace emmVRC.TinyJSON
{
	[Obfuscation(Exclude = true)]
	public sealed class ProxyBoolean : Variant
	{
		readonly bool value;


		public ProxyBoolean( bool value )
		{
			this.value = value;
		}


		public override bool ToBoolean( IFormatProvider provider )
		{
			return value;
		}


		public override string ToString( IFormatProvider provider )
		{
			return value ? "true" : "false";
		}
	}
}
