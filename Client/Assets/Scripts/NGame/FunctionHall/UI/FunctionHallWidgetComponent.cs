using UnityEngine;
using UnityEngine.UI;
namespace NGame
{
    [ObjectSystem]

    public class FunctionHallWidgetAwakeSytem : AwakeSystem<FunctionHallWidgetComponent>
    {
        public override void Awake(FunctionHallWidgetComponent self)
        {
            self.Awake();
        }
    }
    public class FunctionHallWidgetComponent:Component
    {
        public void Awake()
        {
            ReferenceCollector rc = GetParent<UI>().rc;
            Button riskBtn = rc.GetExt<Button>("RiskBtn");
            riskBtn.onClick.AddListener(() => {
                Log.Debug("进入GemMap");
                Game.Scene.GetComponent<UIComponent>().root.Get<GameObject>("MainUICanvas").SetActive(false);
                Game.EventSystem.Run(EventIdType.EnterBattleGenerateMap);
            });
        }
    }
}
