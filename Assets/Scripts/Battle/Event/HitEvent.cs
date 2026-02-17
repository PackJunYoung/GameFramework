namespace Battle.Event
{
    public class HitEvent : BattleEvent
    {
        public float damage;

        public static HitEvent New(float timestamp, int unitId, float damage)
        {
            return new HitEvent()
            {
                timestamp = timestamp,
                unitId = unitId,
                damage = damage
            };
        }
    }
}