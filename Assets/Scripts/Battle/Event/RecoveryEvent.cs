namespace Battle.Event
{
    public class RecoveryEvent : BattleEvent
    {
        public float amount;
        public float curHp;
        public float maxHp;

        public static RecoveryEvent New(float timestamp, int unitId, float amount, float curHp, float maxHp)
        {
            return new RecoveryEvent()
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