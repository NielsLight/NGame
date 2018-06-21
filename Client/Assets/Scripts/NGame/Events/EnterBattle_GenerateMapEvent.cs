namespace NGame
{
    [Event(EventIdType.EnterBattleGenerateMap)]
    public class EnterBattle_GenerateMapEvent:AEvent
    {
        public override void Run()
        {
            UI ui = Game.Scene.GetComponent<UIComponent>().Create(UIType.GemMap);
        }
    }
}