using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGame
{
    [UIFactory(UIType.GemMap)]
    public class BattleMapFactory : IUIFactory
    {
        private Dictionary<Vector2, UI> gems = new Dictionary<Vector2, UI>();

        private Dictionary<GemType, GameObject> items = new Dictionary<GemType, GameObject>();

        private UI map;

        private int row = 5;
        private int col = 5;

        private void PrepareResource()
        {
            BattleMapFactory.Init();
            if (items.Count > 0) return;
            GameObject[] gemConfigs = Resources.LoadAll<GameObject>("5xingyuansu/");
            for (int i = 0; i < gemConfigs.Length; i++)
            {
                GemType type = gemConfigs[i].GetComponent<GemConfig>().gemType;
                items.Add(type, gemConfigs[i]);
            }
        }

        public UI Create(Scene scene, string type, GameObject parent)
        {
            PrepareResource();
            map = ComponentFactory.Create<UI, GameObject>(new GameObject("map"));
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                {
                    if (i >= 2 && j >= 2)
                    {
                        bool bothSame = false;
                        GemType preType = gems[new Vector2(i - 1, j - 1)].GetComponent<GemComponent>().gemType;
                        bothSame = preType == gems[new Vector2(i - 2, j - 2)].GetComponent<GemComponent>().gemType;
                        if (bothSame)
                        {
                            GemType random = BattleMapFactory.GetRandomGemTypeExcude(preType);
                            UI gem = ComponentFactory.Create<UI, GameObject>(items[random]);
                            gem.AddComponent<GemComponent, GemData, GemType>(new GemData(), random);
                            gem.gameObject.transform.SetParent(map.gameObject.transform);
                            gems.Add(new Vector2(i, j), gem);
                            continue;
                        }
                    }
                    CreateOneGem(i, j);
                }
            return map;
        }

        private UI CreateOneGem(int i, int j)
        {
            GemType random = BattleMapFactory.GetRandomGemType();
            UI gem = ComponentFactory.Create<UI, GameObject>(items[random]);
            gem.AddComponent<GemComponent, GemData, GemType>(new GemData(), random);
            gem.gameObject.transform.SetParent(map.gameObject.transform);
            gems.Add(new Vector2(i, j), gem);
            return gem;
        }

        public void Remove(string type)
        {
            if (map != null)
                map.Dispose();
        }

        private static GemType[] gemTypes;

        private static Dictionary<GemType, int> gemTypeArrayIndex = new Dictionary<GemType, int>();

        public static void Init()
        {
            gemTypes = System.Enum.GetValues(typeof(GemType)) as GemType[];
            for (int i = 0; i < gemTypes.Length; i++)
            {
                gemTypeArrayIndex.Add(gemTypes[i], i);
            }
        }

        public static GemType GetRandomGemType()
        {
            int index = Random.Range(0, gemTypes.Length);
            return gemTypes[index];
        }

        private static GemType GetRandomGemTypeByLimit(int min, int max)
        {
            int t = Random.Range(min, max > gemTypes.Length ? gemTypes.Length : max);
            return gemTypes[t];
        }

        public static int GetRandomDir()
        {
            int t = Random.Range(0, 2);
            return t;
        }

        public static GemType GetRandomGemTypeExcude(GemType excude)
        {
            int dir = GetRandomDir();
            GemType type = GemType.FIRE;
            switch (dir)
            {
                case 0:
                    type = GetRandomGemTypeByLimit(0, gemTypeArrayIndex[excude]);
                    break;
                case 1:
                    type = GetRandomGemTypeByLimit(gemTypeArrayIndex[excude] + 1, gemTypes.Length);
                    break;
            }
            return type;
        }
    }
}

