using System;
using BomberOf2048.Utils;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

namespace BomberOf2048.Managers
{
    public class FirebaseAnalyticsManager : Singleton<FirebaseAnalyticsManager>
    {
        DependencyStatus _dependencyStatus = DependencyStatus.UnavailableOther;
        private bool _firebaseInitialized = false;
        
        private void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                _dependencyStatus = task.Result;
                if (_dependencyStatus == DependencyStatus.Available) {
                    InitializeFirebase();
                } else {
                    Debug.LogError(
                        "FIREBASE: Could not resolve all Firebase dependencies: " + _dependencyStatus);
                }
            });
        }
        
        private void InitializeFirebase() {
            Debug.Log("FIREBASE: Enabling data collection.");
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            Debug.Log("FIREBASE: Set user properties.");
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(24, 0, 0));
            _firebaseInitialized = true;

            Singleton<FirebaseAnalyticsManager>.Instance.DisplayAnalyticsInstanceId();
        }

        public void AnalyticsLogin() {
            Debug.Log("FIREBASE: Logging a login event.");
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        }
        
        public void AnalyticsHighScore(int highScore) {
            Debug.Log("FIREBASE: Logging a high score event.");
            FirebaseAnalytics.LogEvent("high_score",
                new Parameter("score", highScore));
        }
        
        public void AnalyticsLevelUp(int level) {
            Debug.Log("FIREBASE: Logging a level up event.");
            FirebaseAnalytics.LogEvent(
                FirebaseAnalytics.EventLevelUp,
                new Parameter(FirebaseAnalytics.ParameterLevel, level)
                );
        }

        public void AnalyticsGameSaved()
        {
            Debug.Log("FIREBASE: Logging a save game event.");
            FirebaseAnalytics.LogEvent("game_saved");
        }

        public void AnalyticsViewTutorial()
        {
            Debug.Log("FIREBASE: Logging a view tutorial event.");
            FirebaseAnalytics.LogEvent(
                FirebaseAnalytics.EventScreenView,
                new Parameter(FirebaseAnalytics.ParameterScreenName, "screen_tutorial"));
        }

        public void AnalyticsViewLeaderboard()
        {
            Debug.Log("FIREBASE: Logging a view leaderboard event.");
            FirebaseAnalytics.LogEvent(
                FirebaseAnalytics.EventScreenView,
                new Parameter(FirebaseAnalytics.ParameterScreenName, "screen_leaderboard"));
        }

        public void AnalyticsViewGameOverScreen()
        {
            Debug.Log("FIREBASE: Logging a game over event.");
            FirebaseAnalytics.LogEvent(
                FirebaseAnalytics.EventScreenView,
                new Parameter(FirebaseAnalytics.ParameterScreenName, "screen_game_over"));
        }

        private void DisplayAnalyticsInstanceId()
        {
            FirebaseAnalytics.GetAnalyticsInstanceIdAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("FIREBASE: App instance ID fetch was canceled.");
                }
                else if (task.IsFaulted)
                {
                    if (task.Exception != null)
                        Debug.Log($"FIREBASE: Encounted an error fetching app instance ID {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log($"FIREBASE: App instance ID: {task.Result}");
                }
            });
        }
    }
}