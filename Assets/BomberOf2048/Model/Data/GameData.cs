using System;
using BomberOf2048.Model.Data.Properties;
using BomberOf2048.Utils.Disposables;
using GooglePlayGames;
using UnityEngine;

namespace BomberOf2048.Model.Data
{
    [Serializable]
    public class GameData
    {
        [Header("Field size")] 
        [SerializeField] private int _fieldSize = 4;
        [Space]
        [SerializeField] private bool _isTutorialAcceptDefault;
        [SerializeField] private bool _musicDefault;
        [SerializeField] private bool _vibrationDefault;
        [SerializeField] private int _levelDefault;
        [SerializeField] private int _currentScoreDefault;


        public int FieldSize => _fieldSize;
        public BoolPersistentProperty IsTutorialAccept { get; private set; }
        public BoolPersistentProperty Music { get; private set; }
        public BoolPersistentProperty Vibration { get; private set; }
        public IntPersistentProperty Level { get; private set; }
        public IntPersistentProperty CurrentScore { get; private set; }
        public IntPersistentProperty[,] GameField { get; private set; }

        public const string LeaderBoardId = "CgkIj5XU1I8EEAIQAQ";

        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        
        public void Initialize()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    
                }
                else
                {
                    
                }
            });
            
            IsTutorialAccept = new BoolPersistentProperty(_isTutorialAcceptDefault, GameDataProperties.Tutorial.ToString());
            Music = new BoolPersistentProperty(_musicDefault, GameDataProperties.Music.ToString());
            Vibration = new BoolPersistentProperty(_vibrationDefault, GameDataProperties.Vibration.ToString());
            Level = new IntPersistentProperty(_levelDefault, GameDataProperties.Level.ToString());
            CurrentScore = new IntPersistentProperty(_currentScoreDefault, GameDataProperties.CurrentScore.ToString());

            GameField = new IntPersistentProperty[_fieldSize,_fieldSize];
            for (int i = 0; i < _fieldSize; i++)
            {
                for (int j = 0; j < _fieldSize; j++)
                {
                    GameField[i, j] = new IntPersistentProperty(0,
                        GameDataProperties.GameField.ToString() + i.ToString() + j.ToString());
                }
            }
            
            _trash.Retain(CurrentScore.SubscribeAndInvoke(OnCurrentScoreChanged));   
        }

        private void OnCurrentScoreChanged(int newvalue, int oldvalue)
        {
            Social.ReportScore(CurrentScore.Value, LeaderBoardId, b => {});
        }
    }

    public enum GameDataProperties
    {
        Tutorial,
        Music,
        Vibration,
        Level,
        CurrentScore,
        GameField
    }
}
