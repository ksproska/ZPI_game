using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelUtils;

namespace CurrentState
{
    public static class CurrentGameState
    {
        public static LoadSaveHelper.SlotNum CurrentSlot { get; set; }
        public static string CurrentLevelName { get; set; }
    }
}
