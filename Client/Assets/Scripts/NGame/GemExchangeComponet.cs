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

        private Dictionary<Vector2Int, UI> clearGems = new Dictionary<Vector2Int, UI>();

        private List<UI> remappedUIs = new List<UI>();

        private float exchangedTime = 0.2f;

        private bool alreadyCheckClear = false;

        private bool needRemap = false;

        private int tweenNum = 0;

        private float tweenInternal = 0.2f;


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
            tweenNum = 0;
            remappedUIs.Clear();
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
                if (CanExchange())
                {
                    if (!isProcessing)
                        isProcessing = true;
                    Vector3 posOne = selectGemsList[0].Value.gameObject.transform.position;
                    Vector3 posTwo = selectGemsList[1].Value.gameObject.transform.position;
                    selectGemsList[0].Value.gameObject.transform.DOMove(posTwo, exchangedTime).Play();
                    selectGemsList[1].Value.gameObject.transform.DOMove(posOne, exchangedTime).OnComplete(() =>
                    {
                        ExchangeTwoGems(selectGemsList[0].Key, selectGemsList[1].Key);
                    }).Play();
                }
                selectGems.Clear();
            }
            if(needRemap &&tweenNum==0)
            {
                needRemap = false;
                isProcessing = false;
                ReMap();
                if (CheckAndMarkClearGems())
                {
                    UpTweenGems();
                }
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
            for (int i = 0; i < battleMapFactory.row; i++)
            {
                for (int j = 0; j < battleMapFactory.col; j++)
                {
                    if (j >= 2)
                    {
                        Vector2Int vector2Int = new Vector2Int(i, j);
                        if (!allGemsObject.ContainsKey(vector2Int))
                            Log.Error(vector2Int.ToString());
                        GemComponent gemComponent = allGemsObject[vector2Int].GetComponent<GemComponent>();
                        vector2Int.y--;
                        if (!allGemsObject.ContainsKey(vector2Int))
                            Log.Error(vector2Int.ToString());
                        GemComponent leftGemComponent = allGemsObject[vector2Int].GetComponent<GemComponent>();
                        vector2Int.y--;
                        if (!allGemsObject.ContainsKey(vector2Int))
                            Log.Error(vector2Int.ToString());
                        GemComponent leftLeftGemComponent = allGemsObject[vector2Int].GetComponent<GemComponent>();
                        if (gemComponent.gemType == leftGemComponent.gemType && gemComponent.gemType == leftLeftGemComponent.gemType)
                        {
                            AddClearGemToList(new Vector2Int(i, j), gemComponent.Entity as UI);
                            AddClearGemToList(new Vector2Int(i, j - 1), leftGemComponent.Entity as UI);
                            AddClearGemToList(new Vector2Int(i, j - 2), leftLeftGemComponent.Entity as UI);
                        }
                    }
                    if (i >= 2)
                    {
                        Vector2Int vector2Int = new Vector2Int(i, j);
                        if (!allGemsObject.ContainsKey(vector2Int))
                            Log.Error(vector2Int.ToString());
                        GemComponent gemComponent = allGemsObject[vector2Int].GetComponent<GemComponent>();
                        vector2Int.x--;
                        if (!allGemsObject.ContainsKey(vector2Int))
                            Log.Error(vector2Int.ToString());
                        GemComponent upGemComponent = allGemsObject[vector2Int].GetComponent<GemComponent>();
                        vector2Int.x--;
                        if (!allGemsObject.ContainsKey(vector2Int))
                            Log.Error(vector2Int.ToString());
                        GemComponent upUpGemComponent = allGemsObject[vector2Int].GetComponent<GemComponent>();
                        if (gemComponent.gemType == upGemComponent.gemType && gemComponent.gemType == upUpGemComponent.gemType)
                        {
                            AddClearGemToList(new Vector2Int(i, j), gemComponent.Entity as UI);
                            AddClearGemToList(new Vector2Int(i - 1, j), upGemComponent.Entity as UI);
                            AddClearGemToList(new Vector2Int(i - 2, j), upUpGemComponent.Entity as UI);
                        }
                    }
                }
            }
            if (clearGems.Count > 0)
                canClear = true;
            return canClear;
        }

        private void AddClearGemToList(Vector2Int key, UI gem)
        {
            if (!clearGems.ContainsKey(key))
            {
                clearGems.Add(key, gem);
            }
        }

        private void ExchangeTwoGems(Vector2Int first, Vector2Int second)
        {
            UI temp = allGemsObject[first];
            Vector2Int gridPosOne = allGemsObject[first].GetComponent<GemComponent>().gridPos;
            Vector2Int gridPosTwo = allGemsObject[second].GetComponent<GemComponent>().gridPos;
            allGemsObject[first] = allGemsObject[second];
            allGemsObject[second] = temp;
            allGemsObject[first].GetComponent<GemComponent>().SetGridPosition(gridPosOne);
            allGemsObject[second].GetComponent<GemComponent>().SetGridPosition(gridPosTwo);
            if (!alreadyCheckClear)
            {
                alreadyCheckClear = true;
                if (!CheckAndMarkClearGems())
                {
                    Log.Debug("不能交换，回退");
                    selectGems[second] = allGemsObject[second];
                    selectGems[first] = allGemsObject[first];
                }
                else
                {
                    Log.Debug("可以交换执行消除");
                    UpTweenGems();
                }
            }
            else
            {
                isProcessing = false;
            }
        }

        private List<Vector2Int> ClearGems()
        {
            List<Vector2Int> clearInfo = new List<Vector2Int>();
            foreach (var gem in clearGems)
            {
                clearInfo.Add(gem.Key);
                gem.Value.Dispose();
                allGemsObject.Remove(gem.Key);
            }
            clearGems.Clear();
            return clearInfo;
        }

        /// <summary>
        /// 上移元素石
        /// </summary>
        private void UpTweenGems()
        {
            List<Vector2Int> clearInfo = ClearGems();
            clearInfo = clearInfo.OrderByDescending(item => item.x).ToList();
            List<Vector2Int> checkedInfo = new List<Vector2Int>();
            remappedUIs.Clear();
            StaticGridComponent staticGridComponent = Game.Scene.GetComponent<StaticGridComponent>();
            BattleMapFactory battleMapFactory = Game.Scene.GetComponent<UIComponent>().GetUIFactory(UIType.GemMap) as BattleMapFactory;
            for (int i = 0; i < clearInfo.Count; i++)
            {
                Vector2Int clearKey = clearInfo[i];
                if (checkedInfo.Contains(clearKey))
                    continue;
                int depth = 1;
                for (int j = clearKey.x - 1; j >= 0; j--)
                {
                    Vector2Int moveKey = new Vector2Int(j, clearKey.y);
                    if (clearInfo.Contains(moveKey))
                    {
                        depth++;
                        checkedInfo.Add(moveKey);
                        continue;
                    }
                    GemComponent gemComponent = allGemsObject[moveKey].GetComponent<GemComponent>();
                    Vector2Int pos = gemComponent.gridPos;
                    pos.y += staticGridComponent.CellSize.y * depth;
                    gemComponent.SetGridPosition(pos);
                    tweenNum++;
                    //Log.Debug(allGemsObject[moveKey].rectTransform.anchoredPosition.ToString());
                    allGemsObject[moveKey].gameObject.transform.DOLocalMove(new Vector3(pos.x,pos.y,0), tweenInternal * depth).OnComplete(()=> { tweenNum--;}).Play();
                    //Log.Debug(allGemsObject[moveKey].rectTransform.anchoredPosition.ToString());
                }
                //Log.Debug(clearKey.ToString()+" depth : " +depth);
                for (int j = 0; j < depth; j++)
                {
                    UI gem = battleMapFactory.CreateOneGemObject(clearKey.y, j + 1);
                    GemComponent gemComponent = gem.GetComponent<GemComponent>();
                    Vector2Int pos = gemComponent.gridPos;
                    pos.y += staticGridComponent.CellSize.y * depth;
                    gemComponent.SetGridPosition(pos);
                    tweenNum++;
                    //Log.Debug(gem.rectTransform.anchoredPosition.ToString());
                    gem.gameObject.transform.DOLocalMove(new Vector3(pos.x, pos.y,0), tweenInternal * depth).OnComplete(() => { tweenNum--;}).Play();
                    remappedUIs.Add(gem);
                }
            }
            needRemap = true;
            //Log.Debug("长度1： " + allGemsObject.Count+" 长度2："+remappedUIs.Count+" tweenNum : "+tweenNum);
            foreach(var pair in allGemsObject)
            {
                remappedUIs.Add(pair.Value);
            }
            allGemsObject.Clear();
        }




        private void ReMap()
        {
            DOTween.KillAll(true);
            StaticGridComponent staticGridComponent = Game.Scene.GetComponent<StaticGridComponent>();
            foreach (var item in remappedUIs)
            {
                Vector2Int pos = item.GetComponent<GemComponent>().gridPos;
                Vector2Int key = staticGridComponent.GetKeyInGrid(pos);
                allGemsObject[key] = item;
                allGemsObject[key].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                AddEventListener(key, allGemsObject[key]);
               // Log.Debug("key: " + key + " position： " + pos);
            }
        }

        private void CalculateClearKeys(List<Vector2Int> keys)
        {
            keys.Clear();
            foreach (var pair in allGemsObject)
            {
                if (pair.Value == null)
                    keys.Add(pair.Key);
            }
        }

    }

}
