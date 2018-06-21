
namespace NGame
{
    [Event(EventIdType.EntryCreatePlayerEvent)]
    public class Entry_CreatePlayerEvent : AEvent
    {
        public override void Run()
        {
            PlayerInfo playerInfo = new PlayerInfo();
            Player player = Game.Scene.GetComponent<PlayerComponent>().Create(PlayerType.normal, playerInfo);

        }
    }
}