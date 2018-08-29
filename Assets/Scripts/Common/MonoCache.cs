using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Simple cache class that activate game objects for spawn and deactivate their for despawn.
    /// </summary>
    /// <typeparam name="T">MonoBehaviour script.</typeparam>
    public class MonoCache<T> where T : MonoBehaviour
    {
        private readonly GameObject original;
        private readonly Transform parent;

        private readonly List<T> activeItems = new List<T>();
        private readonly List<T> inactiveItems = new List<T>();

        public MonoCache(GameObject original, Transform parent)
        {
            var component = original.GetComponent<T>();
            if (component == null)
            {
                throw new ArgumentException(nameof(original));
            }

            this.original = original;
            this.parent = parent;
        }

        public T Spawn()
        {
            T element;

            if (inactiveItems.Count == 0)
            {
                // Automatically last.
                var obj = UnityEngine.Object.Instantiate(original, parent);
                element = obj.GetComponent<T>();
            }
            else
            {
                element = inactiveItems.Last();
                inactiveItems.Remove(element);
                element.gameObject.SetActive(true);

                // Move to the last position in hierarchy.
                element.transform.SetAsLastSibling();
            }

            activeItems.Add(element);
            return element;
        }

        public void Despawn(T item)
        {
            if (activeItems.Contains(item))
            {
                item.gameObject.SetActive(false);
                activeItems.Remove(item);
                inactiveItems.Add(item);
            }
        }

        public void DespawnAll()
        {
            while (activeItems.Count > 0)
            {
                var item = activeItems.Last();
                item.gameObject.SetActive(false);
                inactiveItems.Add(item);
                activeItems.Remove(item);
            }
        }

        public void DespawnAll(Action<T> despawnAction)
        {
            while (activeItems.Count > 0)
            {
                var item = activeItems.Last();
                despawnAction?.Invoke(item);
                item.gameObject.SetActive(false);
                inactiveItems.Add(item);
                activeItems.Remove(item);
            }
        }
    }
}
