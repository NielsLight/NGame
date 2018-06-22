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

        private UI m_UI;

        public UI ui
        {
            get
            {
                return m_UI;
            }
        }

        public PlayerInfo playerInfo
        {
            get
            {
                return playerInfo;
            }
        }

        public void Awake( PlayerInfo info)
        {
            this.m_PlayerInfo = info;
        }

        public void SetConnectUI(UI ui)
        {
            m_UI = ui;
        }

    }

}
