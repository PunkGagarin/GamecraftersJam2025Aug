using System.Collections.Generic;
using Jam.Scripts.GameplayData.Repositories;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Data
{
    [CreateAssetMenu(fileName = "RoomEventRepository", menuName = "Game Resources/RoomEvents/RoomEventRepository")]
    public class RoomEventRepository: Repository<RoomEvent>
    {
        public List<int> UsedDefinitionsIds => new();   
    }
}