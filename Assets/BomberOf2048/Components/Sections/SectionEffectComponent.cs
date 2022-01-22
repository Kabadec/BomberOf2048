using DG.Tweening;
using UnityEngine;

namespace BomberOf2048.Components.Sections
{
    public class SectionEffectComponent : MonoBehaviour
    {
        [SerializeField] private float _timeAnimation;

        [SerializeField] private SpriteRenderer _renderer;
        
        private Sequence _sequence;

        private void OnEnable()
        {
            DoAnim();
        }
        
        [ContextMenu("DoAnim")]
        private void DoAnim()
        {
            var startColor = new Color(1f, 1f, 1f, 1f);
            var endColor = new Color(1f, 1f, 1f, 0f);
            _renderer.color = startColor;
            
            _sequence = DOTween.Sequence();

            _sequence.Append(_renderer.DOColor(endColor, _timeAnimation / 2).SetEase(Ease.InOutQuad));
            _sequence.Append(_renderer.DOColor(startColor, _timeAnimation / 2).SetEase(Ease.InOutQuad));

            _sequence.AppendCallback(() =>
            {
                _sequence.Kill();
                DoAnim();
                //Debug.Log("End Animation");
            });
        }


        private void OnDisable()
        {
            _sequence.Kill();
        }
    }
}