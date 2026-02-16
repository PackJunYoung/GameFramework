using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Battle
{
    public class BattleSimulator
    {
        private float _currentTime;
        private Random _random;

        private Dictionary<int, UnitState> _unitDict = new Dictionary<int, UnitState>();
        private List<UnitState> _unitList = new List<UnitState>();

        public BattleSimulator(int seed, List<UnitState> units)
        {
            _random = new Random(seed);
            _currentTime = 0f;
            units.ForEach(AddUnit);
        }
        
        public List<BattleEvent> Tick(float dt) {
            _currentTime += dt;

            // 1. 유저 개입 처리 (스킬 등)
            // if (inputs != null) {
            //     foreach (var input in inputs) {
            //         // 스킬 로직 처리 (생략)
            //     }
            // }

            // 2. 유닛 AI 로직
            var events = new List<BattleEvent>();
            foreach (var unit in _unitList.Where(u => !u.IsDie)) {
                ProcessUnit(events, unit, dt);
            }
            
            return events;
        }

        private void ProcessUnit(List<BattleEvent> events, UnitState unit, float dt)
        {
            switch (unit.currentActionState)
            {
                case UnitActionState.Idle:
                    HandleIdle(events, unit, dt);
                    break;
                case UnitActionState.Move:
                    HandleMove(events, unit, dt);
                    break;
                case UnitActionState.Attack:
                    HandleAttack(events, unit, dt);
                    break;
            }
        }

        private void HandleIdle(List<BattleEvent> events, UnitState unit, float dt)
        {
            var target = FindClosestTarget(unit);
            if (target == null) return;

            unit.targetId = target.id;
            var dist = Vector3.Distance(unit.position, target.position);
            if (dist <= unit.AttackRange)
            {
                unit.actionGauge += unit.attackSpeed * dt;
                if (unit.actionGauge >= unit.PreAttackDelay) {
                    unit.ChangeAction(UnitActionState.Attack);
                }
            } else {
                unit.ChangeAction(UnitActionState.Move);
            }
        }

        private void HandleMove(List<BattleEvent> events, UnitState unit, float dt)
        {
            var target = GetTarget(unit.targetId);
            if (target == null)
            {
                unit.ChangeAction(UnitActionState.Idle);
                return;
            }

            var dist = Vector3.Distance(unit.position, target.position);
            if (dist <= unit.AttackRange)
            {
                unit.ChangeAction(UnitActionState.Idle);
                return;
            }

            var dir = (target.position - unit.position).normalized;
            unit.position += dir * unit.MoveSpeed * dt;
            events.Add(new MoveEvent { timestamp = _currentTime, unitId = unit.id, position = unit.position });
        }

        private void HandleAttack(List<BattleEvent> events, UnitState unit, float dt)
        {
            var target = GetTarget(unit.targetId);
            if (target == null)
            {
                unit.ChangeAction(UnitActionState.Idle);
                return;
            }

            unit.actionGauge += unit.attackSpeed * dt;
            if (unit.afterHit)
            {
                if (unit.actionGauge >= unit.PostAttackDelay)
                {
                    unit.ChangeAction(UnitActionState.Idle);
                }
            }
            else
            {
                if (unit.actionGauge >= unit.HitAttackDelay)
                {
                    unit.OnHit();
                    events.Add(new AttackEvent { timestamp = _currentTime, attackerId = unit.id, targetId = target.id, damage = unit.Atk });
                }
            }
        }

        // [밸런스 테스트용] 백그라운드 초고속 시뮬레이션
        // public async Task<List<BattleEvent>> RunSimulationAsync() {
        //     return await Task.Run(() => {
        //         var totalLogs = new List<BattleEvent>();
        //         while (_currentTime < 300f && GetWinnerTeam() == 0) {
        //             totalLogs.AddRange(Tick(0.05f)); // 0.05초씩 강제로 시간을 워프
        //         }
        //         return totalLogs;
        //     });
        // }

        private void AddUnit(UnitState unit)
        {
            _unitDict.Add(unit.id, unit);
            _unitList.Add(unit);
        }

        private void RemoveUnit(UnitState unit)
        {
            _unitDict.Remove(unit.id);
            _unitList.Remove(unit);
        }

        private UnitState FindClosestTarget(UnitState self)
        {
            return  _unitList
                .Where(u => u.teamId != self.teamId && !u.IsDie)
                .OrderBy(u => Vector3.Distance(self.position, u.position)).FirstOrDefault();
        }
        
        private UnitState GetTarget(int id)
        {
            if (_unitDict.TryGetValue(id, out var target))
            {
                if (!target.IsDie)
                    return target;
            }
            
            return null;
        }

        public int GetWinnerTeam() {
            var aliveTeams = _unitList.Where(u => !u.IsDie).Select(u => u.teamId).Distinct().ToList();
            return aliveTeams.Count == 1 ? aliveTeams[0] : 0;
        }
    }
}