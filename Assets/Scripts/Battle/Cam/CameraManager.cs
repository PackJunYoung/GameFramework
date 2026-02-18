using Base;
using DG.Tweening;
using UnityEngine;

namespace Battle.Cam
{
    public class CameraManager : BaseManager<CameraManager>
    {
        private Camera _mainCam;
        private float _defaultFOV;
        private Vector3 _originPos;
        private Sequence _zoomSequence;

        public Camera GetCamera() => _mainCam; 

        protected override void Initialize()
        {
            _mainCam = GetComponent<Camera>();
            _defaultFOV = _mainCam.fieldOfView;
            _originPos = _mainCam.transform.position;
        }

        /// <summary>
        /// 카메라 흔들림 (피격, 폭발 시)
        /// </summary>
        public void Shake(float duration = 0.2f, float strength = 0.5f, int vibrato = 10)
        {
            // 쉐이크 전 위치 초기화 (누적 방지)
            _mainCam.transform.DOComplete();
            _mainCam.transform.DOShakePosition(duration, strength, vibrato);
        }

        /// <summary>
        /// 특정 지점으로 순간적인 줌인/아웃 (스킬 사용 시)
        /// </summary>
        public void ZoomImpact(float targetFOV = 40f, float duration = 0.1f)
        {
            _zoomSequence?.Kill();
            _zoomSequence = DOTween.Sequence();

            _zoomSequence.Append(_mainCam.DOFieldOfView(targetFOV, duration))
                .Append(_mainCam.DOFieldOfView(_defaultFOV, duration * 2f).SetEase(Ease.OutQuad));
        }

        /// <summary>
        /// 게임 전체 속도 조절 (슬로우 모션 연출)
        /// </summary>
        public void SlowMotion(float timeScale, float duration, float delay = 0f)
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScale, 0.1f)
                .SetUpdate(true) // 타임스케일이 변해도 트윈은 돌아가야 함
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(duration,
                        () =>
                        {
                            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.2f).SetUpdate(true);
                        }, false).SetUpdate(true);
                });
        }

        /// <summary>
        /// 카메라를 살짝 뒤로 밀었다가 복구 (강한 타격감)
        /// </summary>
        public void Kickback(Vector3 direction, float strength = 0.2f)
        {
            _mainCam.transform.DOMove(_originPos + (direction.normalized * strength), 0.1f)
                .OnComplete(() => _mainCam.transform.DOMove(_originPos, 0.3f).SetEase(Ease.OutBack));
        }
    }
}