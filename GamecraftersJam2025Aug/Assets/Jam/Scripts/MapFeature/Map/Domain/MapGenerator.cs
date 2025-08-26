using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.Utils;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapGenerator
    {
        [Inject] private MapConfig _config;
        [Inject] private MapConnectionsGenerator _connectionsGenerator;

        public MapModel GenerateMap()
        {
            MapModel mapModel = new MapModel();
            GenerateFloors(mapModel);
            return mapModel;
        }

        private void GenerateFloors(MapModel mapModel)
        {
            Floor previousFloor = null;

            for (int i = 0; i < _config.FloorsCountPerLevel; i++)
            {
                var floor = new Floor
                {
                    Id = i + 1
                };
                List<Room> rooms = GenerateRoomsForFloor(i, _config.FloorsCountPerLevel, previousFloor);
                floor.Rooms = rooms;
                AddTypesForRooms(floor, _config.FloorsCountPerLevel);
                mapModel.Floors ??= new List<Floor>();
                mapModel.Floors.Add(floor);
                previousFloor = floor;
            }

            mapModel.Floors = _connectionsGenerator.AddConnectionsBetweenRooms(mapModel.Floors);
        }

        private List<Room> GenerateRoomsForFloor(int currentFloor, int floorsCount, Floor previousFloor)
        {
            List<Room> rooms = new();

            var isFirstFloor = currentFloor == 0;
            if (isFirstFloor)
                return CreateRoomsForFirstFloor(rooms);

            var isLastFloor = currentFloor + 1 == floorsCount;
            if (isLastFloor)
                return CreateRoomsForLastFloor(floorsCount, rooms);

            var isSecondFloor = currentFloor == 1;
            if (isSecondFloor)
                return CreateRoomsForSecondFloor(previousFloor, rooms);

            var possiblePositionsForRooms = GetPossiblePositionsForRooms(previousFloor);

            FillPositionsWithRooms(currentFloor, previousFloor, possiblePositionsForRooms, rooms);

            return rooms;
        }

        private List<Room> CreateRoomsForSecondFloor(Floor previousFloor, List<Room> rooms)
        {
            var entryPointRoom = previousFloor.Rooms[0];
            rooms.Add(new Room
            {
                Id = 1,
                PositionInFloor = entryPointRoom.PositionInFloor - 1,
                Floor = 2,
                Connections = new List<Room>(),
            });
            rooms.Add(new Room
            {
                Id = 2,
                PositionInFloor = entryPointRoom.PositionInFloor + 1,
                Floor = 2,
                Connections = new List<Room>(),
            });
            return rooms;
        }

        private List<Room> CreateRoomsForFirstFloor(List<Room> rooms)
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

        private List<Room> CreateRoomsForLastFloor(int floorsCount, List<Room> rooms)
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

        private Dictionary<int, List<int>> GetPossiblePositionsForRooms(Floor previousFloor)
        {
            Dictionary<int, List<int>> possiblePositionsForRooms = new Dictionary<int, List<int>>();

            foreach (var prevRoom in previousFloor.Rooms)
            {
                List<int> roomPossiblePositions = new List<int>();

                for (int dx = -1; dx <= 1; dx++)
                {
                    int x = prevRoom.PositionInFloor + dx;
                    var isPositionInRange = x >= 0 && x < _config.MaxRoomsPerFloor;
                    var isPositionAlreadyInList = roomPossiblePositions.Contains(x);
                    if (isPositionInRange && !isPositionAlreadyInList)
                        roomPossiblePositions.Add(x);
                }

                possiblePositionsForRooms.Add(prevRoom.Id, roomPossiblePositions);
            }

            ResolveRoomIntersections(possiblePositionsForRooms);
            return possiblePositionsForRooms;
        }

        private void ResolveRoomIntersections(Dictionary<int, List<int>> possiblePositions)
        {
            var roomsIds = possiblePositions.Keys.ToList();

            for (int i = 0; i < roomsIds.Count; i++)
            {
                for (int j = i + 1; j < roomsIds.Count; j++)
                {
                    ResolveRoomIntersection(possiblePositions, roomsIds, i, j);
                }
            }
        }

        private static void ResolveRoomIntersection(
            Dictionary<int, List<int>> possiblePositions,
            List<int> roomsIds,
            int i,
            int j
        )
        {
            var leftRoomIds = roomsIds[i];
            var rightRoomIds = roomsIds[j];

            var leftRoomConnIds = possiblePositions[leftRoomIds];
            var rightRoomConnIds = possiblePositions[rightRoomIds];

            var intersections = leftRoomConnIds.Intersect(rightRoomConnIds).ToList();

            foreach (var x in intersections)
            {
                if (Random.value < 0.5f)
                    leftRoomConnIds.Remove(x);
                else
                    rightRoomConnIds.Remove(x);
            }
        }

        private void FillPositionsWithRooms(
            int currentFloor,
            Floor previousFloor,
            Dictionary<int, List<int>> possiblePositionsForRooms,
            List<Room> rooms)
        {
            var roomId = 0;
            foreach (var previousFloorRoom in previousFloor.Rooms)
            {
                var prevFloorRoomHasRoom = false;
                var possiblePositionsForRoom = possiblePositionsForRooms[previousFloorRoom.Id];
                possiblePositionsForRoom.Shuffle();
                foreach (var position in possiblePositionsForRoom)
                {
                    if (!prevFloorRoomHasRoom)
                        prevFloorRoomHasRoom = CreateRoom(currentFloor, rooms, position, ref roomId);
                    else if (IsThirdFloorShouldHaveThreeRooms(currentFloor, rooms))
                        prevFloorRoomHasRoom = CreateRoom(currentFloor, rooms, position, ref roomId);
                    else if (IsShouldHaveAnotherRoom(rooms))
                        prevFloorRoomHasRoom = CreateRoom(currentFloor, rooms, position, ref roomId);
                }
            }
        }

        private bool IsShouldHaveAnotherRoom(List<Room> rooms)
        {
            var chanceToHaveAnotherRoom = Random.value < _config.ChanceToHaveTwoRooms;
            var chanceToHaveFiveRooms = Random.value < _config.ChanceToHaveFiveRooms;
            return rooms.Count == 4 ? chanceToHaveFiveRooms : chanceToHaveAnotherRoom;
        }

        private bool IsThirdFloorShouldHaveThreeRooms(int currentFloor, List<Room> rooms) =>
            rooms.Count < _config.MinRoomsCountForThirdFloor && currentFloor + 1 == 3;

        private bool CreateRoom(int currentFloor, List<Room> rooms, int position, ref int roomId)
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
            return true;
        }

        private void AddTypesForRooms(Floor currentFloor, int floorsCount)
        {
            RoomType? oneTypeForFloor = null;

            if (IsLastFloor(currentFloor.Id, floorsCount))
                oneTypeForFloor = RoomType.BossFight;
            else if (IsFirstFloor(currentFloor))
                oneTypeForFloor = RoomType.DefaultFight;
            else if (currentFloor.Id % _config.MerchantCountFloorAppearance == 0)
                oneTypeForFloor = RoomType.Merchant;
            else if (currentFloor.Id % _config.ChestCountFloorAppearance == 0)
                oneTypeForFloor = RoomType.Chest;

            if (oneTypeForFloor.HasValue)
            {
                foreach (var room in currentFloor.Rooms)
                    room.Type = oneTypeForFloor.Value;

                return;
            }

            foreach (var room in currentFloor.Rooms)
            {
                room.Type = Random.value < _config.EventChance
                    ? RoomType.Event
                    : RoomType.DefaultFight;
            }
        }

        private static bool IsFirstFloor(Floor currentFloor) => currentFloor.Id == 1;

        private bool IsLastFloor(int currentFloor, int floorsCount) => currentFloor == floorsCount;
    }
}