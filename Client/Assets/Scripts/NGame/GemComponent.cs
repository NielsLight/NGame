namespace NGame
{
    [ObjectSystem]

    public class GemComponentAwakeSystem : AwakeSystem<GemComponent,GemData,GemType>
    {
        public override void Awake(GemComponent self,GemData data,GemType type)
        {
            self.Awake(data,type);
        }
    }
    public class GemComponent : Component
    {
        private GemData gemData;

        public GemComponent leftUI;
        public GemComponent upUI;

        public GemType gemType
        {
            get
            {
                return m_GemType;
            }
        }

        private GemType m_GemType;
        public void Awake(GemData data,GemType type)
        {
            gemData = data;
            this.m_GemType= type;
        }

        public void SetNeighborhoodUI(UI left,UI up)
        {
            if(left!=null)
            this.leftUI = left.GetComponent<GemComponent>();
            if (up != null)
                this.upUI = up.GetComponent<GemComponent>();
        }
    }
}