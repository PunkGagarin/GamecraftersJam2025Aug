using System.Collections.Generic;

namespace Jam.Scripts.MapFeature.Map.Data.RoomContent
{
    public class Fight : RoomContentData
    {
        public List<EnemyUnit> Enemies { get; private set; }
    }
}