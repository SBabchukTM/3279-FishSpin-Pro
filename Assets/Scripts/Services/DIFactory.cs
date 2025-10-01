using UnityEngine;
using Zenject;

namespace Services
{
    public class DIFactory
    {
        private readonly DiContainer _container;

        public DIFactory(DiContainer container)
        {
            _container = container;
        }
        
        public T Create<T>(GameObject prefab)
        {
            var item = _container.InstantiatePrefabForComponent<T>(prefab);
            return item;
        }
    }
}