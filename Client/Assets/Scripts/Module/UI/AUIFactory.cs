using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGame
{
    public interface IUIFactory
    {

        UI Create(Scene scene, UIType type, GameObject parent);

        void Remove(UIType type);
    }
}