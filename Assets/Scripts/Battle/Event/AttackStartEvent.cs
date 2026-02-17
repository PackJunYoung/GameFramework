namespace Battle.Event
{
    public class AttackStartEvent : BattleEvent
    {
        public static AttackStartEvent New(float timestamp, int unitId)
        {
            return new AttackStartEvent()
            {
                timestamp = timestamp,
                unitId = unitId
            };
        }
    }
}