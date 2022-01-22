using UnityEngine;
using UnityEngine.UI;

namespace BomberOf2048.UI.Widgets.ProgressBar
{
    public class BarWithPointerWidget : ProgressBarWidget
    {
        [SerializeField] private Image _pointer;
        [SerializeField] private Vector2 _startPosPointer;
        [SerializeField] private Vector2 _endPosPointer;

        public override void SetProgress(float progress)
        {
            base.SetProgress(progress);
            
            var newPosX = Mathf.Lerp(_startPosPointer.x, _endPosPointer.x, progress);
            var newPosY = Mathf.Lerp(_startPosPointer.y, _endPosPointer.y, progress);
            _pointer.rectTransform.anchoredPosition = new Vector2(newPosX, newPosY);
        }
    }
}