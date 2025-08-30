using System;
using System.Collections.Generic;
using Jam.Scripts.MapFeature.Map.Data.RoomContent;
using UnityEngine;

namespace Jam.Scripts.MapFeature.Map.Data
{
    public class Room
    {
        public int Id { get; set; }
        public RoomType Type { get; set; }
        public int PositionInFloor { get; set; }
        public Sprite MapIcon { get; set; }
        public Boolean IsOpened { get; private set; }
        public RoomContentData ContentData { get; private set; }
        public List<Room> Connections { get; set; }
        public int Floor { get; set; }
    }
}