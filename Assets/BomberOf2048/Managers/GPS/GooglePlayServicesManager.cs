using System;
using System.Globalization;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace BomberOf2048.Managers.GPS
{
    [RequireComponent(typeof(GpsDataSaver))]
    public class GooglePlayServicesManager : MonoBehaviour
    {
        [SerializeField] private string _leaderBoardId = "CgkIj5XU1I8EEAIQAg";

        private GpsDataSaver _dataSaver;
        private GameData Data => Singleton<GameSession>.Instance.Data;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _dataSaver = GetComponent<GpsDataSaver>();
            
            var config =
                new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            //PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    _dataSaver.Initialize();
                    Singleton<FirebaseAnalyticsManager>.Instance.AnalyticsLogin();
                }
                else
                {
                    
                }
            });
            _trash.Retain(Data.Level.SubscribeAndInvoke(OnCurrentScoreChanged));
        }

        public void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
            Singleton<FirebaseAnalyticsManager>.Instance.AnalyticsViewLeaderboard();
        }
        
        private void OnCurrentScoreChanged(int newValue, int oldValue)
        {
            Social.ReportScore(Data.Level.Value, _leaderBoardId, b => {});
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}