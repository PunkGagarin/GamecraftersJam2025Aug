using System.Collections.Generic;

namespace Jam.Scripts.MapFeature.Map.Data
{
    public class MapModel
    {
        public List<Floor> Floors { get; set; }
        public Room CurrentRoom { get; set; }
        public int MiddleRoomIndex { get; set; }
    }
}