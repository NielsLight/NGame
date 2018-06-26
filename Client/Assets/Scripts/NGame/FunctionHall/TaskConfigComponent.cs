using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace NGame
{
    [ObjectSystem]
    public class TaskConfigComponentAwakeSystem : AwakeSystem<TaskConfigComponent>
    {
        public override void Awake(TaskConfigComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class TaskConfigComponentLoadSystem : LoadSystem<TaskConfigComponent>
    {
        public override void Load(TaskConfigComponent self)
        {
            self.Load();
        }
    }
    public class TaskConfigComponent : Component
    {

        private  Dictionary<int, TaskConifg> m_TaskConfigs = new Dictionary<int, TaskConifg>();
        public Dictionary<int, TaskConifg> taskConfigs
        {
            get
            {
                return m_TaskConfigs;
            }
        }
        public void Awake()
        {
            Load();
        }

        public void Load()
        {
            m_TaskConfigs.Clear();
            var xml = Game.monoXmlParse.LoadXml((AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/AssetBundles/Independent/Config/TaskConfig.xml").ToString()));

            foreach (var task in xml.TravelChildren())
            {
                TaskConifg taskConifg = new TaskConifg();
                taskConifg.id = task.ParseInt("id");
                taskConifg.brief = task.ParseAttribute("brief");
                taskConifg.rewardId = task.ParseInt("id");
                m_TaskConfigs.Add(taskConifg.id, taskConifg);
            }
        }
    }
}