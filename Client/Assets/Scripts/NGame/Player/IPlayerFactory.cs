namespace NGame
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(Entity entity, PlayerType type,PlayerInfo playerInfo);

        void Remove(string type);

    }

}
