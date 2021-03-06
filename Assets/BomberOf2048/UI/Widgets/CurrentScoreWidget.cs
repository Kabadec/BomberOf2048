using BomberOf2048.Model;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace BomberOf2048.UI.Widgets
{
    public class CurrentScoreWidget : MonoBehaviour
    {
        [SerializeField] private Text _score;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private void Start()
        {
            _trash.Retain(Singleton<GameSession>.Instance.Data.CurrentScore.SubscribeAndInvoke(SetScore));
        }

        private void SetScore(int newValue, int oldValue)
        {
            _score.text = newValue.ToString();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}