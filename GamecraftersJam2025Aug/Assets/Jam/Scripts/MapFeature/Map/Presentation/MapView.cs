using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.MapFeature.Map.Domain;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.MapFeature.Map.Presentation
{
    public class MapView : MonoBehaviour
    {
        [Inject] private MapPresenter _mapPresenter;

        [Header("References")] [SerializeField]
        public RoomNodePrefab RoomNodePrefab;

        [SerializeField] public RectTransform _container;
        [SerializeField] public ScrollRect _scrollRect;
        [SerializeField] public NodesConnectionPrefab _connectionPrefab;
        [SerializeField] public MapPlayerPrefab _playerPrefab;
        [SerializeField] public float _cellSize = 100f;
        [SerializeField] public Button _exitButton;
        [SerializeField] public Button _battleFinishButton;

        [Header("Auto Scroll")] [SerializeField]
        private AutoScrollController _autoScrollController;

        private List<RoomNodePrefab> _nodes = new();
        private List<NodesConnectionPrefab> _connections = new();
        private MapPlayerPrefab _playerView;
        private RectTransform _playerRect;
        private Vector2 _startPosition;
        private RectTransform _viewport;
        private Room _currentRoom; // todo temp

        private void Awake()
        {
            SetStartPosition();
            InitializeAutoScroll();
            _exitButton.onClick.AddListener(OnExitClicked);
            _battleFinishButton.onClick.AddListener(OnBattleFinished);
        }

        private void OnBattleFinished() // todo temp
        {
            SetRoomsActive(_currentRoom);
        }

        private void OnExitClicked()
        {
            gameObject.SetActive(false);
        }

        public void OpenMap()
        {
            gameObject.SetActive(true);
        }

        private void InitializeAutoScroll()
        {
            if (_autoScrollController == null)
                _autoScrollController = _scrollRect.gameObject.AddComponent<AutoScrollController>();
        }

        public void ShowMap(List<Floor> floors, int middleRoomIndex, Room selectedRoom)
        {
            ClearMap();
            DrawMap(floors, middleRoomIndex);
            SetCurrentRoom(selectedRoom);
            PlayAnimations();
        }

        public void SetCurrentRoom(Room targetRoom)
        {
            _currentRoom = targetRoom;
            SetRoomsActive(targetRoom);
            var pos = GetExistingRoomPosition(targetRoom);
            _playerView.SetAndAnimatePos(pos, _cellSize);
            _autoScrollController.SetTarget(_playerRect);
        }

        private void OnRoomNodeClicked(Room room)
        {
            _mapPresenter.OnRoomNodeClicked(room);
            EnableAllRooms(false);
        }

        private void EnableAllRooms(bool isEnabled)
        {
            foreach (var roomNodePrefab in _nodes)
                roomNodePrefab.IsActive = isEnabled;
        }


        private void SetStartPosition()
        {
            var startX = -_container.rect.width / 2 - _cellSize;
            _startPosition = new Vector2(startX, 0f);
        }

        private void DrawMap(List<Floor> floors, int middleRoomIndex)
        {
            UpdateContainerWidth(floors.Count);
            DrawNodes(floors, middleRoomIndex);
            DrawConnections(floors);
            DrawPlayer();
        }

        private void DrawPlayer()
        {
            _playerView = Instantiate(_playerPrefab, _container);
            var pos = _nodes.First().GetComponent<RectTransform>().anchoredPosition;
            if (_playerView.TryGetComponent(out RectTransform rectTransform))
            {
                _playerRect = rectTransform;
            }

            _playerView.Setup(pos);
            _playerView.SetAndAnimatePos(pos, _cellSize);
        }

        private void UpdateContainerWidth(int floorCount)
        {
            float width = floorCount * (_cellSize / 2) + _cellSize;
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
                    var isLastFloor = room.Floor == floors.Count;
                    node.Setup(room, GetRoomPosition(room, middleRoomIndex, isLastFloor), nodeName, isLastFloor);
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
            Vector2 sourceNode = GetExistingRoomPosition(room);
            Vector2 targetNode = GetExistingRoomPosition(conn);
            connection.Setup(sourceNode, targetNode, room, conn);
            return connection;
        }

        private Vector2 GetExistingRoomPosition(Room room)
        {
            var conn = _nodes.Find(r => r.Room == room);
            if (conn == null)
                return Vector2.zero;
            return conn.TryGetComponent<RectTransform>(out var rt) ? rt.anchoredPosition : Vector2.zero;
        }

        private Vector2 GetRoomPosition(Room room, int middleRoomIndex, bool isLastFloor)
        {
            var nodeSize = _cellSize;
            var x = _startPosition.x + (room.Floor - 1) * nodeSize * 2 + Random.Range(-20f, 20f);
            var y = (room.PositionInFloor - middleRoomIndex) * nodeSize * 2 + Random.Range(-30f, 30f);
            if (isLastFloor)
                x += _cellSize;

            return new Vector2(x, y);
        }

        private void PlayAnimations()
        {
            AnimateNodes();
            AnimateConnections();
        }

        private void AnimateConnections()
        {
            float lineDelayStep = 0.03f;
            for (int i = 0; i < _connections.Count; i++)
            {
                var line = _connections[i];
                line.gameObject.SetActive(true);

                RectTransform rt = line.GetComponent<RectTransform>();
                float finalWidth = rt.sizeDelta.x;

                rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);

                var index = i;
                rt.DOSizeDelta(new Vector2(finalWidth, rt.sizeDelta.y), 0.4f)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(i * lineDelayStep);
            }
        }

        private void AnimateNodes()
        {
            float nodeDelayStep = 0.05f;
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                node.gameObject.SetActive(true);

                RectTransform rt = node.GetComponent<RectTransform>();
                Vector2 endPos = rt.anchoredPosition;
                rt.anchoredPosition += new Vector2(0, 200f);
                CanvasGroup cg = node.GetComponent<CanvasGroup>();
                if (cg == null) cg = node.gameObject.AddComponent<CanvasGroup>();
                cg.alpha = 0f;

                rt.DOAnchorPosY(endPos.y, 0.5f)
                    .SetEase(Ease.OutBack)
                    .SetDelay(i * nodeDelayStep);

                cg.DOFade(1f, 0.5f).SetDelay(i * nodeDelayStep);
            }
        }

        private void SetRoomsActive(Room targetRoom)
        {
            var conns = targetRoom.Connections;
            foreach (var roomNodePrefab in _nodes)
            {
                roomNodePrefab.IsActive = conns.Contains(roomNodePrefab.Room);
            }
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
            _exitButton.onClick.RemoveAllListeners();
            _battleFinishButton.onClick.RemoveListener(OnBattleFinished);
        }
    }
}