using UnityEngine;
namespace NGame
{
    [FuncHall(FuncHallComponent.FuncType.Hero)]
    public class HeroFunc : IFunc
    {
        public IFunc CreateFunc()
        {
            return this;
        }

        public void RemoveFunc()
        {

        }
    }
}