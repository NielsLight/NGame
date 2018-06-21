
using System;

namespace NGame
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlayerFactoryAttribute: Attribute
    {
        public string Type { get; private set; }
        public PlayerFactoryAttribute(string type)
        {
            this.Type = type;
        }
    }
}
