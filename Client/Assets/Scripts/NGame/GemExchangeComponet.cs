using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGame
{

    [ObjectSystem]

    public class GemExchangeComponetSystem : AwakeSystem<GemExchangeComponet>
    {
        public override void Awake(GemExchangeComponet self)
        {
            self.Awake();
        }
    }
    public class GemExchangeComponet:Component
    {
        private Dictionary<Vector2, UI> allGemsObject = new Dictionary<Vector2, UI>();

        public void Add(Vector2 id, UI gem)
        {
            if(!allGemsObject.ContainsKey(id))
            {
                allGemsObject.Add(id, gem);
            }
        }

        public void Awake()
        {
            allGemsObject.Clear();
        }
        
        public void SetAllGemsObject(Dictionary<Vector2, UI> gems)
        {
            allGemsObject = gems;
        }

    }

}
