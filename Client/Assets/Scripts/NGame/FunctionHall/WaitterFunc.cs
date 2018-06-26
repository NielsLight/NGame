using UnityEngine;
namespace NGame
{
    [FuncHall(FuncHallComponent.FuncType.Waitter)]
    public class WaitterFunc : IFunc
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