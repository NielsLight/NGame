using System;
using System.Threading;
using UnityEngine;
namespace NGame
{
	public class Entry : MonoBehaviour
	{

		private  void Start()
		{
			try
			{

				DontDestroyOnLoad(gameObject);
				Game.EventSystem.Add(DLLType.Model, typeof(Entry).Assembly);

				Game.Scene.AddComponent<UIComponent>();


                Game.EventSystem.Run(EventIdType.EnterBattleGenerateMap);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}