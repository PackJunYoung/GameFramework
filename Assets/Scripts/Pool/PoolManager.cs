using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.Pool;

namespace Pool
{
    public class PoolManager : BaseManager<PoolManager>
    {
        private Dictionary<GameObject, IObjectPool<GameObject>> _pools = new();

        /// <summary>
        /// 오브젝트를 풀에서 가져옵니다.
        /// </summary>
        public GameObject Get(GameObject prefab, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
        {
            if (!_pools.ContainsKey(prefab))
            {
                _pools.Add(prefab, CreatePool(prefab));
            }

            var obj = _pools[prefab].Get();
            obj.transform.SetParent(parent);
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        /// <summary>
        /// 오브젝트를 풀로 반환합니다.
        /// </summary>
        public void Release(GameObject obj)
        {
            if (obj.TryGetComponent<PoolItem>(out var item)) {
                obj.transform.SetParent(transform);
                item.myPool.Release(obj);
            } else {
                Destroy(obj);
            }
        }

        private IObjectPool<GameObject> CreatePool(GameObject prefab)
        {
            return new ObjectPool<GameObject>(
                createFunc: () => {
                    var obj = Instantiate(prefab);
                    var item = obj.AddComponent<PoolItem>();
                    item.myPool = _pools[prefab]; 
                    return obj;
                }, // 생성 로직
                actionOnGet: obj => obj.SetActive(true),           // 꺼낼 때
                actionOnRelease: obj => obj.SetActive(false),      // 넣을 때
                actionOnDestroy: obj => Destroy(obj),              // 풀 용량 초과 시 파괴
                collectionCheck: true,                             // 중복 반환 체크 (에러 방지)
                defaultCapacity: 10,                               // 기본 용량
                maxSize: 100                                       // 최대 용량
            );
        }
    }
}