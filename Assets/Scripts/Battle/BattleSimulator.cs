using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Event;
using UnityEngine;

namespace Battle
{
    public partial class BattleSimulator
    {
        private float _currentTime;
        private System.Random _random;

        private Dictionary<int, UnitState> _unitDict = new Dictionary<int, UnitState>();
        private List<UnitState> _unitList = new List<UnitState>();

        public BattleSimulator(int seed, List<UnitState> units)
        {
            _random = new System.Random(seed);
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
                unit.ChangeAction(UnitActionState.Attack);
                events.Add(AttackStartEvent.New(_currentTime, unit.id));
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
            events.Add(MoveEvent.New(_currentTime, unit.id, unit.position));
        }

        private void HandleAttack(List<BattleEvent> events, UnitState unit, float dt)
        {
            var target = GetTarget(unit.targetId);
            if (target == null)
            {
                unit.ChangeAction(UnitActionState.Idle);
                return;
            }

            unit.actionGauge += unit.AttackSpeed * dt;
            if (unit.afterHit)
            {
                if (unit.actionGauge >= unit.PostAttackDelay)
                {
                    unit.ChangeAction(UnitActionState.Idle);
                    events.Add(AttackEndEvent.New(_currentTime, unit.id));
                }
            }
            else
            {
                if (unit.actionGauge >= unit.PreAttackDelay)
                {
                    var hit = Calculate(unit, target);
                    unit.DoHit(hit);
                    target.OnHit(hit);
                    if (unit.currentActionState == UnitActionState.Die)
                    {
                        events.Add(DieEvent.New(_currentTime, unit.id));
                    }
                    else
                    {
                        if (hit.reflectedDamage > 0f) events.Add(DamageEvent.New(_currentTime, unit.id, hit.reflectedDamage, unit.curHp, unit.MaxHp));
                        if (hit.lifeStealAmount > 0f) events.Add(RecoveryEvent.New(_currentTime, unit.id, hit.lifeStealAmount, unit.curHp, unit.MaxHp));
                    }
                    
                    if (target.currentActionState == UnitActionState.Die)
                    {
                        events.Add(DieEvent.New(_currentTime, target.id));
                    }
                    else
                    {
                        events.Add(HitEvent.New(_currentTime, target.id, hit, target.curHp, target.MaxHp));
                    }
                }
            }
        }

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