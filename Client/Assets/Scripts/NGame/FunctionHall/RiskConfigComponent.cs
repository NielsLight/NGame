using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace NGame
{
    [ObjectSystem]
    public class RiskConfigComponentAwakeSystem : AwakeSystem<RiskConfigComponent>
    {
        public override void Awake(RiskConfigComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class RiskConfigComponentLoadSystem : LoadSystem<RiskConfigComponent>
    {
        public override void Load(RiskConfigComponent self)
        {
            self.Load();
        }
    }
    public class RiskConfigComponent : Component
    {

        private  RiskConfig m_RiskConifg = new RiskConfig();
        public RiskConfig riskConfig
        {
            get
            {
                return m_RiskConifg;
            }
        }
        public void Awake()
        {
            Load();
        }

        public void Load()
        {
            this.m_RiskConifg.mapConfigs = new List<MapConfig>();

            var xml = Game.monoXmlParse.LoadXml((AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/AssetBundles/Independent/Config/RiskConfig.xml").ToString()));
            foreach (var map in xml.TravelChildren())
            {
                MapConfig mapConfig = new MapConfig();
                mapConfig.id = map.ParseInt("id");
                mapConfig.name = map.ParseAttribute("name");
                List<ChapterConfig> chapterConfigs = new List<ChapterConfig>();
                foreach (var chapter in map.TravelChildren())
                {
                    ChapterConfig chapterConfig = new ChapterConfig();
                    chapterConfig.id = chapter.ParseInt("id");
                    chapterConfig.tasks = chapter.ParseInts("tasks");
                }

                mapConfig.chapterConfigs = chapterConfigs;
                m_RiskConifg.mapConfigs.Add(mapConfig);
            }
        }
    }
}