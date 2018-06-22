
namespace NGame
{
    [Event(EventIdType.EntryCreatePlayerEvent)]
    public class Entry_CreatePlayerEvent : AEvent
    {
        public override void Run()
        {
            PlayerInfo playerInfo = PlayerInfo.GetTestPlayerInfo();
            Player player = Game.Scene.GetComponent<PlayerComponent>().Create(PlayerType.Normal, playerInfo);

        }
    }
}