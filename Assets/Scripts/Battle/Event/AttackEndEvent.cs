namespace Battle.Event
{
    public class AttackEndEvent : BattleEvent
    {
        public static AttackEndEvent New(float timestamp, int unitId)
        {
            return new AttackEndEvent()
            {
                timestamp = timestamp,
                unitId = unitId
            };
        }
    }
}