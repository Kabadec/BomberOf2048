using BomberOf2048.Input;
using BomberOf2048.Model;
using BomberOf2048.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BomberOf2048.UI.Windows
{
    public class GameOverWindow : AnimatedWindow
    {
        [SerializeField] private Text _score;
        private Lock InputLocker => Singleton<InputManager>.Instance.InputLocker;
        
        private void Awake()
        {
            _score.text = Singleton<GameSession>.Instance.Data.CurrentScore.Value.ToString();
            InputLocker.Retain(this);
        }
        
        public override void OnCloseAnimationComplete()
        {
            InputLocker.Release(this);
            Singleton<GameSession>.Instance.FieldViewController.UpdateAllField();
            Singleton<GameSession>.Instance.MainController.SpawnStartSections();
            Singleton<GameSession>.Instance.Data.CurrentScore.Value = 0;
            base.OnCloseAnimationComplete();
        }
    }
}