using Base;
using Pool;
using UnityEngine;

namespace Battle.DamageText
{
    public class DamageTextManager : BaseManager<DamageTextManager>
    {
        [SerializeField] private GameObject _damageTextPrefab;
        [SerializeField] private Transform _canvasTransform;

        public void Spawn(float damage, Vector3 worldPos)
        {
            // 1. 풀링으로 텍스트 객체 획득
            var go = _damageTextPrefab.Spawn(_canvasTransform);
            var damageText = go.GetComponent<DamageText>();

            // 2. 가독성을 위한 약간의 랜덤 위치 수정
            var offset = new Vector3(Random.Range(-0.5f, 0.5f), 1.5f, 0);
        
            // 3. 연출 시작
            damageText.Show(damage, worldPos + offset);
        }
    }
}