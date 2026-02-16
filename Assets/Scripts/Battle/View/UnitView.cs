using UnityEngine;

namespace Battle.View
{
    public class UnitView : BaseView
    {
        private Vector3 _logicPosition;

        // 초기화
        public void Initialize(UnitState state)
        {
            Initialize(state.id);
            
            _logicPosition = state.position;
        }
        
        // 목표 위치로 이동
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _logicPosition, Time.deltaTime * 15f);
        }

        // 목표 위치를 변경
        public void OnMove(Vector3 newPos)
        {
            _logicPosition = newPos;
        }

        // 피격
        public void OnDamage(float damage)
        {
            FlashView();
        }
    }
}
