using UnityEngine;

namespace Battle.Attack
{
    [CreateAssetMenu(fileName = "SingleAttackPattern", menuName = "AttackPattern/Single")]
    public class SingleAttackPattern : AttackPattern
    {
        public override AttackType type => AttackType.Single;
    }
}