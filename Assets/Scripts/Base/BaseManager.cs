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
                // 1. 씬에 이미 있는지 확인
                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    // 2. Resources 폴더에서 클래스 이름과 같은 프리팹이 있는지 확인
                    var prefab = Resources.Load<T>(typeof(T).Name);
                    if (prefab != null)
                    {
                        // 프리팹이 있다면 인스턴스화
                        _instance = Instantiate(prefab);
                        _instance.name = typeof(T).Name;
                    }
                    else
                    {
                        // 프리팹이 없다면 기존처럼 빈 오브젝트 생성
                        var go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }
                        
                    _instance.Initialize();
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}