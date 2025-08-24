using Jam.Scripts.MapFeature.Map.Presentation;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.MapFeature.Map
{
    public class MapStart : MonoBehaviour
    {
        [Inject] private MapView _view;

        void Start()
        {
            _view.InitMap();
        }
    }
}