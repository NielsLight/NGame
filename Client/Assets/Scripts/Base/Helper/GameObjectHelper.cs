using System;
using UnityEngine;

namespace NGame
{
	public static class GameObjectHelper
	{
		public static T Get<T>(this GameObject gameObject, string key) where T : class
		{
			try
			{
				return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
			}
			catch (Exception e)
			{
                throw new Exception(string.Format("获取{0}的ReferenceCollector key失败, key: {1}",gameObject.name,e));
			}
		}
	}
}