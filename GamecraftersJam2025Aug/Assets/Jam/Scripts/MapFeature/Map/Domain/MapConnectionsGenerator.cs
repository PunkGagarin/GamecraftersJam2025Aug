using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapConnectionsGenerator
    {
        [Inject] private MapConfig _config;

        public List<Floor> AddConnectionsBetweenRooms(List<Floor> floors)
        {
            List<Floor> result = new List<Floor>();
            for (int i = 0; i < floors.Count; i++)
            {
                if (i + 1 >= floors.Count)
                    return result;
                var curFloor = floors[i];
                var nextFloor = floors[i + 1];

                var isPenultimateFloor = curFloor.Id == floors.Count - 1;
                if (isPenultimateFloor)
                {
                    return SetConnectionToBoss(curFloor, nextFloor, result);
                }

                SetConnectionsBetweenFloors(curFloor, nextFloor);
                result.Add(curFloor);
                RemoveRedundantConnections(curFloor);
            }
            
            return result;
        }

        private void RemoveRedundantConnections(Floor curFloor)
        {
            foreach (var room in curFloor.Rooms.ToList())
            {
                foreach (var connection in room.Connections.ToList())
                {
                    RemoveRedundantConnection(room, curFloor, connection);
                }
            }
        }

        private static void RemoveRedundantConnection(Room room, Floor floor, Room connection)
        {
            bool sourceHasOtherConnections = room.Connections.Count > 1;

            int incomingConnectionsCount = floor.Rooms
                .SelectMany(r => r.Connections)
                .Count(r => r == connection);
            var randomWeight = Random.value < 0.5;

            bool targetHasOtherConnections = incomingConnectionsCount > 1;

            if (sourceHasOtherConnections && targetHasOtherConnections && randomWeight)
            {
                room.Connections.Remove(connection);
            }
        }

        private static List<Floor> SetConnectionToBoss(Floor curFloor, Floor nextFloor, List<Floor> floors)
        {
            curFloor.Rooms.ForEach(room => { room.Connections = nextFloor.Rooms; });
            floors.Add(curFloor);
            floors.Add(nextFloor);
            return floors;
        }

        private void SetConnectionsBetweenFloors(Floor curFloor, Floor nextFloor)
        {
            Dictionary<Room, List<Room>> availableRooms = GetConnectionForFloor(curFloor, nextFloor);

            foreach (var (room, connections) in availableRooms)
                room.Connections.AddRange(connections);

            ResolveConnectionsIntersections(availableRooms, curFloor, nextFloor);
        }

        private Dictionary<Room, List<Room>> GetConnectionForFloor(Floor curFloor, Floor nextFloor)
        {
            Dictionary<Room, List<Room>> possiblePositionsForRooms = new Dictionary<Room, List<Room>>();

            foreach (var curRoom in curFloor.Rooms)
            {
                var roomPossiblePositions = GetRoomsWithPossiblePositions(nextFloor, curRoom);

                possiblePositionsForRooms.Add(curRoom, roomPossiblePositions);
            }

            return possiblePositionsForRooms;
        }

        private List<Room> GetRoomsWithPossiblePositions(Floor nextFloor, Room curRoom)
        {
            List<Room> roomPossiblePositions = new List<Room>();

            for (int dx = -1; dx <= 1; dx++)
            {
                int x = curRoom.PositionInFloor + dx;
                var isPositionInRange = x >= 0 && x < _config.MaxRoomsPerFloor;
                var isRoomExistOnFloor = IsRoomExistOnFloor(nextFloor, x, out var room);
                var isRoomAlreadyInList = roomPossiblePositions.Contains(room);
                if (isPositionInRange && isRoomExistOnFloor && !isRoomAlreadyInList)
                    roomPossiblePositions.Add(room);
            }

            return roomPossiblePositions;
        }

        private bool IsRoomExistOnFloor(Floor nextFloor, int x, out Room room)
        {
            var isRoomExist = nextFloor.Rooms.Any(room => room.PositionInFloor == x);
            room = isRoomExist ? nextFloor.Rooms.Find(r => r.PositionInFloor == x) : null;
            return room != null;
        }


        private void ResolveConnectionsIntersections(Dictionary<Room, List<Room>> possiblePositions, Floor curFloor, Floor nextFloor)
        {
            for (var i = 0; i < possiblePositions.Count; i++)
            {
                if (i + 1 > possiblePositions.Count)
                    return;
                KeyValuePair<Room, List<Room>> item = possiblePositions.ElementAt(i);
                Room curRoom = item.Key;

                if (HasIntersection(curRoom, curFloor, out var rightRoom)) 
                    ResolveConflict(curRoom, rightRoom, nextFloor);
            }
        }

        private void ResolveConflict(Room leftRoom, Room rightRoom, Floor nextFloor)
        {
            var leftRoomConflictConn =
                leftRoom.Connections.Find(conn => conn.PositionInFloor > leftRoom.PositionInFloor);
            var rightRoomConflictConn =
                rightRoom.Connections.Find(conn => conn.PositionInFloor < rightRoom.PositionInFloor);

            bool isLeftHasOtherConn = leftRoom.Connections.Count > 1;
            bool isRightHasOtherConn = rightRoom.Connections.Count > 1;

            if (isLeftHasOtherConn && isRightHasOtherConn)
            {
                if (Random.value < 0.5f)
                    RemoveConnection(leftRoom, leftRoomConflictConn);
                else
                    RemoveConnection(rightRoom, rightRoomConflictConn);
            }
            else if (isLeftHasOtherConn)
            {
                RemoveConnection(leftRoom, leftRoomConflictConn);
            }
            else if (isRightHasOtherConn)
            {
                RemoveConnection(rightRoom, rightRoomConflictConn);
            }
            else
            {
                SetNewNearestConnection(leftRoom, rightRoom, nextFloor);
            }
        }

        private void SetNewNearestConnection(Room leftRoom, Room rightRoom, Floor nextFloor)
        {
            var newLeftConn = FindNearestRoom(nextFloor, leftRoom.PositionInFloor);
            var newRightConn = FindNearestRoom(nextFloor, rightRoom.PositionInFloor);

            if (newLeftConn != null) leftRoom.Connections.Add(newLeftConn);
            if (newRightConn != null) rightRoom.Connections.Add(newRightConn);
        }

        private static void RemoveConnection(Room room, Room connection) => 
            room.Connections.Remove(connection);

        private Room FindNearestRoom(Floor floor, int targetPos)
        {
            Room nearestRoom = null;

            foreach (var room in floor.Rooms)
            {
                if (
                    nearestRoom == null ||
                    Mathf.Abs(room.PositionInFloor - targetPos) < Mathf.Abs(nearestRoom.PositionInFloor - targetPos)
                )
                {
                    nearestRoom = room;
                }
            }

            return nearestRoom;
        }

        private bool HasIntersection(Room leftRoom, Floor currentFloor, out Room rightRoom)
        {
            rightRoom = null;
            return RoomHasRightConnection(leftRoom) &&
                   RoomHasRightNeighbor(leftRoom, currentFloor, out rightRoom) &&
                   RoomHasLeftConnection(rightRoom);
        }

        private bool RoomHasRightNeighbor(Room leftRoom, Floor currentFloor, out Room rightRoom)
        {
            int currentPosition = leftRoom.PositionInFloor;
            int nextPosition = currentPosition + 1;
            rightRoom = currentFloor.Rooms.FirstOrDefault(room => room.PositionInFloor == nextPosition);
            return rightRoom != null;
        }

        private bool RoomHasRightConnection(Room leftRoom) =>
            leftRoom.Connections.Any(conn => conn.PositionInFloor > leftRoom.PositionInFloor);

        private bool RoomHasLeftConnection(Room rightRoom) =>
            rightRoom.Connections.Any(conn => conn.PositionInFloor < rightRoom.PositionInFloor);
    }
}