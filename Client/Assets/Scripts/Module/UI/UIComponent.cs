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
		private GameObject m_Root;
        public GameObject root
        {
            get
            {
                return m_Root;
            }
        }
		private readonly Dictionary<UIType, IUIFactory> UiTypes = new Dictionary<UIType, IUIFactory>();
		private readonly Dictionary<UIType, UI> uis = new Dictionary<UIType, UI>();

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			foreach (UIType type in uis.Keys.ToArray())
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
			this.m_Root = GameObject.Find("Global/UI/");
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

        /// <summary>
        /// 获取组件下的工厂
        /// </summary>
        /// <param name="type">工厂属性</param>
        /// <returns></returns>
        public IUIFactory GetUIFactory(UIType type)
        {
            IUIFactory uIFactory = null;
            if(!this.UiTypes.TryGetValue(type,out uIFactory))
            {
                Log.Debug(string.Format("{0} 没有继承 IUIFactory", type));
            }
            return uIFactory;
        }

        public UI Create(UIType type)
		{
			try
			{
                
				UI ui = UiTypes[type].Create(this.GetParent<Scene>(), type, m_Root);
				uis.Add(type, ui);
                Log.Debug(ui.Name);
				// 设置canvas
				string cavasName = ui.gameObject.GetComponent<CanvasConfig>().CanvasName;
				ui.gameObject.transform.SetParent(this.m_Root.Get<GameObject>(cavasName).transform, false);
				return ui;
			}
			catch (Exception e)
			{
                throw new Exception(string.Format("{0} UI 错误: {1}",type,e));
			}
		}

        public void Add(UIType type, UI ui)
		{
			this.uis.Add(type, ui);
		}

		public void Remove(UIType type)
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
			foreach (UIType type in this.uis.Keys.ToArray())
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

		public UI Get(UIType type)
		{
			UI ui;
			this.uis.TryGetValue(type, out ui);
			return ui;
		}

		public List<UIType> GetUITypeList()
		{
			return new List<UIType>(this.uis.Keys);
		}
	}
}