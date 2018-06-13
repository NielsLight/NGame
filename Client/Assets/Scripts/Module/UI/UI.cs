using System.Collections.Generic;
using UnityEngine;

namespace NGame
{
	[ObjectSystem]
	public class UIAwakeSystem : AwakeSystem<UI, GameObject>
	{
		public override void Awake(UI self, GameObject gameObject)
		{
			self.Awake(gameObject);
		}
	}

	public sealed class UI: Entity
	{
		public string Name
		{
			get
			{
				return this.gameObject.name;
			}
		}

		public GameObject gameObject { get; private set; }

		public Dictionary<string, UI> children = new Dictionary<string, UI>();
		
		public void Awake(GameObject gameObject)
		{
			this.children.Clear();
			this.gameObject = gameObject;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (UI ui in this.children.Values)
			{
				ui.Dispose();
			}
			
			UnityEngine.Object.Destroy(gameObject);
			children.Clear();
			this.Parent = null;
		}

		public void SetAsFirstSibling()
		{
			this.gameObject.transform.SetAsFirstSibling();
		}

		public void Add(UI ui)
		{
			this.children.Add(ui.Name, ui);
			ui.Parent = this;
		}

		public void Remove(string name)
		{
			UI ui;
			if (!this.children.TryGetValue(name, out ui))
			{
				return;
			}
			this.children.Remove(name);
			ui.Dispose();
		}

		public UI Get(string name)
		{
			UI child;
			if (this.children.TryGetValue(name, out child))
			{
				return child;
			}
			Transform childGameObject = this.gameObject.transform.Find(name);
			if (childGameObject == null)
			{
				return null;
			}
			child = ComponentFactory.Create<UI, GameObject>(childGameObject.gameObject);
			this.Add(child);
			return child;
		}
	}
}