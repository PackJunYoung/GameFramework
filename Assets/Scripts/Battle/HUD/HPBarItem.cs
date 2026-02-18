using Battle.Cam;
using DG.Tweening;
using Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.HUD
{
    public class HPBarItem : MonoBehaviour
    {
        [SerializeField] private Image _mainBar;
        [SerializeField] private Image _ghostBar; // 뒤따라오는 바
        [SerializeField] private float _followSpeed = 0.5f;

        private Transform _target;
        private Vector3 _offset;

        public void Setup(Transform target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
            UpdatePosition();
        }

        public void SetHealth(float currentHp, float maxHp)
        {
            var ratio = currentHp / maxHp;

            // 즉시 메인 바 변경
            _mainBar.fillAmount = ratio;

            // 잔상 바는 부드럽게 뒤따라옴 (타격감 상승)
            _ghostBar.DOFillAmount(ratio, _followSpeed).SetEase(Ease.OutQuad);
        }

        private void LateUpdate()
        {
            if (_target == null)
            {
                gameObject.Release();
                return;
            }
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var mainCam = CameraManager.Instance.GetCamera();
            var screenPos = mainCam.WorldToScreenPoint(_target.position + _offset);
        
            // 카메라 뒤에 있으면 렌더링 방지
            if (screenPos.z < 0) 
            {
                if(transform.localScale != Vector3.zero) transform.localScale = Vector3.zero;
                return;
            }
        
            transform.localScale = Vector3.one;
            transform.position = new Vector3(screenPos.x, screenPos.y, 0);
        }
    }
}