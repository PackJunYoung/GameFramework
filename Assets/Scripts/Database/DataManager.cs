using Base;
using UnityEngine;

namespace Database
{
    public class DataManager : BaseManager<DataManager>
    {
        private MasterDatabase _master;
        
        protected override void Initialize()
        {
            _master = Resources.Load<MasterDatabase>("MasterDatabase");
        }

        public static UnitDatabase GetUnitDatabase()
        {
            return Instance._master.unit;
        }

        public static TeamDatabase GetTeamDatabase()
        {
            return Instance._master.team;
        }
    }
}