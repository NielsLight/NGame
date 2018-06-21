using UnityEngine;
namespace NGame
{
    [ObjectSystem]
    public class PlayerAwakeSystem : AwakeSystem<Player, PlayerInfo>
    {
        public override void Awake(Player self, PlayerInfo playerInfo)
        {
            self.Awake(playerInfo);
        }
    }
    public class Player : Entity
    {
        private PlayerInfo m_PlayerInfo;
        public PlayerInfo playerInfo
        {
            get
            {
                return playerInfo;
            }
        }

        public void Awake(PlayerInfo info)
        {
            this.m_PlayerInfo = info;
        }


    }

}
