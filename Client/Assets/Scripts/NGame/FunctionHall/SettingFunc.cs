using UnityEngine;
namespace NGame
{
    [FuncHall(FuncHallComponent.FuncType.Setting)]
    public class SettingFunc : IFunc
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