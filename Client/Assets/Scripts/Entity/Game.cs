namespace NGame
{
    public static class Game
    {
        private static Scene scene;

        public static Scene Scene
        {
            get
            {
                if (scene != null)
                {
                    return scene;
                }
                scene = new Scene();
                //scene.AddComponent<TimerComponent>();
                return scene;
            }
        }

        private static EventSystem eventSystem;

        public static EventSystem EventSystem
        {
            get
            {
                return eventSystem ?? (eventSystem = new EventSystem());
            }
        }

        private static ObjectPool objectPool;

        public static ObjectPool ObjectPool
        {
            get
            {
                return objectPool ?? (objectPool = new ObjectPool());
            }
        }

        private static MonoXmlParse m_MonoXmlParse;
        public static MonoXmlParse monoXmlParse
        {
            get
            {
                return m_MonoXmlParse ?? (m_MonoXmlParse = new MonoXmlParse());
            }
        }

        public static void Close()
        {
            scene.Dispose();
            eventSystem = null;
            scene = null;
            objectPool = null;
            m_MonoXmlParse = null;
        }
    }
}