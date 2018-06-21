using UnityEngine;
namespace NGame
{
    [PlayerFactory(PlayerType.normal)]
    public class NormalPlayerFactory : IPlayerFactory
    {
        public Player CreatePlayer(Entity entity, string type, PlayerInfo playerInfo)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string type)
        {
            
        }
    }

}
