using UnityEngine;
using UnityEngine.UI;
namespace NGame
{
    [ObjectSystem]

    public class PlayerHeadWidgetComponentAwake : AwakeSystem<PlayerHeadWidgetComponent, PlayerInfo>
    {
        public override void Awake(PlayerHeadWidgetComponent self, PlayerInfo a)
        {
            self.Awake(a);
        }
    }

    public class PlayerHeadWidgetComponent : Component
    {

        public void Awake(PlayerInfo info)
        {
            ReferenceCollector rc = GetParent<UI>().rc;
            Text playerName = rc.GetExt<Text>("PlayerName");
            Text playerLevel = rc.GetExt<Text>("PlayerLevel");
            Text coinNum = rc.GetExt<Text>("CoinNum");
            Text goldNum = rc.GetExt<Text>("GoldNum");
            playerName.text = info.playerNickName;
            playerLevel.text = info.playerLevel.ToString();
            coinNum.text = info.coinNum.ToString();
            goldNum.text = info.goldNum.ToString();
        }
    }
}