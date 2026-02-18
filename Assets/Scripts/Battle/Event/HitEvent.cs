namespace Battle.Event
{
    public class HitEvent : BattleEvent
    {
        public float damage;
        public float curHp;
        public float maxHp;

        public static HitEvent New(float timestamp, int unitId, float damage, float curHp, float maxHp)
        {
            return new HitEvent()
            {
                timestamp = timestamp,
                unitId = unitId,
                damage = damage,
                curHp = curHp,
                maxHp = maxHp
            };
        }
    }
}