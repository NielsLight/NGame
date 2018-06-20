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

        private Vector2Int m_Position;
        public Vector2Int  gridPos
        {
            get
            {
                return m_Position;
            }
        }

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

        public void SetGridPosition(Vector2Int pos)
        {
            this.m_Position = pos;
        }
        
    }
}