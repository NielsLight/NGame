using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NGame
{

    [ObjectSystem]

    public class GemExchangeComponetAwakeSystem : AwakeSystem<GemExchangeComponet>
    {
        public override void Awake(GemExchangeComponet self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
    public class GemExchangeComponetUpdateSystem : UpdateSystem<GemExchangeComponet>
    {
        public override void Update(GemExchangeComponet self)
        {
            self.Update();
        }
    }
    public class GemExchangeComponet : Component
    {
        private Dictionary<Vector2, UI> allGemsObject = new Dictionary<Vector2, UI>();

        private Dictionary<Vector2, UI> selectGems = new Dictionary<Vector2, UI>();

        public void Add(Vector2 id, UI gem)
        {
            if (!allGemsObject.ContainsKey(id))
            {
                allGemsObject.Add(id, gem);
            }
        }

        public void Awake()
        {
            allGemsObject.Clear();
            selectGems.Clear();
        }

        public void SetAllGemsObject(Dictionary<Vector2, UI> gems)
        {
            allGemsObject = gems;
            foreach (var pair in allGemsObject)
            {
                AddEventListener(pair.Key,pair.Value);
            }
        }


        private void AddEventListener(Vector2 index, UI ui)
        {
            if (ui.gameObject)
            {
                ui.gameObject.GetComponent<Button>().onClick.AddListener(() => { selectGems.Add(index,ui); });
            }
        }


        public void Update()
        {
            if (selectGems.Count == 2)
            {
                
                selectGems.Clear();
            }
        }
    }

}
