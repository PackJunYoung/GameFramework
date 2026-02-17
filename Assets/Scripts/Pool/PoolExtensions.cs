using UnityEngine;

namespace Pool
{
    public static class PoolExtensions
    {
        public static GameObject Spawn(this GameObject prefab,Transform parent = null,  Vector3 pos = default, Quaternion rot = default)
        {
            return PoolManager.Instance.Get(prefab, parent, pos, rot);
        }

        public static void Release(this GameObject obj)
        {
            PoolManager.Instance.Release(obj);
        }
    }
}