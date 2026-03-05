using UnityEngine;

namespace Battle.Attack
{
    public enum AttackType
    {
        Single,
        Area,
        Projectile,
    }
    
    public abstract class AttackPattern : ScriptableObject
    {
        public abstract AttackType type { get; }
        
        public float preAttackDelay;
        public float postAttackDelay;
        public float approachRange;
        public float baseMultiplier;
    }
}