using Battle.Database;
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
        public float attackSpeed;

        public float Atk => data.atk;
        public float MoveSpeed => data.moveSpeed;
        public float AttackRange => data.attackRange;
        public float PreAttackDelay => data.preAttackDelay;
        public float HitAttackDelay => data.hitAttackDelay;
        public float PostAttackDelay => data.postAttackDelay;
        public bool IsDie => currentActionState == UnitActionState.Die || curHp <= 0;

        public UnitState(int id, int teamId, Vector3 position, UnitData data)
        {
            this.id = id;
            this.teamId = teamId;
            this.position = position;
            this.data = data;
            
            curHp = data.hp;
            attackSpeed = 1f;
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

        public void OnHit()
        {
            afterHit = true;
            actionGauge = 0f;
        }
    }
}