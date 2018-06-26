using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace NGame
{
    [ObjectSystem]
    public class FuncHallComponentAwakeSystem : AwakeSystem<FuncHallComponent>
    {
        public override void Awake(FuncHallComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class FuncHallComponentLoadSystem : LoadSystem<FuncHallComponent>
    {
        public override void Load(FuncHallComponent self)
        {
            self.Load();
        }
    }
    public class FuncHallComponent : Component
    {
        public enum FuncType
        {
            Risk,
            Func,
            Hero,
            Waitter,
            Setting,
            None
        }

        private GameObject m_Root;
        public GameObject root
        {
            get
            {
                return m_Root;
            }
        }

        private Dictionary<FuncType, IFunc> funcTypes = new Dictionary<FuncType, IFunc>();

        private FuncType m_CurFuncType = FuncType.None;
        public FuncType curFuncType
        {
            get
            {
                return m_CurFuncType;
            }
        }

        private IFunc m_CurFunc;
        public IFunc curFunc
        {
            get
            {
                return m_CurFunc;
            }
        }

        public void Awake()
        {
            m_Root = GameObject.Find("Global/UI/FuncHallCanvas");
            this.Load();
        }

        public void Load()
        {
            funcTypes.Clear();
            Type[] types = DllHelper.GetMonoTypes();
            foreach (var type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(FuncHallAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                FuncHallAttribute attribute = attrs[0] as FuncHallAttribute;
                if (funcTypes.ContainsKey(attribute.funcType))
                {
                    Log.Debug(string.Format("已经存在同类 IFunc : {0}", attribute.funcType));
                    throw new Exception(string.Format("已经存在同类IFunc : {0}", attribute.funcType));
                }
                object o = Activator.CreateInstance(type);
                IFunc factory = o as IFunc;
                if (factory == null)
                {
                    Log.Debug(string.Format("{0} 没有继承 ", o.GetType().FullName));
                    continue;
                }
                this.funcTypes.Add(attribute.funcType, factory);
            }
            ///默认显示Risk功能栏
            CreateFunc(FuncType.Risk);
        }

        public void CreateFunc(FuncType type)
        {
            if (curFuncType != type)
            {
                if(m_CurFunc != null)
                {
                    m_CurFunc.RemoveFunc();
                }
                m_CurFunc = funcTypes[type].CreateFunc();
                m_CurFuncType = type;
            } 
        }

        public IFunc GetFunc(FuncType type)
        {
            return funcTypes[type];
        }

        public override void Dispose()
        {
            if(this.IsDisposed)
            {
                return;
            }
            m_CurFunc.RemoveFunc();
            base.Dispose();
        }
    }
}