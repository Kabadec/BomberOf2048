using BomberOf2048.Utils;
using DG.Tweening;
using UnityEngine;

namespace BomberOf2048.Components.Sections
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SectionEffectLerpColorComponent : Singleton<SectionEffectLerpColorComponent>
    {
        [SerializeField] private float _timeAnimation;

        [SerializeField] private SpriteRenderer _renderer;
        
        private Sequence _sequence;

        private readonly Color _startColor = new Color(1f, 1f, 1f, 1f);
        private readonly Color _endColor = new Color(1f, 1f, 1f, 0f);

        public Color LerpColor => _renderer.color;

        private void OnEnable()
        {
            DoAnim();
        }
        
        [ContextMenu("DoAnim")]
        private void DoAnim()
        {
            _renderer.color = _startColor;
            var sequence = DOTween.Sequence();
            
            sequence.Append(_renderer.DOColor(_endColor, _timeAnimation / 2).SetEase(Ease.InOutQuad));
            sequence.Append(_renderer.DOColor(_startColor, _timeAnimation / 2).SetEase(Ease.InOutQuad));

            sequence.AppendCallback(() =>
            {
                _sequence.Kill();
                DoAnim();
            });
        }

        
        private void OnDisable()
        {
            _sequence.Kill();
        }
    }
}