using UnityEngine;

namespace Base
{
    public abstract class BaseManager<T> : MonoBehaviour where T : BaseManager<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        var go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }

                    _instance.Initialize();
                }

                return _instance;
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}