using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services;
using UI.Screens;
using UnityEngine;

namespace UI
{
    public class ScreenManager
    {
        private readonly RectTransform _canvas;

        public ScreenManager(BaseScreen[] screens, RectTransform canvas)
        {
            _canvas = canvas;
        }

        public async UniTask<T> CreateScreen<T>(Dictionary<Type, BaseScreen> gameScreens, List<BaseScreen> openedScreens, DIFactory factory, UIFader fader) where T : BaseScreen
        {
            if (gameScreens.TryGetValue(typeof(T), out BaseScreen prefab))
            {
                var screen = factory.Create<T>(prefab.gameObject);
                openedScreens.Add(screen);
                screen.transform.SetParent(_canvas, false);

                await fader.FadeOut();

                return screen as T;
            }

            throw new Exception($"Screen {typeof(T)} not found");
        }

        public async UniTask HideScreen<T>(List<BaseScreen> openedScreens, UIFader uiFader) where T : BaseScreen
        {
            for (var i = 0; i < openedScreens.Count; i++)
                if (openedScreens[i].GetType() == typeof(T))
                {
                    var screen = openedScreens[i];
                    openedScreens.RemoveAt(i);

                    await uiFader.FadeIn();

                    UnityEngine.Object.Destroy(screen.gameObject);
                }
        }
    }
}