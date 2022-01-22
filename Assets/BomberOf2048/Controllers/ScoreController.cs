using System;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using UnityEngine;

namespace BomberOf2048.Controllers
{
    public class ScoreController : IDisposable
    {
        private GameData GameData => Singleton<GameSession>.Instance.Data;

        public const float FirstLevelScore = 600f;
        public const float NextLevelModifier = 1.1f;

        public ScoreController()
        {
            
        }

        public void AddScore(int score)
        {
            GameData.CurrentScore.Value += score;
            GameData.LevelScore.Value += score;

            if (GameData.CurrentScore.Value > GameData.HighScore.Value)
            {
                GameData.HighScore.Value = GameData.CurrentScore.Value;
            }

            var scoreForNextLevel = FirstLevelScore * Mathf.Pow(NextLevelModifier, GameData.Level.Value - 1);

            if (GameData.LevelScore.Value >= scoreForNextLevel)
            {
                GameData.Level.Value++;
                var newLevelScore = Mathf.Abs(GameData.LevelScore.Value - scoreForNextLevel);
                GameData.LevelScore.Value = (int)newLevelScore;
                
                Debug.Log("Level Up!");
            }

            GameData.LevelProgress.Value = GameData.LevelScore.Value / scoreForNextLevel;
        }
        
        
        public void Dispose()
        {
        }
    }
}