using System.Collections.Generic;
using UnityEngine;

public class TeamPoint : MonoBehaviour
{
    [SerializeField] private int _teamIndex;
    
    private List<Transform> _points;

    public int Index => _teamIndex;

    public Transform GetPoint(int index)
    {
        if (_points == null)
        {
            _points = new List<Transform>();
            foreach (Transform child in transform) // transform을 순회하면 직계 자식만 나옵니다.
            {
                _points.Add(child);
            }
        }

        if (index < _points.Count)
        {
            return _points[index];
        }

        return null;
    }
}
