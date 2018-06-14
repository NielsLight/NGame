using System.ComponentModel;
namespace NGame
{
    public abstract class Object : ISupportInitialize
    {

        public virtual void BeginInit()
        {

        }

        public virtual void EndInit()
        {

        }

        public override string ToString()
        {
            return JsonHelper.ToJson(this);
        }
    }

}