using Base;
using Pool;
using UnityEngine;

namespace Battle.DamageText
{
    public class DamageTextManager : BaseManager<DamageTextManager>
    {
        [SerializeField] private GameObject _damageTextPrefab;
        [SerializeField] private Transform _canvasTransform;

        public void Spawn(HitResult hit, Vector3 worldPos)
        {
            // 1. 풀링으로 텍스트 객체 획득
            var go = _damageTextPrefab.Spawn(_canvasTransform);
            var damageText = go.GetComponent<DamageText>();

            // 2. 가독성을 위한 약간의 랜덤 위치 수정
            var offset = new Vector3(Random.Range(-0.5f, 0.5f), 1.5f, 0);
        
            // 3. 연출 시작
            if (hit.isMissed)
            {
                damageText.Show("MISS", worldPos + offset, Color.grey, 15);
            }
            else if (hit.isBlocked)
            {
                damageText.Show($"<size=50%>BLOCK</size>\n{Mathf.FloorToInt(hit.finalDamage)}", worldPos + offset, Color.cyan, 20);
            }
            else if (hit.isCritical)
            {
                damageText.Show($"{Mathf.FloorToInt(hit.finalDamage)}", worldPos + offset, Color.red, 25);
            }
            else
            {
                damageText.Show($"{Mathf.FloorToInt(hit.finalDamage)}", worldPos + offset, Color.yellow, 20);
            }
        }
    }
}