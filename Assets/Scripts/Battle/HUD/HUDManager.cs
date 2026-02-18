using System.Collections.Generic;
using Base;
using Battle.View;
using Pool;
using UnityEngine;

namespace Battle.HUD
{
    public class HUDManager : BaseManager<HUDManager>
    {
        [SerializeField] private GameObject _hpBarPrefab;
        [SerializeField] private Transform _hudContainer; // 전용 캔버스 하위

        private Dictionary<int, HPBarItem> _hpBarMap = new();

        public void AttachHPBar(UnitView unit, float maxHp)
        {
            var go = _hpBarPrefab.Spawn(_hudContainer);
            var hpBar = go.GetComponent<HPBarItem>();
        
            // 유닛 머리 위 적절한 위치에 셋업
            hpBar.Setup(unit.transform, Vector3.up * 1f);
            hpBar.SetHealth(maxHp, maxHp);

            _hpBarMap[unit.Id] = hpBar;
        }

        public void UpdateHP(int unitId, float currentHp, float maxHp)
        {
            if (_hpBarMap.TryGetValue(unitId, out var hpBar))
            {
                hpBar.SetHealth(currentHp, maxHp);
            }
        }

        public void DetachHPBar(int unitId)
        {
            if (_hpBarMap.TryGetValue(unitId, out var hpBar))
            {
                hpBar.gameObject.Release();
                _hpBarMap.Remove(unitId);
            }
        }
    }
}