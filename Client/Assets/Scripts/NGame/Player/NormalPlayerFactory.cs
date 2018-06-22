using UnityEngine;

namespace NGame
{
    [PlayerFactory(PlayerType.Normal)]
    public class NormalPlayerFactory : IPlayerFactory
    {
        public Player CreatePlayer(Entity entity, PlayerType type, PlayerInfo playerInfo)
        {

            Player player = ComponentFactory.Create<Player, PlayerInfo>(playerInfo);
            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            UI ui = uiComponent.Create(UIType.PlayerHeadWidget);
            ui.AddComponent<PlayerHeadWidgetComponent, PlayerInfo>(playerInfo);
            player.SetConnectUI(ui);
            uiComponent.Create(UIType.FunctionHallWidget);
            return player;
        }

        public void Remove(string type)
        {

        }
    }

}
