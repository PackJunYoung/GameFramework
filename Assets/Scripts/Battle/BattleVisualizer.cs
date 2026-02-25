using System.Collections.Generic;
using Battle.Event;
using Battle.View;
using Pool;
using UnityEngine;

namespace Battle
{
    public class BattleVisualizer : MonoBehaviour
    {
        private Dictionary<int, UnitView> _unitViews = new Dictionary<int, UnitView>();

        public void SpawnInitialUnits(List<UnitState> units)
        {
            _unitViews.Clear();
            foreach (var unit in units)
            {
                var go = unit.data.prefab.Spawn(transform, unit.position);
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
                            unit.PlayMove(move.position);
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
                            unit.PlayHit(hit);
                        }
                            break;
                        case DamageEvent damage:
                        {
                            unit.PlayDamage(damage);
                        }
                            break;
                        case RecoveryEvent recovery:
                        {
                            unit.PlayRecovery(recovery);
                        }
                            break;
                        case DieEvent die:
                        {
                            // 사망
                            unit.PlayDie();
                        }
                            break;
                    }
                }
            }
        }
    }
}