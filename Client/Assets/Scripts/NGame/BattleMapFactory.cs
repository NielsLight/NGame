using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGame
{
    [UIFactory(UIType.GemMap)]
    public class BattleMapFactory : IUIFactory
    {
        private Dictionary<Vector2Int, UI> gems = new Dictionary<Vector2Int, UI>();

        private Dictionary<GemType, GameObject> items = new Dictionary<GemType, GameObject>();

        private GameObject m_BattleMap;

        private UI map;

        private int m_Row = 5;
        public int row
        {
            get
            {
                return m_Row;
            }
        }

        private int m_Col = 5;
        
        public int col
        {
            get
            {
                return m_Col;
            }
        }

        private void PrepareResource()
        {
            BattleMapFactory.Init();
            if (items.Count > 0) return;
#if UNITY_EDITOR
            m_BattleMap = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/BattleMap.prefab");
            GameObject[] gemConfigs = new GameObject[5];
            gemConfigs[0] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/5xingyuansu/Jin.prefab");
            gemConfigs[1] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/5xingyuansu/Mu.prefab");
            gemConfigs[2] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/5xingyuansu/Shui.prefab");
            gemConfigs[3] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/5xingyuansu/Huo.prefab");
            gemConfigs[4] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/5xingyuansu/Tu.prefab");
            for (int i = 0; i < gemConfigs.Length; i++)
            {
                GemType type = gemConfigs[i].GetComponent<GemConfig>().gemType;
                items.Add(type, gemConfigs[i]);
            }
#endif
        }

        public UI Create(Scene scene, string type, GameObject parent)
        {
            PrepareResource();
            Game.Scene.AddComponent<GemExchangeComponet>();
            map = ComponentFactory.Create<UI, GameObject>(GameObject.Instantiate(m_BattleMap));
            List<GemType> excudeType = new List<GemType>();
            bool bothSame = false;
            GemType preType;
            for (int i = 0; i < m_Row; i++)
                for (int j = 0; j < m_Col; j++)
                {
                    if (i >= 2)
                    {
                        preType = gems[new Vector2Int(i - 1, j)].GetComponent<GemComponent>().gemType;
                        bothSame = preType == gems[new Vector2Int(i - 2, j)].GetComponent<GemComponent>().gemType;
                        if (bothSame)
                        {
                            excudeType.Add(preType);
                        }
                    }
                    if (j >= 2)
                    {
                        preType = gems[new Vector2Int(i, j - 1)].GetComponent<GemComponent>().gemType;
                        bothSame = preType == gems[new Vector2Int(i, j - 2)].GetComponent<GemComponent>().gemType;
                        if (bothSame)
                        {
                            excudeType.Add(preType);
                        }
                    }
                    CreateOneGem(i, j, excudeType.ToArray());
                    excudeType.Clear();
                }
            Game.Scene.GetComponent<GemExchangeComponet>().SetAllGemsObject(gems);
            return map;
        }

        private UI CreateOneGem(int i, int j, GemType[] excudeGemTypes)
        {
            GemType random = GemType.FIRE;
            switch (excudeGemTypes.Length)
            {
                case 0:
                    random = BattleMapFactory.GetRandomGemType(BattleMapFactory.gemTypes);
                    break;
                case 1:
                    random = BattleMapFactory.GetRandomGemTypeExcude(excudeGemTypes[0]);
                    break;
                case 2:
                    random = BattleMapFactory.GetRandomGemTypeExcude(excudeGemTypes[0],excudeGemTypes[1]);
                    break;
            }
            GameObject obj = GameObject.Instantiate(items[random]);
            UI gem = ComponentFactory.Create<UI, GameObject>(obj);
            GemComponent gemComp = gem.AddComponent<GemComponent, GemData, GemType>(new GemData(), random);
            gem.gameObject.transform.SetParent(map.gameObject.transform);
            UI left;
            gems.TryGetValue(new Vector2Int(i - 1, j), out left);
            UI up;
            gems.TryGetValue(new Vector2Int(i, j - 1), out up);
            gemComp.SetNeighborhoodUI(left, up);
            gems.Add(new Vector2Int(i, j), gem);
            return gem;
        }

        public void Remove(string type)
        {
            if (map != null)
                map.Dispose();
        }

        private static GemType[] gemTypes;
        private static List<GemType> excudedIncudedGemType = new List<GemType>();

        private static Dictionary<GemType, int> gemTypeArrayIndex = new Dictionary<GemType, int>();

        public static void Init()
        {
            gemTypes = System.Enum.GetValues(typeof(GemType)) as GemType[];
            excudedIncudedGemType.AddRange(gemTypes);
        }

        public static GemType GetRandomGemType(GemType[] _gemTypes)
        {
            int index = Random.Range(0, _gemTypes.Length);
            return _gemTypes[index];
        }

        private static GemType GetRandomGemTypeByLimit(int min, int max)
        {
            int t = Random.Range(min, max > gemTypes.Length ? gemTypes.Length : max);
            return gemTypes[t];
        }

        public static GemType GetRandomGemTypeExcude(GemType excude)
        {
            excudedIncudedGemType.Remove(excude);
            GemType t = GetRandomGemType(excudedIncudedGemType.ToArray());
            excudedIncudedGemType.Add(excude);
            return t;
        }

        public static GemType GetRandomGemTypeExcude(GemType excudeOne, GemType excudeTwo)
        {
            excudedIncudedGemType.Remove(excudeOne);
            excudedIncudedGemType.Remove(excudeTwo);
            GemType t = GetRandomGemType(excudedIncudedGemType.ToArray());
            excudedIncudedGemType.Add(excudeOne);
            excudedIncudedGemType.Add(excudeTwo);
            return t;
        }

    }
}

