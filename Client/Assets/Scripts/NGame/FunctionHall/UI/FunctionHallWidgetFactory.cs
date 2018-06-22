using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGame
{
    [UIFactory(UIType.FunctionHallWidget)]
    public class FunctionHallWidgetFactory : IUIFactory
    {
        public UI Create(Scene scene, UIType type, GameObject parent)
        {
            GameObject funchall = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/FunctionHallWidget.prefab");
            UI ui = ComponentFactory.Create<UI, GameObject>(GameObject.Instantiate(funchall));
            ui.AddComponent<FunctionHallWidgetComponent>();
            return ui;
        }

        public  void Remove(UIType type)
        {
            throw new System.NotImplementedException();
        }
    }
}