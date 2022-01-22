using System;
using BomberOf2048.Model;
using BomberOf2048.UI.Widgets.ProgressBar;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace BomberOf2048.UI.Widgets
{
    public class LevelWidget : MonoBehaviour
    {
        [SerializeField] private Text _level;
        [SerializeField] private BarWithPointerWidget _progressBar;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _trash.Retain(Singleton<GameSession>.Instance.Data.Level.SubscribeAndInvoke(SetLevel));
            _trash.Retain(Singleton<GameSession>.Instance.Data.LevelProgress.SubscribeAndInvoke(SetProgress));
        }

        private void SetProgress(float newvalue, float oldvalue)
        {
            _progressBar.SetProgress(newvalue);
        }

        private void SetLevel(int newvalue, int oldvalue)
        {
            _level.text = (newvalue + 1).ToString();
        }
    }
}