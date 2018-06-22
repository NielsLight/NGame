using System;

namespace NGame
{
	[AttributeUsage(AttributeTargets.Class)]
	public class UIFactoryAttribute: Attribute
	{
		public UIType Type { get; private set; }

		public UIFactoryAttribute(UIType type)
		{
			this.Type = type;
		}
	}
}