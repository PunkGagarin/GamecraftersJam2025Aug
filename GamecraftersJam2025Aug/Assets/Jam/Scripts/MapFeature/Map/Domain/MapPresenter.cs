using System;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.MapFeature.Map.Presentation;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapPresenter : IInitializable, IDisposable
    {
        [Inject] private MapView _mapView;
        [Inject] private MapGenerator _mapGenerator;
        private MapModel _mapModel;

        public void Initialize()
        {
            _mapView.OnInitialize += OnMapInitialize;
        }

        private void OnMapInitialize()
        {
            //todo check save loadMap();
            _mapModel = _mapGenerator.GenerateMap();
            _mapView.ShowMap(_mapModel.Floors);
        }

        public void Dispose()
        {
            _mapView.OnInitialize -= OnMapInitialize;
        }
    }
}