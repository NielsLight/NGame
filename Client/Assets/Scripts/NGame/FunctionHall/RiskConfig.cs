using System.Collections;
using System.Collections.Generic;
namespace NGame
{
    public struct TaskConifg
    {
        public int id;
        public string brief;
        public int rewardId;
    }

    public struct ChapterConfig
    {
        public int id;
        public int[] tasks;
    }

    public struct MapConfig
    {
        public int id;
        public string name;
        public List<ChapterConfig> chapterConfigs;
    }

    public struct RiskConfig
    {
        public List<MapConfig> mapConfigs;
    }
}