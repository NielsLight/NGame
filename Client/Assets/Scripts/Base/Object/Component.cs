using System;


namespace NGame
{
	public abstract partial class Component : Object, IDisposable
	{
		public long InstanceId { get; protected set; }

		private bool isFromPool;

		public bool IsFromPool
		{
			get
			{
				return this.isFromPool;
			}
			set
			{
				this.isFromPool = value;

				if (this.InstanceId == 0)
				{
					this.InstanceId = IdGenerater.GenerateId();
					Game.EventSystem.Add(this);
				}
			}
		}

		public bool IsDisposed
		{
			get
			{
				return this.InstanceId == 0;
			}
		}
		
		public Component Parent { get; set; }

		public T GetParent<T>() where T : Component
		{
			return this.Parent as T;
		}

		public Entity Entity
		{
			get
			{
				return this.Parent as Entity;
			}
		}

		protected Component()
		{
			this.InstanceId = IdGenerater.GenerateId();
			Game.EventSystem.Add(this);
		}

		public virtual void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			Game.EventSystem.Remove(this.InstanceId);

			this.InstanceId = 0;

			if (this.IsFromPool)
			{
				Game.ObjectPool.Recycle(this);
			}

			// 触发Destroy事件
			Game.EventSystem.Destroy(this);
		}
	}
}