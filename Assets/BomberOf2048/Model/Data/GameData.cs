using System;
using BomberOf2048.Model.Data.Properties;
using UnityEngine;

namespace BomberOf2048.Model.Data
{
    [Serializable]
    public class GameData
    {
        [Header("Field size")] 
        [SerializeField] private int _fieldSize = 4;


        public int FieldSize => _fieldSize;
        public BoolPersistentProperty IsNewGame { get; private set; }
        public BoolPersistentProperty IsPlayerSeeTutorial { get; private set; }
        public IntPersistentProperty Level { get; private set; }
        public IntPersistentProperty LevelScore { get; private set; }
        public IntPersistentProperty CurrentScore { get; private set; }
        public IntPersistentProperty HighScore { get; private set; }
        public IntPersistentProperty[,] GameField { get; private set; }
        
        public FloatProperty LevelProgress { get; private set; }


        public void Initialize()
        {
            IsNewGame = new BoolPersistentProperty(true, GameDataProperties.IsNewGame.ToString());
            IsPlayerSeeTutorial = new BoolPersistentProperty(false, GameDataProperties.IsPlayerSeeTutorial.ToString());
            Level = new IntPersistentProperty(1, GameDataProperties.Level.ToString());
            LevelScore = new IntPersistentProperty(0, GameDataProperties.LevelScore.ToString());
            CurrentScore = new IntPersistentProperty(0, GameDataProperties.CurrentScore.ToString());
            HighScore = new IntPersistentProperty(0, GameDataProperties.HighScore.ToString());
            LevelProgress = new FloatProperty();
            LevelProgress.Value = 0f;
            
            GameField = new IntPersistentProperty[_fieldSize,_fieldSize];
            for (int i = 0; i < _fieldSize; i++)
            {
                for (int j = 0; j < _fieldSize; j++)
                {
                    GameField[i, j] = new IntPersistentProperty(0,
                        GameDataProperties.GameField.ToString() + i.ToString() + j.ToString());
                }
            }
        }
    }

    public enum GameDataProperties
    {
        IsNewGame,
        IsPlayerSeeTutorial,
        Level,
        LevelScore,
        CurrentScore,
        HighScore,
        GameField
    }
}
