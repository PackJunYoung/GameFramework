using UnityEngine;
using UnityEngine.Pool;

namespace Pool
{
    public class PoolItem : MonoBehaviour
    {
        public IObjectPool<GameObject> myPool;
    }
}