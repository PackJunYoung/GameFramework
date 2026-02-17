namespace Battle.Event
{
    public class DieEvent : BattleEvent
    {
        public static DieEvent New(float timestamp, int unitId)
        {
            return new DieEvent()
            {
                timestamp = timestamp,
                unitId = unitId
            };
        }
    }
}