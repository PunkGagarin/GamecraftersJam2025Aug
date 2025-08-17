using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Utils.Popup
{
    public class PopupManager : MonoBehaviour
    {
        [Inject] private PopupFactory _factory;
        private HashSet<Popup> _popups = new();

        public T OpenPopup<T>(Action openEvent = null, Action closeEvent = null, bool withPause = false)
            where T : Popup
        {
            foreach (T popup in _popups.OfType<T>())
            {
                popup.SetOpenEvent(openEvent);
                popup.Open(withPause);
                popup.transform.SetAsLastSibling();
                popup.SetCloseEvent(closeEvent);
                return popup;
            }

            return AssignNewPopup<T>(openEvent, closeEvent, withPause);
        }

        public void ResetPopup<T>() where T : Popup
        {
            var popupToRemove = _popups.FirstOrDefault(popup => popup.GetType() == typeof(T));
            if (popupToRemove != null)
                _popups.Remove(popupToRemove);
        }

        public bool IsPopupOpen<T>(out T popup) where T : Popup
        {
            popup = (T)_popups.FirstOrDefault(popup => popup.GetType() == typeof(T));
            return popup != null && popup.gameObject.activeSelf;
        }

        private T AssignNewPopup<T>(Action openEvent, Action closeEvent, bool withPause) where T : Popup
        {
            T newPopup = _factory.CreatePopup<T>();
            newPopup.transform.SetParent(transform, false);
            newPopup.SetOpenEvent(openEvent);
            newPopup.Open(withPause);
            newPopup.SetCloseEvent(closeEvent);
            _popups.Add(newPopup);
            return newPopup;
        }
    }
}