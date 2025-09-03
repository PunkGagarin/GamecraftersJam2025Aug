using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.Utils;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapFactory
    {
        [Inject] private MapConfig _config;
        [Inject] private MapConnectionsFactory _connectionsFactory;

        public MapModel CreateMap()
        {
            MapModel mapModel = new MapModel();
            SetInitLevel(mapModel);
            GenerateFloors(mapModel);
            mapModel.MiddleRoomIndex = _config.MaxRoomsPerFloor / 2;
            mapModel.CurrentRoom = mapModel.Floors.First().Rooms.First();
            return mapModel;
        }

        private void SetInitLevel(MapModel mapModel)
        {
            mapModel.CurrentLevel = 1;
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
                List<Room> rooms = GenerateRoomsForFloor(i, _config.FloorsCountPerLevel, previousFloor,
                    mapModel.CurrentLevel);
                floor.Rooms = rooms;
                AddTypesForRooms(floor, _config.FloorsCountPerLevel, mapModel.CurrentLevel);
                SetIconsForRooms(floor);
                mapModel.Floors ??= new List<Floor>();
                mapModel.Floors.Add(floor);
                previousFloor = floor;
            }

            mapModel.Floors = _connectionsFactory.AddConnectionsBetweenRooms(mapModel.Floors);
            RemoveRedundantRooms(mapModel.Floors);
        }

        private void RemoveRedundantRooms(List<Floor> floors)
        {
            for (int i = 0; i < floors.Count; i++)
            {
                if (i + 1 >= floors.Count)
                    return;
                var curFloor = floors[i];
                var nextFloor = floors[i + 1];

                if (nextFloor.Rooms.Count <= _config.MinRoomsPerFloor)
                    continue;

                RemoveRedundantRoom(curFloor, nextFloor);
            }
        }

        private void RemoveRedundantRoom(Floor curFloor, Floor nextFloor)
        {
            var possiblePositionsForRooms = GetPossiblePositionsForRooms(curFloor);
            foreach (var curFloorRoom in curFloor.Rooms)
            {
                var availablePositions = possiblePositionsForRooms[curFloorRoom.Id];
                var floorHaveMinCountRooms = nextFloor.Rooms.Count - 1 ! < _config.MinRoomsPerFloor;
                var roomWillHaveAtLeastOneRoom = availablePositions.Count - 1 != 0;
                if (floorHaveMinCountRooms && roomWillHaveAtLeastOneRoom)
                    RemoveRoom(availablePositions, nextFloor);
            }
        }

        private void RemoveRoom(List<int> availablePositions, Floor nextFloor)
        {
            var removedOne = false;
            foreach (var availablePosition in availablePositions)
            {
                if (!removedOne)
                {
                    nextFloor.Rooms.RemoveAt(availablePosition);
                    removedOne = true;
                    continue;
                }

                var chanceToRemoveNextRoom = Random.value < .7f;
                if (!chanceToRemoveNextRoom)
                    continue;
                nextFloor.Rooms.RemoveAt(availablePosition);
            }
        }

        private List<Room> GenerateRoomsForFloor(
            int currentFloor,
            int floorsCount,
            Floor previousFloor,
            int level
        )
        {
            List<Room> rooms = new();

            var isFirstFloor = currentFloor == 0;
            if (isFirstFloor)
                return CreateRoomsForFirstFloor(rooms, level);

            var isLastFloor = currentFloor + 1 == floorsCount;
            if (isLastFloor)
                return CreateRoomsForLastFloor(floorsCount, rooms, level);

            var isSecondFloor = currentFloor == 1;
            if (isSecondFloor)
                return CreateRoomsForSecondFloor(previousFloor, rooms, level);

            var possiblePositionsForRooms = GetPossiblePositionsForRooms(previousFloor);

            FillPositionsWithRooms(currentFloor, previousFloor, possiblePositionsForRooms, rooms, level);

            return rooms;
        }

        private List<Room> CreateRoomsForSecondFloor(Floor previousFloor, List<Room> rooms, int level)
        {
            var entryPointRoom = previousFloor.Rooms[0];
            rooms.Add(new Room
            {
                Id = 1,
                PositionInFloor = entryPointRoom.PositionInFloor - 1,
                Floor = 2,
                Level = level,
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

        private List<Room> CreateRoomsForFirstFloor(List<Room> rooms, int level)
        {
            rooms.Add(new Room
            {
                Id = 1,
                PositionInFloor = _config.MaxRoomsPerFloor / 2,
                Floor = 1,
                Level = level,
                Connections = new List<Room>(),
            });
            return rooms;
        }

        private List<Room> CreateRoomsForLastFloor(int floorsCount, List<Room> rooms, int level)
        {
            rooms.Add(new Room
            {
                Id = 1,
                PositionInFloor = _config.MaxRoomsPerFloor / 2,
                Floor = floorsCount,
                Level = level,
                Connections = new List<Room>(),
            });
            return rooms;
        }

        private Dictionary<int, List<int>> GetPossiblePositionsForRooms(Floor previousFloor)
        {
            Dictionary<int, List<int>> possiblePositionsForRooms = new Dictionary<int, List<int>>();

            foreach (var prevRoom in previousFloor.Rooms)
            {
                var roomPossiblePositions = GetPossiblePositionForRoom(prevRoom);

                possiblePositionsForRooms.Add(prevRoom.Id, roomPossiblePositions);
            }

            ResolveRoomIntersections(possiblePositionsForRooms);
            return possiblePositionsForRooms;
        }

        private List<int> GetPossiblePositionForRoom(Room prevRoom)
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

            return roomPossiblePositions;
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
            var leftRoomId = roomsIds[i];
            var rightRoomId = roomsIds[j];

            var leftRoomConnIds = possiblePositions[leftRoomId];
            var rightRoomConnIds = possiblePositions[rightRoomId];

            var intersections = leftRoomConnIds.Intersect(rightRoomConnIds).ToList();

            foreach (var intersectionPos in intersections)
            {
                ResolveIntersection(leftRoomConnIds, rightRoomConnIds, intersectionPos, leftRoomId, rightRoomId);
            }
        }

        private static void ResolveIntersection(
            List<int> leftRoomConnIds,
            List<int> rightRoomConnIds,
            int intersectionPos,
            int leftRoomId,
            int rightRoomId
        )
        {
            bool leftHasOtherConn = leftRoomConnIds.Count > 1;
            bool rightHasOtherConn = rightRoomConnIds.Count > 1;

            if (leftHasOtherConn && rightHasOtherConn)
            {
                if (Random.value < 0.5f)
                    leftRoomConnIds.Remove(intersectionPos);
                else
                    rightRoomConnIds.Remove(intersectionPos);
            }
            else if (leftHasOtherConn)
            {
                leftRoomConnIds.Remove(intersectionPos);
            }
            else if (rightHasOtherConn)
            {
                rightRoomConnIds.Remove(intersectionPos);
            }
            else
            {
                ChooseAndRemoveIntersection(leftRoomConnIds, rightRoomConnIds, intersectionPos, leftRoomId,
                    rightRoomId);
            }
        }

        private static void ChooseAndRemoveIntersection(List<int> leftRoomConnIds, List<int> rightRoomConnIds,
            int intersectionPos,
            int leftRoomId, int rightRoomId)
        {
            int leftDist = Math.Abs(leftRoomId - intersectionPos);
            int rightDist = Math.Abs(rightRoomId - intersectionPos);

            if (leftDist <= rightDist)
                rightRoomConnIds.Remove(intersectionPos);
            else
                leftRoomConnIds.Remove(intersectionPos);
        }

        private void FillPositionsWithRooms(int currentFloor,
            Floor previousFloor,
            Dictionary<int, List<int>> possiblePositionsForRooms,
            List<Room> rooms,
            int level
        )
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
                        prevFloorRoomHasRoom = CreateRoom(currentFloor, rooms, position, ref roomId, level);
                    else if (rooms.Count < _config.MinRoomsPerFloor)
                        prevFloorRoomHasRoom = CreateRoom(currentFloor, rooms, position, ref roomId, level);
                    else if (!IsThirdFloorRoomsEnough(currentFloor, rooms))
                        prevFloorRoomHasRoom = CreateRoom(currentFloor, rooms, position, ref roomId, level);
                }
            }
        }

        private bool IsThirdFloorRoomsEnough(int currentFloor, List<Room> rooms) =>
            currentFloor + 1 == 4 && rooms.Count < _config.ThirdFloorMinRoomCount;

        private bool CreateRoom(int currentFloor, List<Room> rooms, int position, ref int roomId, int level)
        {
            rooms.Add(
                new Room
                {
                    Id = roomId,
                    PositionInFloor = position,
                    Floor = currentFloor + 1,
                    Level = level,
                    Connections = new List<Room>(),
                }
            );
            roomId += 1;
            return true;
        }

        private void AddTypesForRooms(Floor currentFloor, int floorsCount, int level)
        {
            RoomType? oneTypeForFloor = null;

            if (IsLastFloor(currentFloor.Id, floorsCount))
                oneTypeForFloor = RoomType.BossFight;
            else if (IsFirstFloor(currentFloor))
                oneTypeForFloor = RoomType.DefaultFight;
            else if (_config.MerchantCountFloorAppearance > -1 &&
                     currentFloor.Id % _config.MerchantCountFloorAppearance == 0)
                oneTypeForFloor = RoomType.Merchant;
            else if (_config.ChestCountFloorAppearance > -1 && currentFloor.Id % _config.ChestCountFloorAppearance == 0)
                oneTypeForFloor = RoomType.Chest;

            if (oneTypeForFloor.HasValue)
            {
                foreach (var room in currentFloor.Rooms)
                    room.Type = oneTypeForFloor.Value;

                return;
            }

            var chances = _config.RoomTypesChances.Where(rtc => rtc.Level == level).ToList();
            foreach (var room in currentFloor.Rooms)
            {
                room.Type = GetRandomRoomType(chances);
            }
        }

        private void SetIconsForRooms(Floor floor)
        {
            foreach (var floorRoom in floor.Rooms)
            {
                SetIconByType(floorRoom);
            }
        }

        private RoomType GetRandomRoomType(List<RoomTypeChance> chances)
        {
            float total = 0f;
            foreach (var chance in chances)
                total += chance.Chance;

            float randomPoint = Random.value * total;

            float cumulative = 0f;
            foreach (var chance in chances)
            {
                cumulative += chance.Chance;
                if (randomPoint <= cumulative)
                    return chance.RoomType;
            }

            return chances[^1].RoomType;
        }

        private void SetIconByType(Room room)
        {
            room.MapIcon = room.Type switch
            {
                RoomType.Merchant => Resources.Load<Sprite>($"Sprites/MapSprites/map_icon_merchant"),
                RoomType.EliteFight => Resources.Load<Sprite>($"Sprites/MapSprites/map_icon_elite_fight"),
                RoomType.Campfire => Resources.Load<Sprite>($"Sprites/MapSprites/map_icon_campfire"),
                RoomType.Chest => Resources.Load<Sprite>($"Sprites/MapSprites/map_icon_chest"),
                RoomType.Event => Resources.Load<Sprite>($"Sprites/MapSprites/map_icon_event"),
                RoomType.BossFight => Resources.Load<Sprite>($"Sprites/MapSprites/map_icon_boss_fight"),
                _ => Resources.Load<Sprite>($"Sprites/MapSprites/map_icon_default_fight")
            };
        }

        private static bool IsFirstFloor(Floor currentFloor) => currentFloor.Id == 1;

        private bool IsLastFloor(int currentFloor, int floorsCount) => currentFloor == floorsCount;
    }
}