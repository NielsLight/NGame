using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

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
        private Dictionary<Vector2Int, UI> allGemsObject = new Dictionary<Vector2Int, UI>();

        private Dictionary<Vector2Int, UI> selectGems = new Dictionary<Vector2Int, UI>();

        private List<KeyValuePair<Vector2Int, UI>> selectGemsList = new List<KeyValuePair<Vector2Int, UI>>();

        private List<UI> clearGems = new List<UI>();

        private float exchangedTime = 0.2f;

        private bool alreadyCheckClear = false;


        private bool isProcessing = false;

        public void Add(Vector2Int id, UI gem)
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
            clearGems.Clear();
            isProcessing = false;
            alreadyCheckClear = false;
        }

        public void SetAllGemsObject(Dictionary<Vector2Int, UI> gems)
        {
            allGemsObject = gems;
            foreach (var pair in allGemsObject)
            {
                AddEventListener(pair.Key, pair.Value);
            }
        }


        private void AddEventListener(Vector2Int index, UI ui)
        {
            if (ui.gameObject)
            {
                ui.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!isProcessing)
                    {
                        alreadyCheckClear = false;
                        if (!selectGems.ContainsKey(index))
                            selectGems.Add(index, ui);
                    }
                });
            }
        }


        public void Update()
        {
            if (selectGems.Count == 2)
            {
                selectGemsList = selectGems.ToList();
                if(CanExchange())
                {
                    if (!isProcessing)
                        isProcessing = true;
                    Vector3 posOne = selectGemsList[0].Value.gameObject.transform.position;
                    Vector3 posTwo = selectGemsList[1].Value.gameObject.transform.position;
                    selectGemsList[0].Value.gameObject.transform.DOMove(posTwo, exchangedTime);
                    selectGemsList[1].Value.gameObject.transform.DOMove(posOne, exchangedTime).OnComplete(() =>
                    {
                        isProcessing = false;
                        ExchangeTwoGems(selectGemsList[0].Key, selectGemsList[1].Key);
                    });
                }
                selectGems.Clear();
            }
        }

        private int GetTwoGemsDistance()
        {
            Vector2Int distance = selectGemsList[0].Key - selectGemsList[1].Key;
            return Mathf.Abs(distance.x) + Mathf.Abs(distance.y);
        }

        private bool CanExchange()
        {
            return GetTwoGemsDistance() == 1 && selectGemsList[0].Value.GetComponent<GemComponent>().gemType != selectGemsList[1].Value.GetComponent<GemComponent>().gemType;
        }

        private bool CheckAndMarkClearGems()
        {
            bool canClear = false;
            BattleMapFactory battleMapFactory = Game.Scene.GetComponent<UIComponent>().GetUIFactory(UIType.GemMap) as BattleMapFactory;
            for (int i=0; i < battleMapFactory.row;i++)
            {
                for(int j = 0;j<battleMapFactory.col;j++)
                {
                    if(j>=2)
                    {
                       GemComponent gemComponent = allGemsObject[new Vector2Int(i,j)].GetComponent<GemComponent>();
                       if(gemComponent.gemType == gemComponent.leftUI.gemType && gemComponent.gemType == gemComponent.leftUI.leftUI.gemType)
                        {
                            AddClearGemToList(gemComponent.Entity as UI);
                            AddClearGemToList(gemComponent.leftUI.Entity as UI);
                            AddClearGemToList(gemComponent.leftUI.leftUI.Entity as UI);
                        }
                    }
                    if (i >= 2)
                    {
                        GemComponent gemComponent = allGemsObject[new Vector2Int(i, j)].GetComponent<GemComponent>();
                        if (gemComponent.gemType == gemComponent.upUI.gemType && gemComponent.gemType == gemComponent.upUI.upUI.gemType)
                        {
                            AddClearGemToList(gemComponent.Entity as UI);
                            AddClearGemToList(gemComponent.upUI.Entity as UI);
                            AddClearGemToList(gemComponent.upUI.upUI.Entity as UI);
                        }
                    }
                }
            }
            if(clearGems.Count>0)
                canClear = true;
            return canClear;
        }

        private void AddClearGemToList(UI gem)
        {
            if (!clearGems.Contains(gem))
            {
                clearGems.Add(gem);
            }
        }

        private void ExchangeTwoGems(Vector2Int first, Vector2Int second)
        {
            UI temp = allGemsObject[first];
            allGemsObject[first] = allGemsObject[second];
            allGemsObject[second] = temp;
            if (!alreadyCheckClear)
            {
                alreadyCheckClear = true;
                if (!CheckAndMarkClearGems())
                {
                    selectGems[second] = allGemsObject[second];
                    selectGems[first] = allGemsObject[first];
                }
                else
                {

                }
            }
        }
    }

}
