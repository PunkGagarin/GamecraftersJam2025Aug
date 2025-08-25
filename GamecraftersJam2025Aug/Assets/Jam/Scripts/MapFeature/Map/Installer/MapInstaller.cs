using Jam.Scripts.MapFeature.Map.Domain;
using Jam.Scripts.MapFeature.Map.Presentation;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Installer
{
    public class MapInstaller : MonoInstaller
    {
        [SerializeField] private MapView _mapPrefab;
        [SerializeField] private Transform _mapPlaceholder;

        public override void InstallBindings()
        {
            MapGeneratorInstall();
            MapViewInstall();
            MapPresenterInstall();
        }

        private void MapViewInstall()
        {
            Container
                .BindInterfacesAndSelfTo<MapView>()
                .FromComponentInNewPrefab(_mapPrefab)
                .UnderTransform(_mapPlaceholder)
                .AsSingle()
                .NonLazy();
        }

        private void MapPresenterInstall()
        {
            Container.BindInterfacesAndSelfTo<MapPresenter>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void MapGeneratorInstall()
        {
            Container.Bind<MapConnectionsGenerator>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            Container.Bind<MapGenerator>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}