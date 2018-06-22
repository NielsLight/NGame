using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGame
{
    [UIFactory(UIType.PlayerHeadWidget)]
    public class PlayerHeadWidgetFactory : IUIFactory
    {
        public  UI Create(Scene scene, UIType type, GameObject parent)
        {
            GameObject playerHead = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundles/Share/PlayerHeadWidget.prefab");
            UI playerUI = ComponentFactory.Create<UI, GameObject>(GameObject.Instantiate(playerHead));
            return playerUI;
        }

        public  void Remove(UIType type)
        {
        }
    }
}