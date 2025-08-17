using System;
using System.Collections.Generic;
using Jam.Scripts.Utils.Popup;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Factories
{
    public class PopupFactory : MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private List<Popup> _popupsPrefabs;

        private Dictionary<Type, Popup> _popupPrefabMap = null;

        private void Awake()
        {
            _popupPrefabMap = new Dictionary<Type, Popup>();

            foreach (var popup in _popupsPrefabs)
            {
                Type type = popup.GetType();
                if (typeof(Popup).IsAssignableFrom(type))
                {
                    _popupPrefabMap[type] = popup;
                }
                else
                {
                    Debug.LogWarning($"Invalid popup type: {popup.name}");
                }
            }
        }

        public T CreatePopup<T>() where T : Popup
        {
            var type = typeof(T);
            if (!_popupPrefabMap.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"Popup prefab for type {type.Name} not found!");
                return null;
            }

            T newPopup = _diContainer.InstantiatePrefabForComponent<T>(prefab);

            return newPopup;
        }
    }
}