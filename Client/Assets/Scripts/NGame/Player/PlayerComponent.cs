using System;
using System.Collections.Generic;
using UnityEngine;
namespace NGame
{
    [ObjectSystem]

    public class PlayerComponentAwakeSystem : AwakeSystem<PlayerComponent>
    {
        public override void Awake(PlayerComponent self)
        {
            self.Awake();
        }
    }
    public class PlayerComponent : Component
    {
        private readonly Dictionary<PlayerType, IPlayerFactory> playerTypes = new Dictionary<PlayerType, IPlayerFactory>();
        private readonly Dictionary<PlayerType, Player> players = new Dictionary<PlayerType, Player>();

        public void Awake()
        {
            this.Load();
        }

        public void Load()
        {
            playerTypes.Clear();
            players.Clear();
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
        public Player Create(PlayerType type, PlayerInfo playerInfo)
        {
            Player player = this.playerTypes[type].CreatePlayer(this.Entity, type, playerInfo);
            players.Add(type, player);

            return player;
        }

        public Player GetPlayer(PlayerType type)
        {
            Player player;
            if(!players.TryGetValue(type,out player))
            {
                Log.Error("没有这个id的player: " + type);
            }
            return player;
        }
    }

}