using System.Collections.Generic;
using DG.Tweening;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.MapFeature.Map.Domain;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.MapFeature.Map.Presentation
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] public RoomNodePrefab RoomNodePrefab;
        [SerializeField] public RectTransform _container;
        [SerializeField] public NodesConnectionPrefab _connectionPrefab;
        [SerializeField] public float _cellSize = 100f;
        [Inject] private MapPresenter _mapPresenter;

        private List<RoomNodePrefab> _nodes = new();
        private List<NodesConnectionPrefab> _connections = new();

        private Vector2 _startPosition;
        private RectTransform _canvasRect;

        private void Awake()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            _canvasRect = canvas.GetComponent<RectTransform>();
            SetStartPosition();
        }

        private void OnRoomNodeClicked(Room room)
        {
            _mapPresenter.OnRoomNodeClicked(room);
            Debug.Log("OnPointerClick");
        }

        private void SetStartPosition()
        {
            var startX = -_canvasRect.rect.width / 2;
            _startPosition = new Vector2(startX, 0f);
        }

        public void ShowMap(List<Floor> floors, int middleRoomIndex)
        {
            ClearMap();
            DrawMap(floors, middleRoomIndex);
        }

        public void AnimateConnection(Room sourceRoom, Room targetRoom)
        {
            foreach (var nodesConnectionPrefab in _connections)
            {
                if (nodesConnectionPrefab.SourceRoom == sourceRoom && nodesConnectionPrefab.TargetRoom == targetRoom)
                {
                    nodesConnectionPrefab.PlayImpulseAnim();        
                }
            }
        }
        
        private void DrawMap(List<Floor> floors, int middleRoomIndex)
        {
            UpdateContainerWidth(floors.Count);
            DrawNodes(floors, middleRoomIndex);
            DrawConnections(floors);
            PlayAnimations();
        }

        private void UpdateContainerWidth(int floorCount)
        {
            float width = floorCount * _cellSize / 2;
            _container.sizeDelta = new Vector2(width, _container.sizeDelta.y);
        }

        private void DrawNodes(List<Floor> floors, int middleRoomIndex)
        {
            foreach (var floor in floors)
            {
                foreach (var room in floor.Rooms)
                {
                    RoomNodePrefab node = Instantiate(RoomNodePrefab, _container);
                    node.OnRoomNodeClicked += OnRoomNodeClicked;
                    var nodeName = $"Floor{floor.Id}_Room{room.Id}";
                    node.Setup(room, GetRoomPosition(room, middleRoomIndex), nodeName);
                    _nodes.Add(node);
                }
            }
        }

        private void DrawConnections(List<Floor> floors)
        {
            foreach (var floor in floors)
            {
                foreach (var room in floor.Rooms)
                {
                    if (room.Connections == null) continue;

                    foreach (var conn in room.Connections)
                    {
                        var line = CreateConnectionLine(room, conn);
                        _connections.Add(line);
                    }
                }
            }
        }

        private NodesConnectionPrefab CreateConnectionLine(Room room, Room conn)
        {
            NodesConnectionPrefab connection = Instantiate(_connectionPrefab, _container);
            Vector2 sourceNode = GetRoomPosition(room);
            Vector2 targetNode = GetRoomPosition(conn);
            connection.Setup(sourceNode, targetNode, room, conn);
            return connection;
        }

        private Vector2 GetRoomPosition(Room room)
        {
            var conn = _nodes.Find(r => r.Room == room);
            if (conn == null)
                return Vector2.zero;
            return conn.TryGetComponent<RectTransform>(out var rt) ? rt.anchoredPosition : Vector2.zero;
        }

        private Vector2 GetRoomPosition(Room room, int middleRoomIndex)
        {
            var x = _startPosition.x + (room.Floor - 1) * _cellSize * 2 + Random.Range(-30f, 30f);
            var y = (room.PositionInFloor - middleRoomIndex) * _cellSize * 2 + Random.Range(-40f, 40f);
            return new Vector2(x, y);
        }

        private void PlayAnimations()
        {
            /*_nodes.ForEach(n => n.gameObject.SetActive(true));
            _connections.ForEach(n => n.gameObject.SetActive(true));*/

            float nodeDelayStep = 0.05f;
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                node.gameObject.SetActive(true);

                RectTransform rt = node.GetComponent<RectTransform>();
                Vector2 endPos = rt.anchoredPosition;
                rt.anchoredPosition += new Vector2(0, 200f); // старт сверху
                CanvasGroup cg = node.GetComponent<CanvasGroup>();
                if (cg == null) cg = node.gameObject.AddComponent<CanvasGroup>();
                cg.alpha = 0f;

                // Падение
                rt.DOAnchorPosY(endPos.y, 0.5f)
                    .SetEase(Ease.OutBack)
                    .SetDelay(i * nodeDelayStep);

                // Прозрачность
                cg.DOFade(1f, 0.5f).SetDelay(i * nodeDelayStep);
            }

            float lineDelayStep = 0.03f;
            for (int i = 0; i < _connections.Count; i++)
            {
                var line = _connections[i];
                line.gameObject.SetActive(true);

                RectTransform rt = line.GetComponent<RectTransform>();
                float finalWidth = rt.sizeDelta.x;

                // начинаем с нуля
                rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);

                // "рисуем" слева направо
                rt.DOSizeDelta(new Vector2(finalWidth, rt.sizeDelta.y), 0.4f)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(i * lineDelayStep);
            }
        }

        public void ShowCurrentRoom(Room curRoom, [CanBeNull] Floor prevFloor, [CanBeNull] Floor nextFloor)
        {
            if (nextFloor != null)
                foreach (var room in nextFloor.Rooms)
                {
                    var roomView = _nodes.Find(n => n.Room == room);
                    roomView.IsActive = true;
                }

            if (prevFloor != null)
                foreach (var room in prevFloor.Rooms)
                {
                    var roomView = _nodes.Find(n => n.Room == room);
                    roomView.IsActive = false;
                }
            //todo
        }

        private void ClearMap()
        {
            foreach (var obj in _nodes)
            {
                obj.OnRoomNodeClicked -= OnRoomNodeClicked;
                Destroy(obj);
            }

            _nodes.Clear();

            foreach (var obj in _connections)
                Destroy(obj);

            _connections.Clear();
        }

        private void OnDestroy()
        {
            RoomNodePrefab.OnRoomNodeClicked -= OnRoomNodeClicked;
        }
    }
}