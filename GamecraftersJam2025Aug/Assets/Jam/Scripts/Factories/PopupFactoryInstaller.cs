using System.Collections.Generic;
using Jam.Scripts.Utils.Popup;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Factories
{
    public class PopupFactoryInstaller : MonoInstaller
    {
        [SerializeField] private List<Popup> _popupsPrefabs;

        public override void InstallBindings()
        {
            ProjectPopupFactoryInstall();
        }

        private void ProjectPopupFactoryInstall()
        {
            Container.Bind<PopupFactory>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .WithArguments(_popupsPrefabs)
                .NonLazy();
        }
    }
}