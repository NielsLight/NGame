using System;
using UnityEngine;

namespace NGame
{
    public interface IFunc
    {
        IFunc CreateFunc();
        void RemoveFunc();
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FuncHallAttribute : Attribute
    {
        public FuncHallComponent.FuncType funcType;
        public FuncHallAttribute(FuncHallComponent.FuncType type)
        {
            this.funcType = type;
        }
    }

}