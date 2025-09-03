using Jam.Scripts.MapFeature.Map.Data;

namespace Jam.Scripts.Gameplay
{
    public class RoomBattleConfig
    {
        public RoomType RoomType { get; private set; }
        public int Level { get; private set; }
        public int Floor { get; private set; }

        public RoomBattleConfig(RoomType roomType, int level, int floor)
        {
            RoomType = roomType;
            Level = level;
            Floor = floor;
        }
    }
}