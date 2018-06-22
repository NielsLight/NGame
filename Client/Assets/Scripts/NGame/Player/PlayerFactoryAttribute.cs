
using System;

namespace NGame
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlayerFactoryAttribute: Attribute
    {
        public PlayerType Type { get; private set; }
        public PlayerFactoryAttribute(PlayerType type)
        {
            this.Type = type;
        }
    }
}
