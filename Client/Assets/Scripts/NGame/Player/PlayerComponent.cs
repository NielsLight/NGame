using System;
using System.Collections.Generic;
using UnityEngine;
namespace NGame
{

    public class PlayerComponent : Component
    {
        private readonly Dictionary<string, IPlayerFactory> playerTypes = new Dictionary<string, IPlayerFactory>();

        public void Awake()
        {
            playerTypes.Clear();
            this.Load();
        }

        public void Load()
        {
            Type[] types = DllHelper.GetMonoTypes();

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(PlayerFactoryAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                PlayerFactoryAttribute attribute = attrs[0] as PlayerFactoryAttribute;
                if (playerTypes.ContainsKey(attribute.Type))
                {
                    Log.Debug(string.Format("已经存在同类Player Factory: {0}", attribute.Type));
                    throw new Exception(string.Format("已经存在同类Player Factory: {0}", attribute.Type));
                }
                object o = Activator.CreateInstance(type);
                IPlayerFactory factory = o as IPlayerFactory;
                if (factory == null)
                {
                    Log.Debug(string.Format("{0} 没有继承 IPlayerFactory", o.GetType().FullName));
                    continue;
                }
                this.playerTypes.Add(attribute.Type, factory);
            }
        }
        public Player Create(string type, PlayerInfo playerInfo)
        {
            Player player = this.playerTypes[type].CreatePlayer(this.Entity, type, playerInfo);

            return player;
        }
    }

}