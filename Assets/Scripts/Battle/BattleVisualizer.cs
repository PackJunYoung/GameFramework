using System.Collections.Generic;
using Battle.Event;
using Battle.View;
using UnityEngine;

namespace Battle
{
    public class BattleVisualizer : MonoBehaviour
    {
        [Header("Prefabs")] 
        public GameObject UnitPrefab;

        [Header("Container")] 
        public Transform UnitContainer;

        private Dictionary<int, UnitView> _unitViews = new Dictionary<int, UnitView>();

        public void SpawnInitialUnits(List<UnitState> units)
        {
            _unitViews.Clear();
            foreach (var unit in units)
            {
                var go = Instantiate(UnitPrefab, unit.position, Quaternion.identity, UnitContainer);
                var view = go.GetComponent<UnitView>();
                view.Initialize(unit);
                _unitViews[unit.id] = view;
            }
        }

        public void PlayEvents(List<BattleEvent> events)
        {
            if (events == null || events.Count == 0)
                return;

            foreach (var e in events)
            {
                if (_unitViews.TryGetValue(e.unitId, out var unit))
                {
                    switch (e)
                    {
                        case MoveEvent move:
                        {
                            unit.OnMove(move.position);
                        }
                            break;
                        case AttackStartEvent attackStart:
                        {
                            // 대기 모션 전환
                        }
                            break;
                        case AttackEndEvent attackEnd:
                        {
                            // 공격 모션 전환
                        }
                            break;
                        case HitEvent hit:
                        {
                            unit.OnDamage(hit.damage);
                        }
                            break;
                        case DieEvent die:
                        {
                            // 사망
                            unit.OnDie();
                        }
                            break;
                    }
                }
            }
        }
    }
}