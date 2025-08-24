using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapGenerator
    {
        [Inject] private MapConfig _config;

        public MapModel GenerateMap()
        {
            MapModel mapModel = new MapModel();
            Floor previousFloor = null;
            Floor floor;

            for (int i = 0; i < _config.FloorsCountPerLevel; i++)
            {
                floor = new Floor
                {
                    Id = i + 1
                };

                List<Room> rooms = GenerateRoomsForFloor(i, _config.FloorsCountPerLevel, previousFloor);
                floor.Rooms = rooms;
                GenerateRoomTypes(floor, _config.FloorsCountPerLevel);
                previousFloor = floor;
                mapModel.Floors ??= new List<Floor>();
                mapModel.Floors.Add(floor);
            }

            mapModel.Floors = AddConnectionsBetweenRooms(mapModel.Floors);
            return mapModel;
        }

        private List<Room> GenerateRoomsForFloor(int currentFloor, int floorsCount, Floor previousFloor)
        {
            List<Room> rooms = new();

            if (currentFloor == 0)
            {
                rooms.Add(new Room
                {
                    Id = 1,
                    PositionInFloor = _config.MaxRoomsPerFloor / 2,
                    Floor = 1,
                    Connections = new List<Room>(),
                });
                return rooms;
            }

            if (currentFloor + 1 == floorsCount)
            {
                rooms.Add(new Room
                {
                    Id = 1,
                    PositionInFloor = _config.MaxRoomsPerFloor / 2,
                    Floor = floorsCount,
                    Connections = new List<Room>(),
                });
                return rooms;
            }

            Dictionary<int, List<int>> possiblePositionsForRooms = new Dictionary<int, List<int>>();

            if (previousFloor == null) return rooms;
            foreach (var prevRoom in previousFloor.Rooms)
            {
                List<int> roomPossiblePositions = new List<int>();

                for (int dx = -1; dx <= 1; dx++)
                {
                    int x = prevRoom.PositionInFloor + dx;
                    if (x >= 0 && x < _config.MaxRoomsPerFloor && !roomPossiblePositions.Contains(x))
                        roomPossiblePositions.Add(x);
                }

                possiblePositionsForRooms.Add(prevRoom.Id, roomPossiblePositions);
            }

            possiblePositionsForRooms = ResolveRoomIntersections(possiblePositionsForRooms);

            var roomId = 0;
            foreach (var previousFloorRoom in previousFloor.Rooms)
            {
                var prevFloorRoomHasRoom = false;
                var possiblePositionsForRoom = possiblePositionsForRooms[previousFloorRoom.Id];
                possiblePositionsForRoom.ForEach(position =>
                    {
                        if (!prevFloorRoomHasRoom)
                        {
                            rooms.Add(
                                new Room
                                {
                                    Id = roomId,
                                    PositionInFloor = position,
                                    Floor = currentFloor + 1,
                                    Connections = new List<Room>(),
                                }
                            );
                            roomId += 1;
                            prevFloorRoomHasRoom = true;
                        }
                        else if (Random.value < _config.ChanceToHaveTwoRoomsOnNextFloor)
                        {
                            rooms.Add(
                                new Room
                                {
                                    Id = roomId,
                                    PositionInFloor = position,
                                    Floor = currentFloor + 1,
                                    Connections = new List<Room>(),
                                }
                            );
                            roomId += 1;
                            prevFloorRoomHasRoom = true;
                        }
                    }
                );
            }

            return rooms;
        }

        private List<Floor> AddConnectionsBetweenRooms(List<Floor> floors)
        {
            List<Floor> result = new List<Floor>();
            Floor curFloor;
            Floor nextFloor;
            for (int i = 0; i < floors.Count; i++)
            {
                curFloor = floors[i];
                nextFloor = i + 1 >= floors.Count ? null : floors[i + 1];
                if (nextFloor == null) continue;

                if (curFloor.Id == floors.Count - 1)
                {
                    curFloor.Rooms.ForEach(room => { room.Connections = nextFloor.Rooms; });
                    result.Add(curFloor);
                    result.Add(nextFloor);
                    return result;
                }

                for (var index = 0; index < curFloor.Rooms.Count; index++)
                {
                    var curFloorRoom = curFloor.Rooms[index];
                    List<Room> possibleRooms = nextFloor.Rooms
                        .Where(next => Mathf.Abs(next.PositionInFloor - curFloorRoom.PositionInFloor) <= 1)
                        .ToList();
                    List<Room> connectionsToNextFloor = new List<Room>();

                    connectionsToNextFloor.AddRange(possibleRooms);
                    curFloorRoom.Connections = connectionsToNextFloor;
                }

                result.Add(curFloor);
                
                //StraightCrossingConnections(curFloor, nextFloor); <- ии код я невиновата к сожалению
                //RemoveCrossingConnections(curFloor, nextFloor);
            }

            return result;
        }

        private void StraightCrossingConnections(Floor curFloor, Floor nextFloor)
        {
            for (int i = 0; i < curFloor.Rooms.Count - 1; i++)
            {
                var leftRoom = curFloor.Rooms[i];
                var rightRoom = curFloor.Rooms[i + 1];

                foreach (var leftConn in leftRoom.Connections.ToList())
                {
                    foreach (var rightConn in rightRoom.Connections.ToList())
                    {
                        if (leftConn.PositionInFloor > rightConn.PositionInFloor)
                        {
                            var leftTarget =
                                nextFloor.Rooms.FirstOrDefault(r => r.PositionInFloor == leftRoom.PositionInFloor);
                            var rightTarget =
                                nextFloor.Rooms.FirstOrDefault(r => r.PositionInFloor == rightRoom.PositionInFloor);

                            if (leftTarget != null)
                            {
                                leftRoom.Connections.Clear();
                                leftRoom.Connections.Add(leftTarget);
                            }

                            if (rightTarget != null)
                            {
                                rightRoom.Connections.Clear();
                                rightRoom.Connections.Add(rightTarget);
                            }
                        }
                    }
                }
            }
        }


        private void RemoveCrossingConnections(Floor curFloor, Floor nextFloor)
        {
            Dictionary<Room, int> incomingCount = nextFloor.Rooms.ToDictionary(r => r, r => 0);
            foreach (var room in curFloor.Rooms)
            {
                foreach (var conn in room.Connections)
                {
                    if (incomingCount.ContainsKey(conn))
                        incomingCount[conn]++;
                }
            }

            List<ConnectionPair> toRemove = new List<ConnectionPair>();

            for (int i = 0; i < curFloor.Rooms.Count - 1; i++)
            {
                var leftRoom = curFloor.Rooms[i];
                var rightRoom = curFloor.Rooms[i + 1];

                foreach (var leftConn in leftRoom.Connections.ToList())
                {
                    foreach (var rightConn in rightRoom.Connections.ToList())
                    {
                        if (leftConn.PositionInFloor > rightConn.PositionInFloor)
                        {
                            if (leftRoom.Connections.Count > 1 &&
                                rightRoom.Connections.Count > 1 &&
                                incomingCount[leftConn] > 1 &&
                                incomingCount[rightConn] > 1)
                            {
                                toRemove.Add(new ConnectionPair(leftRoom, leftConn, rightRoom, rightConn));
                            }
                        }
                    }
                }
            }

            foreach (var pair in toRemove)
            {
                pair.LeftRoom.Connections.Remove(pair.LeftConn);
                pair.RightRoom.Connections.Remove(pair.RightConn);
                incomingCount[pair.LeftConn]--;
                incomingCount[pair.RightConn]--;
            }

            foreach (var room in curFloor.Rooms)
            {
                if (room.Connections.Count == 0)
                {
                    var nearest = nextFloor.Rooms.OrderBy(r => Mathf.Abs(r.PositionInFloor - room.PositionInFloor))
                        .First();
                    room.Connections.Add(nearest);
                    incomingCount[nearest]++;
                }
            }

            foreach (var nextRoom in nextFloor.Rooms)
            {
                if (incomingCount[nextRoom] == 0)
                {
                    var nearest = curFloor.Rooms.OrderBy(r => Mathf.Abs(r.PositionInFloor - nextRoom.PositionInFloor))
                        .First();
                    nearest.Connections.Add(nextRoom);
                    incomingCount[nextRoom]++;
                }
            }
        }


        private Dictionary<int, List<int>> ResolveRoomIntersections(Dictionary<int, List<int>> possiblePositions)
        {
            var roomsIds = possiblePositions.Keys.ToList();

            for (int i = 0; i < roomsIds.Count; i++)
            {
                for (int j = i + 1; j < roomsIds.Count; j++)
                {
                    var roomA = roomsIds[i];
                    var roomB = roomsIds[j];

                    var listA = possiblePositions[roomA];
                    var listB = possiblePositions[roomB];

                    var intersections = listA.Intersect(listB).ToList();

                    foreach (var x in intersections)
                    {
                        if (Random.value < 0.5f)
                            listA.Remove(x);
                        else
                            listB.Remove(x);
                    }
                }
            }

            return possiblePositions;
        }

        private static bool IsFirstFloor(Floor currentFloor) => currentFloor.Id == 1;
        private bool IsLastFloor(int currentFloor, int floorsCount) => currentFloor == floorsCount;

        private void GenerateRoomTypes(Floor currentFloor, int floorsCount)
        {
            RoomType? forcedFloorType = null;

            if (IsLastFloor(currentFloor.Id, floorsCount))
                forcedFloorType = RoomType.BossFight;
            else if (IsFirstFloor(currentFloor))
                forcedFloorType = RoomType.DefaultFight;
            else if (currentFloor.Id % _config.MerchantCountFloorAppearance == 0)
                forcedFloorType = RoomType.Merchant;
            else if (currentFloor.Id % _config.ChestCountFloorAppearance == 0)
                forcedFloorType = RoomType.Chest;

            if (forcedFloorType.HasValue)
            {
                foreach (var room in currentFloor.Rooms)
                    room.Type = forcedFloorType.Value;

                return;
            }

            foreach (var room in currentFloor.Rooms)
            {
                room.Type = Random.value < _config.EventChance
                    ? RoomType.Event
                    : RoomType.DefaultFight;
            }
        }
    }

    class ConnectionPair
    {
        public Room LeftRoom;
        public Room LeftConn;
        public Room RightRoom;
        public Room RightConn;

        public ConnectionPair(Room leftRoom, Room leftConn, Room rightRoom, Room rightConn)
        {
            LeftRoom = leftRoom;
            LeftConn = leftConn;
            RightRoom = rightRoom;
            RightConn = rightConn;
        }
    }
}