using System;

namespace Jam.Scripts.MapFeature.Map.Data.RoomContent
{
    public class EventModel : RoomContentData
    {
        public String Id { get; private set; }
        public EventType Type { get; private set; }
    }
}