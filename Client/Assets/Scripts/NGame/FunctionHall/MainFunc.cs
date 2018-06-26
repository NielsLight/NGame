using UnityEngine;
namespace NGame
{
    [FuncHall(FuncHallComponent.FuncType.Func)]
    public class MainFunc : IFunc
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