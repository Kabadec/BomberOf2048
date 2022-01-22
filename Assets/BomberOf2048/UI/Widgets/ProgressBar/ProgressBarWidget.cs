using UnityEngine;
using UnityEngine.UI;

namespace BomberOf2048.UI.Widgets.ProgressBar
{
    public class ProgressBarWidget : MonoBehaviour
    {
        [SerializeField] protected Image _bar;

        public virtual void SetProgress(float progress)
        {
            _bar.fillAmount = progress;
        }
    }
}