using UnityEngine;

namespace Database
{
    [CreateAssetMenu(fileName = "MasterDatabase", menuName = "Battle/MasterDatabase")]
    public class MasterDatabase : ScriptableObject
    {
        public UnitDatabase unit;
        public TeamDatabase team;
    }
}
