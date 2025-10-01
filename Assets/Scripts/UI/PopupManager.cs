using System;
using System.Collections.Generic;
using Services;
using UI.Popups;
using UnityEngine;

namespace UI
{
    public class PopupManager
    {
        private readonly RectTransform _canvas;

        public PopupManager(BasePopup[] popups, RectTransform canvas)
        {
            _canvas = canvas;
        }

        public T CreatePopup<T>(Dictionary<Type, BasePopup> gamePopups, RectTransform canvas, DIFactory factory) where T : BasePopup
        {
            if (gamePopups.TryGetValue(typeof(T), out BasePopup prefab))
            {
                var popup = factory.Create<T>(prefab.gameObject);
                popup.transform.SetParent(canvas, false);
                return popup as T;
            }

            throw new Exception($"Popup {typeof(T)} not found");
        }
    }
}