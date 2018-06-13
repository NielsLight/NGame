using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGame
{
	[ObjectSystem]
	public class UiComponentAwakeSystem : AwakeSystem<UIComponent>
	{
		public override void Awake(UIComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class UiComponentLoadSystem : LoadSystem<UIComponent>
	{
		public override void Load(UIComponent self)
		{
			self.Load();
		}
	}

	/// <summary>
	/// 管理所有UI
	/// </summary>
	public class UIComponent: Component
	{
		private GameObject Root;
		private readonly Dictionary<string, IUIFactory> UiTypes = new Dictionary<string, IUIFactory>();
		private readonly Dictionary<string, UI> uis = new Dictionary<string, UI>();

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			foreach (string type in uis.Keys.ToArray())
			{
				UI ui;
				if (!uis.TryGetValue(type, out ui))
				{
					continue;
				}
				uis.Remove(type);
				ui.Dispose();
			}

			this.uis.Clear();
			this.UiTypes.Clear();
		}

		public void Awake()
		{
			this.Root = GameObject.Find("Global/UI/");
			this.Load();
		}

		public void Load()
		{
			this.UiTypes.Clear();
            
            Type[] types = DllHelper.GetMonoTypes();

			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof (UIFactoryAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				UIFactoryAttribute attribute = attrs[0] as UIFactoryAttribute;
				if (UiTypes.ContainsKey(attribute.Type))
				{
                    Log.Debug(string.Format("已经存在同类UI Factory: {0}",attribute.Type));
					throw new Exception(string.Format("已经存在同类UI Factory: {0}",attribute.Type));
				}
				object o = Activator.CreateInstance(type);
				IUIFactory factory = o as IUIFactory;
				if (factory == null)
				{
                    Log.Debug(string.Format("{0} 没有继承 IUIFactory",o.GetType().FullName));
					continue;
				}
				this.UiTypes.Add(attribute.Type, factory);
			}
		}

		public UI Create(string type)
		{
			try
			{
                
				UI ui = UiTypes[type].Create(this.GetParent<Scene>(), type, Root);
				uis.Add(type, ui);
                Log.Debug(ui.Name);
				// 设置canvas
				string cavasName = ui.gameObject.GetComponent<CanvasConfig>().CanvasName;
				ui.gameObject.transform.SetParent(this.Root.Get<GameObject>(cavasName).transform, false);
				return ui;
			}
			catch (Exception e)
			{
                throw new Exception(string.Format("{0} UI 错误: {1}",type,e));
			}
		}

		public void Add(string type, UI ui)
		{
			this.uis.Add(type, ui);
		}

		public void Remove(string type)
		{
			UI ui;
			if (!uis.TryGetValue(type, out ui))
			{
				return;
			}
            uis.Remove(type);
			ui.Dispose();
		}

		public void RemoveAll()
		{
			foreach (string type in this.uis.Keys.ToArray())
			{
				UI ui;
				if (!this.uis.TryGetValue(type, out ui))
				{
					continue;
                }
                this.uis.Remove(type);
				ui.Dispose();
			}
		}

		public UI Get(string type)
		{
			UI ui;
			this.uis.TryGetValue(type, out ui);
			return ui;
		}

		public List<string> GetUITypeList()
		{
			return new List<string>(this.uis.Keys);
		}
	}
}