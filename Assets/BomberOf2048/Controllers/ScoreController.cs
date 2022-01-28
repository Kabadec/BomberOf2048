using System;
using BomberOf2048.Components;
using BomberOf2048.Managers;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using UnityEngine;

namespace BomberOf2048.Controllers
{
    public class ScoreController : IDisposable
    {
        private readonly float _firstLevelScore;
        private readonly float _nextLevelModifier;

        private const float AnalyticsDelaySendHighScore = 30f;
        private float _timeAnalyticsSendHighScore = 0f;
        
        private readonly LevelUpParticlesComponent _levelUpParticles;
        private GameData GameData => Singleton<GameSession>.Instance.Data;

        public ScoreController(float firstLevelScore, float nextLevelModifier, LevelUpParticlesComponent levelUpParticles)
        {
            _levelUpParticles = levelUpParticles;
            _firstLevelScore = firstLevelScore;
            _nextLevelModifier = nextLevelModifier;
        }

        public void AddScore(int score)
        {
            GameData.CurrentScore.Value += score;
            GameData.LevelScore.Value += score;

            if (GameData.CurrentScore.Value > GameData.HighScore.Value)
            {
                GameData.HighScore.Value = GameData.CurrentScore.Value;
                if (_timeAnalyticsSendHighScore < Time.time)
                {
                    _timeAnalyticsSendHighScore = Time.time + AnalyticsDelaySendHighScore;
                    Singleton<FirebaseAnalyticsManager>.Instance.AnalyticsHighScore(GameData.HighScore.Value);
                }
            }

            var scoreForNextLevel = _firstLevelScore * Mathf.Pow(_nextLevelModifier, GameData.Level.Value - 1);

            for (int i = 0; i < 100; i++)
            {
                if (GameData.LevelScore.Value >= scoreForNextLevel)
                {
                    GameData.Level.Value++;
                    var newLevelScore = Mathf.Abs(GameData.LevelScore.Value - scoreForNextLevel);
                    GameData.LevelScore.Value = (int)newLevelScore;
                
                    Debug.Log("Level Up!");
                    _levelUpParticles.SpawnLevelUpParticles();
                    Singleton<FirebaseAnalyticsManager>.Instance.AnalyticsLevelUp(GameData.Level.Value);
                }
                else
                {
                    break;
                }
                scoreForNextLevel = _firstLevelScore * Mathf.Pow(_nextLevelModifier, GameData.Level.Value - 1);
            }
            
            GameData.LevelProgress.Value = GameData.LevelScore.Value / scoreForNextLevel;
        }
        
        public void Dispose()
        {
        }
    }
}