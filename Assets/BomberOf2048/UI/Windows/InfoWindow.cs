using BomberOf2048.Input;
using BomberOf2048.Utils;

namespace BomberOf2048.UI.Windows
{
    public class InfoWindow : AnimatedWindow
    {
        private Lock InputLocker => Singleton<InputManager>.Instance.InputLocker;
        private void Awake()
        {
            InputLocker.Retain(this);
        }
        
        public override void OnCloseAnimationComplete()
        {
            InputLocker.Release(this);
            base.OnCloseAnimationComplete();
        }
    }
}