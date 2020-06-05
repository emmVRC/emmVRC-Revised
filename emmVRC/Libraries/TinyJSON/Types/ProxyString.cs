using System;
using System.Reflection;

namespace emmVRC.TinyJSON
{
	[Obfuscation(Exclude = true)]
	public sealed class ProxyString : Variant
	{
		readonly string value;


		public ProxyString( string value )
		{
			this.value = value;
		}


		public override string ToString( IFormatProvider provider )
		{
			return value;
		}
	}
}
