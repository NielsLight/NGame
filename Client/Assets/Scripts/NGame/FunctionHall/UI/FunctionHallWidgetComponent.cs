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
    public class FunctionHallWidgetComponent : Component
    {
        private Button m_SettingBtn;
        private Button m_RiskBtn;
        private Button m_FuncBtn;
        private Button m_HeroBtn;
        private Button m_WaitterBtn;
        public void Awake()
        {
            ReferenceCollector rc = GetParent<UI>().rc;
            m_SettingBtn = rc.GetExt<Button>("SettingBtn");
            m_SettingBtn.onClick.AddListener(() =>
            {
                Game.Scene.GetComponent<FuncHallComponent>().CreateFunc(FuncHallComponent.FuncType.Setting);
            });
            m_RiskBtn = rc.GetExt<Button>("RiskBtn");
            m_RiskBtn.onClick.AddListener(() =>
            {
                Game.Scene.GetComponent<FuncHallComponent>().CreateFunc(FuncHallComponent.FuncType.Risk);
            });
            m_FuncBtn = rc.GetExt<Button>("FuncBtn");
            m_FuncBtn.onClick.AddListener(() =>
            {
                Game.Scene.GetComponent<FuncHallComponent>().CreateFunc(FuncHallComponent.FuncType.Func);
            });
            m_HeroBtn = rc.GetExt<Button>("HeroBtn");
            m_HeroBtn.onClick.AddListener(() =>
            {
                Game.Scene.GetComponent<FuncHallComponent>().CreateFunc(FuncHallComponent.FuncType.Hero);
            });
            m_WaitterBtn = rc.GetExt<Button>("WaitterBtn");
            m_WaitterBtn.onClick.AddListener(() =>
            {
                Game.Scene.GetComponent<FuncHallComponent>().CreateFunc(FuncHallComponent.FuncType.Waitter);
            });
        }
    }
}
