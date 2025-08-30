using DG.Tweening;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.MapFeature.Map.Presentation
{
    public class NodesConnectionPrefab : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        public Room SourceRoom;
        public Room TargetRoom;

        public void Setup(Vector2 sourceNode, Vector2 targetNode, Room sourceRoom, Room targetRoom)
        {
            SourceRoom = sourceRoom;
            TargetRoom = targetRoom;
            gameObject.SetActive(false);
            SetPosition(sourceNode, targetNode);
            SetAngle(sourceNode, targetNode);
            SetDist(sourceNode, targetNode);
        }

        public void PlayImpulseAnim()
        {
            //todo wip
        }

        private void SetAngle(Vector2 sourceNode, Vector2 targetNode)
        {
            float angle = Mathf.Atan2(targetNode.y - sourceNode.y, targetNode.x - sourceNode.x) * Mathf.Rad2Deg;
            _rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void SetDist(Vector2 sourceNode, Vector2 targetNode)
        {
            float dist = Vector2.Distance(sourceNode, targetNode) * .75f;
            _rectTransform.sizeDelta = new Vector2(dist, _rectTransform.sizeDelta.y);
        }

        private void SetPosition(Vector2 sourceNode, Vector2 targetNode)
        {
            _rectTransform.anchoredPosition = (sourceNode + targetNode) / 2;
        }
    }
}