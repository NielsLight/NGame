namespace NGame
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(Entity entity,string type,PlayerInfo playerInfo);

        void Remove(string type);

    }

}
