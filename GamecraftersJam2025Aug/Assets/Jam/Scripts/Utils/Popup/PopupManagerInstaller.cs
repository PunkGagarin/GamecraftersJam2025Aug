using UnityEngine;
using Zenject;

namespace Jam.Scripts.Utils.Popup
{
    public class PopupManagerInstaller : MonoInstaller
    {
        [SerializeField] private PopupManager _prefab;

        public override void InstallBindings()
        {
            Container.Bind<PopupManager>().FromMethod(_ =>
            {
                var instance = Container.InstantiatePrefabForComponent<PopupManager>(_prefab);
                return instance;
            }).AsSingle().NonLazy();
        }
    }
}
