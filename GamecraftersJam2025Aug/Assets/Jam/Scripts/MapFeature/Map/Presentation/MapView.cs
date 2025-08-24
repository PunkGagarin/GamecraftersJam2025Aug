using System;
using System.Collections.Generic;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;

namespace Jam.Scripts.MapFeature.Map.Presentation
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] public RoomNodePrefab RoomNodePrefab;
        [SerializeField] public Sprite lineSprite;
        [SerializeField] public float cellSize = 5f;
        public event Action OnInitialize = delegate { };


        public void InitMap()
        {
            OnInitialize.Invoke();
        }

        public void ShowMap(List<Floor> floors)
        {
            DrawMap(floors, RoomNodePrefab);
        }

        private void DrawMap(List<Floor> floors, RoomNodePrefab roomNodePrefab)
        {
            foreach (var floor in floors)
            {
                foreach (var room in floor.Rooms)
                {
                    // создаём комнату
                    RoomNodePrefab node = Instantiate(RoomNodePrefab, transform);
                    node.transform.position = GetRoomPosition(room);

                    // цвет по порядку
                    var sr = node.GetComponent<SpriteRenderer>();
                    sr.color = room.Floor switch
                    {
                        1 => Color.red,
                        10 => Color.green,
                        _ => Color.white
                    };

                    node.name = $"Floor{floor.Id}_Room{room.Id}";
                }
            }

            foreach (var floor in floors)
            {
                foreach (var room in floor.Rooms)
                {
                    if (room.Connections == null) continue;

                    foreach (var conn in room.Connections)
                    {
                        CreateConnectionLine(room, conn);
                    }
                }
            }
        }

        private void CreateConnectionLine(Room roomA, Room roomB)
        {
            GameObject line = new GameObject("Line");
            line.transform.SetParent(transform, false);

            var sr = line.AddComponent<SpriteRenderer>();
            sr.sprite = lineSprite;
            sr.color = Color.white;

            Vector2 posA = GetRoomPosition(roomA);
            Vector2 posB = GetRoomPosition(roomB);

            // позиция — середина
            line.transform.position = (posA + posB) / 2;

            // масштаб по длине
            float dist = Vector2.Distance(posA, posB);
            line.transform.localScale = new Vector3(dist, 0.1f, 1f); // 0.1f — толщина линии

            // угол
            float angle = Mathf.Atan2(posB.y - posA.y, posB.x - posA.x) * Mathf.Rad2Deg;
            line.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private Vector2 GetRoomPosition(Room room)
        {
            return new Vector2(room.PositionInFloor * cellSize * 2, room.Floor * cellSize * 2);
        }
    }
}