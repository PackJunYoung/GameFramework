namespace Battle
{
    public class AttackEvent : BattleEvent
    {
        public int attackerId;
        public int targetId;
        public float damage;
    }
}