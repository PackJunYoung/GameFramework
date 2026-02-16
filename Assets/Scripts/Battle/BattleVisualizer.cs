using System.Collections.Generic;
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

        public void PlayInstantEvents(List<BattleEvent> events)
        {
            if (events == null || events.Count == 0)
                return;

            foreach (var e in events)
            {
                switch (e)
                {
                    case MoveEvent move:
                    {
                        if (_unitViews.TryGetValue(move.unitId, out var mover))
                        {
                            mover.OnMove(move.position);
                        }
                    }
                        break;
                    case AttackEvent attack:
                    {
                        if (_unitViews.TryGetValue(attack.targetId, out var target))
                        {
                            target.OnDamage(attack.damage);
                        }
                    } 
                        break;
                }
            }
        }
    }
}