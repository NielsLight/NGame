using System;
namespace NGame
{
    public class PlayerInfo
    {
        private string m_PlayerNickName;

        public string playerNickName
        {
            get
            {
                return m_PlayerNickName;
            }
        }

        private int m_PlayerLevel;
        public int playerLevel
        {
            get
            {
                return m_PlayerLevel;
            }
        }



        private int m_CoinNum;
        public int coinNum
        {
            get
            {
                return m_CoinNum;
            }
        }

        private int m_GoldNum;
        public int goldNum
        {
            get
            {
                return m_GoldNum;
            }
        }

        public void SetNickName(string name)
        {
            m_PlayerNickName = name;
        }

        public void AddCoin(int num)
        {
            m_CoinNum += num;
        }
        public void AddGold(int num)
        {
            m_GoldNum += num;
        }
        public void AddLevel(int level)
        {
            m_PlayerLevel += level;
        }

        public void SubCoin(int num)
        {
            m_CoinNum -= num;
        }
        public void SubGold(int num)
        {
            m_GoldNum -= num;
        }

        public static PlayerInfo GetTestPlayerInfo()
        {
            PlayerInfo playerinfo = new PlayerInfo();
            playerinfo.m_PlayerLevel = 1;
            playerinfo.m_CoinNum = 1000;
            playerinfo.m_GoldNum = 300;
            playerinfo.m_PlayerNickName = "Hugh";

            return playerinfo;
        }
    }
}