using Battle.Attack;
using Database;
using UnityEngine;
using AttackType = Database.AttackType;

namespace Battle
{
    public class UnitState
    {
        public int id;
        public int teamId;
        public UnitData data;
        
        public UnitActionState currentActionState = UnitActionState.Idle;
        public AttackPattern currentAttackPattern;
        public Vector3 position;
        public float actionGauge;
        public int targetId;
        public bool afterHit;
        public float curHp;
        public int level;

        public AttackType AttackType => data.attackType;
        public float MoveSpeed => data.statSet ? data.statSet.moveSpeed : 0f;
        public float MaxHp => data.statSet ? data.statSet.hp : 0f;
        public float Attack => data.statSet ? data.statSet.attack : 0f;
        public float PDefense => data.statSet ? data.statSet.pDefense : 0f;
        public float MDefense => data.statSet ? data.statSet.mDefense : 0f;
        public float AttackSpeed => data.statSet ? data.statSet.attackSpeed : 0f;
        public float CooldownReduction => data.statSet ? data.statSet.cooldownReduction : 0f;
        public float Penetration => data.statSet ? data.statSet.penetration : 0f;
        public float CriticalRate => data.statSet ? data.statSet.criticalRate : 0f;
        public float CriticalDamage => data.statSet ? data.statSet.criticalDamage : 0f;
        public float CriticalResist => data.statSet ? data.statSet.criticalResist : 0f;
        public float Accuracy => data.statSet ? data.statSet.accuracy : 0f;
        public float Evasion => data.statSet ? data.statSet.evasion : 0f;
        public float BlockPenetration => data.statSet ? data.statSet.blockPenetration : 0f;
        public float BlockRate => data.statSet ? data.statSet.blockRate : 0f;

        public float ApproachRange => currentAttackPattern != null ? currentAttackPattern.approachRange : 0f;
        public float PreAttackDelay => currentAttackPattern != null ? currentAttackPattern.preAttackDelay : 0f;
        public float PostAttackDelay => currentAttackPattern != null ? currentAttackPattern.postAttackDelay : 0f;

        public float HealingPower;
        public float HealingReceived;
        public float LiftSteal;
        public float ReflectDamage;
        public float DamageIncrease;
        public float DamageReduction;
        
        public bool IsDie => currentActionState == UnitActionState.Die || curHp <= 0;

        public UnitState(int id, int teamId, Vector3 position, UnitData data)
        {
            this.id = id;
            this.teamId = teamId;
            this.position = position;
            this.data = data;

            curHp = MaxHp;
        }

        public void ChangeAction(UnitActionState action)
        {
            currentActionState = action;
            switch (action)
            {
                case UnitActionState.Idle:
                    actionGauge = 0;
                    afterHit = false;
                    break;
                case UnitActionState.Move:
                    break;
                case UnitActionState.Attack:
                    actionGauge = 0;
                    break;
                case UnitActionState.Die:
                    break;
            }
        }

        public void DoHit(HitResult hit)
        {
            afterHit = true;
            actionGauge = 0f;
            ApplyHeal(hit.lifeStealAmount);
            ApplyDamage(hit.reflectedDamage);
        }

        public void OnHit(HitResult hit)
        {
            ApplyDamage(hit.finalDamage);
        }

        public void RenewCurrentAttackPattern()
        {
            currentAttackPattern = data?.baseAttackPattern ? data.baseAttackPattern : null;
        }

        private void ApplyDamage(float amount)
        {
            if (IsDie) return;
            
            curHp -= amount;
            if (curHp <= 0f)
            {
                curHp = 0f;
                currentActionState = UnitActionState.Die;
            }
        }

        private void ApplyHeal(float amount)
        {
            if (IsDie) return;

            curHp += amount;
        }
    }
}