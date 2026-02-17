using System.Collections.Generic;
using System.Linq;
using Battle;
using Database;
using UnityEngine;

public class BattleContainer : MonoBehaviour
{
    public List<UnitState> GetUnitInfos(int teamA, int teamB)
    {
        var teamPoints = GetComponentsInChildren<TeamPoint>();
        var teamPointA = teamPoints.FirstOrDefault(i => i.Index == teamA);
        var teamPointB = teamPoints.FirstOrDefault(i => i.Index == teamB);
        if (teamPointA == null || teamPointB == null)
            throw new MissingReferenceException();

        var teamDatabase = DataManager.GetTeamDatabase();
        var unitDatabase = DataManager.GetUnitDatabase();
        var unitInfos = new List<UnitState>();
        var teamAData = teamDatabase.teams.FirstOrDefault(i => i.teamId == teamA);
        var teamBData = teamDatabase.teams.FirstOrDefault(i => i.teamId == teamB);
        if (teamAData == null || teamBData == null) 
            return unitInfos;

        var id = 0;
        for (var i = 0; i < teamAData.unitIds.Count; i++)
        {
            var unitId = teamAData.unitIds[i];
            var unitData = unitDatabase.units.FirstOrDefault(data => data.unitId == unitId);
            if (unitData == null) continue;
            var unitState = new UnitState(id++, 0, teamPointA.GetPoint(i).position, unitData);
            unitInfos.Add(unitState);
        }

        for (var i = 0; i < teamBData.unitIds.Count; i++)
        {
            var unitId = teamBData.unitIds[i];
            var unitData = unitDatabase.units.FirstOrDefault(data => data.unitId == unitId);
            if (unitData == null) continue;
            var unitState = new UnitState(id++, 1, teamPointB.GetPoint(i).position, unitData);
            unitInfos.Add(unitState);
        }

        return unitInfos;
    }
}
