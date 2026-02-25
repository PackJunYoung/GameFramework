namespace Battle.Event
{
    public class DamageEvent : BattleEvent
    {
        public float amount;
        public float curHp;
        public float maxHp;

        public static DamageEvent New(float timestamp, int unitId, float amount, float curHp, float maxHp)
        {
            return new DamageEvent()
            {
                timestamp = timestamp,
                unitId = unitId,
                amount = amount,
                curHp = curHp,
                maxHp = maxHp
            };
        }
    }
}