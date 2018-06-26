using System;
using System.Threading;
using UnityEngine;
namespace NGame
{
	public class Entry : MonoBehaviour
	{

		private  void Start()
		{
            DontDestroyOnLoad(gameObject);
            Game.EventSystem.Add(DLLType.Model, typeof(Entry).Assembly);
            Game.Scene.AddComponent<RiskConfigComponent>();
            Game.Scene.AddComponent<TaskConfigComponent>();
            Game.Scene.AddComponent<ResourcesComponent>();

            Game.Scene.AddComponent<UIComponent>();
            Game.Scene.AddComponent<PlayerComponent>();

            Game.EventSystem.Run(EventIdType.EntryCreatePlayerEvent);
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