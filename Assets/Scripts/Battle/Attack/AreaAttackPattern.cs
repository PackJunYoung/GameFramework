using UnityEngine;

namespace Battle.Attack
{
    [CreateAssetMenu(fileName = "AreaAttackPattern", menuName = "AttackPattern/Area")]
    public class AreaAttackPattern : AttackPattern
    {
        public override AttackType type => AttackType.Area;

        public float attackRange;
        public float sectorAngle;
    }
}