namespace NGame
{
    [Event(EventIdType.EnterBattleGenerateMap)]
    public class EnterBattle_GenerateMap:AEvent
    {
        public override void Run()
        {
            UI ui = Game.Scene.GetComponent<UIComponent>().Create(UIType.GemMap);
        }
    }
}