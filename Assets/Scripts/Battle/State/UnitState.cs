using Database;
using UnityEngine;

namespace Battle
{
    public class UnitState
    {
        public int id;
        public int teamId;
        public UnitData data;
        
        public UnitActionState currentActionState = UnitActionState.Idle;
        public Vector3 position;
        public float actionGauge;
        public int targetId;
        public bool afterHit;
        public float curHp;
        public int level;

        public AttackType AttackType => data.attackType;
        public float MoveSpeed => data.moveSpeed;
        public float AttackRange => data.attackRange;
        public float PreAttackDelay => data.preAttackDelay;
        public float PostAttackDelay => data.postAttackDelay;
        
        public float MaxHp => data.hp;
        public float Attack => data.attack;
        public float PDefense => data.pDefense;
        public float MDefense => data.mDefense;
        public float AttackSpeed => data.attackSpeed;
        public float CooldownReduction => data.cooldownReduction;
        public float Penetration => data.penetration;
        public float CriticalRate => data.criticalRate;
        public float CriticalDamage => data.criticalDamage;
        public float CriticalResist => data.criticalResist;
        public float Accuracy => data.accuracy;
        public float Evasion => data.evasion;
        public float BlockPenetration => data.blockPenetration;
        public float BlockRate => data.blockRate;

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
            
            curHp = data.hp;
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