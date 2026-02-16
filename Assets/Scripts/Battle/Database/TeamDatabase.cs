using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle.Database
{
    [CreateAssetMenu(fileName = "TeamDatabase", menuName = "Battle/TeamDatabase")]
    public class TeamDatabase : ScriptableObject
    {
        public List<TeamData> teams;

        public TeamData GetTeam(int teamId)
        {
            return teams.FirstOrDefault(i => i.teamId == teamId);
        }
    }

    [Serializable]
    public class TeamData
    {
        public int teamId;
        public List<string> unitIds;
    }
}