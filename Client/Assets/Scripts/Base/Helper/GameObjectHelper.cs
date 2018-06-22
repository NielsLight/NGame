using System;
using UnityEngine;

namespace NGame
{
    public static class GameObjectHelper
    {
        /// <summary>
        /// 获取一个对象的rc 上key的引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this GameObject gameObject, string key) where T : class
        {
            try
            {
                return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("获取{0}的ReferenceCollector key失败, key: {1}", gameObject.name, e));
            }
        }

        /// <summary>
        /// 获取rc 上key引用的对象组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="referenceCollector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetExt<T>(this ReferenceCollector referenceCollector, string key) where T : UnityEngine.Component
        {
            GameObject obj = referenceCollector.gameObject.Get<GameObject>(key);
            return obj.GetComponent<T>();
        }
	}
}