using System.Collections.Generic;

namespace Jam.Scripts.MapFeature.Map.Data
{
    public class Level
    {
        public int Id { get; set; }
        public List<Floor> Floors { get; set; }
    }
}