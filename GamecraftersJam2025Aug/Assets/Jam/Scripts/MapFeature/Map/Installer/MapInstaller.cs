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
            MapBusEventInstall();
            MapFactoryInstall();
            MapViewInstall();
            MapPresenterInstall();
            MapServiceInstall();
            RoomManagerSystemInstall();
        }

        private void RoomManagerSystemInstall()
        {
            Container.BindInterfacesAndSelfTo<RoomManagerSystem>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void MapBusEventInstall()
        {
            Container.Bind<MapEventBus>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void MapServiceInstall()
        {
            Container.BindInterfacesAndSelfTo<MapService>()
                .FromNew()
                .AsSingle()
                .NonLazy();
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

        private void MapFactoryInstall()
        {
            Container.Bind<MapConnectionsFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            Container.Bind<MapFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}