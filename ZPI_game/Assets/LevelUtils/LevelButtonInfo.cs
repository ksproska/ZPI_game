

using System.Collections.Generic;
using System.Linq;

namespace LevelUtils
{
    public class LevelButtonInfo
    {
        public string GameObjectName { get; }
        public int LevelNumber { get; }
        public bool IsFinished { get; set; }
        public List<LevelButtonInfo> PrevLevels { get; set; }
        public string LevelName { get; private set; }
        public LevelButtonInfo(string gameObjectName, string levelName, int levelNumber, bool isFinished, List<LevelButtonInfo> prevLevels)
        {
            GameObjectName = gameObjectName;
            LevelNumber = levelNumber;
            IsFinished = isFinished;
            PrevLevels = prevLevels;
            LevelName = levelName;
        }
        public bool IsAvailable()
        {
            if (PrevLevels == null || !PrevLevels.Any()) { return true; }
            foreach (LevelButtonInfo level in PrevLevels)
            {
                if (level.IsFinished) { return true; }
            }
            return false;
        }
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            LevelButtonInfo other = obj as LevelButtonInfo;
            bool prevEqual = PrevLevels == null && other.PrevLevels == null || PrevLevels != null && other.PrevLevels != null && PrevLevels.SequenceEqual(other.PrevLevels);
            return GameObjectName == other.GameObjectName && LevelNumber == other.LevelNumber && IsFinished == other.IsFinished
                   && LevelName == other.LevelName && prevEqual;
        }

    }

    public class LevelInfoJson
    {
        public string GameObjectName { get; set; }
        public string LevelName { get; set; }
        public int LevelNumber { get; set; }
        public List<int> PrevLevels { get; set; }

        public LevelInfoJson(string gameObjectName, int levelNumber, List<int> prevLevels)
        {
            GameObjectName = gameObjectName;
            LevelNumber = levelNumber;
            PrevLevels = prevLevels;
        }
    }

}
