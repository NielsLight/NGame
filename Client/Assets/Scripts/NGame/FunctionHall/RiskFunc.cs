using UnityEngine;
using System.Collections.Generic;

namespace NGame
{
    [FuncHall(FuncHallComponent.FuncType.Risk)]
    public class RiskFunc : IFunc
    {
       

        public IFunc CreateFunc()
        {
            RiskConfigComponent riskConfigComponent = Game.Scene.GetComponent<RiskConfigComponent>();
            
            return this;
        }

        public void RemoveFunc()
        {

        }
    }
}