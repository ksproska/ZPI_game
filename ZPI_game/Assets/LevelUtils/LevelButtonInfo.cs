using System.Collections.Generic;

namespace LevelUtils
{
    public class LevelButtonInfo
    {
        public string GameObjectName { get; }
        public int LevelNumber { get; }
        public bool IsFinished { get; }
        public List<LevelButtonInfo> PrevLevels { get; set; }
        public string LevelName { get; private set; }
        public LevelButtonInfo(string gameObjectName, int levelNumber, bool isFinished, List<LevelButtonInfo> prevLevels)
        {
            GameObjectName = gameObjectName;
            LevelNumber = levelNumber;
            IsFinished = isFinished;
            PrevLevels = prevLevels;
            LevelName = $"lvl_{LevelNumber}";
        }
        public bool IsAvailable()
        {
            if (PrevLevels == null) { return true; }
            foreach (LevelButtonInfo level in PrevLevels)
            {
                if (level.IsFinished) { return true; }
            }
            return false;
        }

    }

    public class LevelInfoJson
    {
        public string GameObjectName { get; set; }
        public int LevelNumber { get; set; }
        public List<int> PrevLevels { get; set; }

    }

}
