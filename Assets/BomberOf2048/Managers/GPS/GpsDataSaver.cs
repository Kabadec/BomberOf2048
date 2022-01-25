using System;
using System.Collections;
using System.Globalization;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace BomberOf2048.Managers.GPS
{
    public class GpsDataSaver : MonoBehaviour
    {
        private bool _isSaving = false;

        private GameData Data => Singleton<GameSession>.Instance.Data;


        private Coroutine _newGameCoroutine;

        private Coroutine _autoSavingCoroutine;

        public void Initialize()
        {
            if (Data.IsNewGame.Value)
            {
                _newGameCoroutine = StartCoroutine(NewGameCoroutine());
            }
            else
            {
                _autoSavingCoroutine = StartCoroutine(AutoSavingCoroutine());
            }
        }
        
        private IEnumerator NewGameCoroutine()
        {
            OpenSave(false);
            yield return new WaitForSeconds(60f);
            _newGameCoroutine = StartCoroutine(NewGameCoroutine());
        }
        
        private IEnumerator AutoSavingCoroutine()
        {
            yield return new WaitForSeconds(300f);
            OpenSave(true);
            _autoSavingCoroutine = StartCoroutine(AutoSavingCoroutine());
        }
        

        public void OpenSave(bool saving)
        {
            Debug.Log("Open Save");
            if (Social.localUser.authenticated)
            {
                _isSaving = saving;
                ((PlayGamesPlatform)Social.Active).SavedGame
                    .OpenWithAutomaticConflictResolution(
                        "BomberOf2048",
                        GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork, 
                        ConflictResolutionStrategy.UseLongestPlaytime, SaveGameOpened);
            }
        }
        
        private void SaveGameOpened(SavedGameRequestStatus status, ISavedGameMetadata meta)
        {
            Debug.Log("SaveGameOpened");
            
            if (status == SavedGameRequestStatus.Success)
            {
                if (_isSaving) // Writting
                {
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(GetSaveString());

                    SavedGameMetadataUpdate update =
                        new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Saved at " +
                            DateTime.Now.ToString(CultureInfo.InvariantCulture)).Build();
                    
                    ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, SaveUpdate);
                }
                else //Reading
                {
                    ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, SaveRead);
                }
            }
        }
        
        // Load
        private void SaveRead(SavedGameRequestStatus status, byte[] data)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                var saveData = System.Text.Encoding.ASCII.GetString(data);
                Debug.Log(saveData);
                
                LoadSaveString(saveData);
                
                if (Data.IsNewGame.Value == true)
                {
                    Data.IsNewGame.Value = false;
                    
                    if (_newGameCoroutine != null)
                        StopCoroutine(_newGameCoroutine);
                    _newGameCoroutine = null;
                    
                    _autoSavingCoroutine = StartCoroutine(AutoSavingCoroutine());
                }
            }
        }

        // Success save
        private void SaveUpdate(SavedGameRequestStatus status, ISavedGameMetadata data)
        {
            Debug.Log(status);
        }
        
        private string GetSaveString()
        {
            var r = "";
            r += Data.Level.Value.ToString();
            r += "|";
            r += Data.LevelScore.Value.ToString();
            r += "|";
            r += Data.HighScore.Value.ToString();
            return r;
            // 35|659|12864
        }

        private void LoadSaveString(string save)
        {
            if (save == "")
            {
                OpenSave(true);
                return;
            }
            // 35|659|12864
            var data = save.Split('|');
            // data[0] = 35, data[1] = 659, data[2] = 12864;
            Debug.Log($"LoadSaveString, save: {save}");
            Debug.Log(data[0]);
            Debug.Log(data[1]);
            Debug.Log(data[2]);
            
                
            
            var level = int.Parse(data[0]);
            var levelScore = int.Parse(data[1]);
            var highScore = int.Parse(data[2]);


            if (Data.Level.Value > level || Data.HighScore.Value > highScore)
            {
                if (Data.Level.Value < level)
                {
                    Data.Level.Value = level;
                    Data.LevelScore.Value = levelScore;
                }
                if (Data.HighScore.Value < highScore)
                    Data.HighScore.Value = highScore;

                OpenSave(true);
                Debug.Log("Value on phone bolshe chem in save, start saving");
            }
            else
            {
                Data.Level.Value = level;
                Data.LevelScore.Value = levelScore;
                Data.HighScore.Value = highScore;
                Debug.Log("Restoring values in save");
            }
            Singleton<GameSession>.Instance.ScoreController.AddScore(0);
        }
        
        public void ShowSelectSaveUI() {
            const uint maxNumToDisplay = 5;
            const bool allowCreateNew = false;
            const bool allowDelete = true;

            var savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                OnSavedGameSelected);
        }

        private void OnSavedGameSelected (SelectUIStatus status, ISavedGameMetadata game) {
            if (status == SelectUIStatus.SavedGameSelected) {
                // handle selected game save
            } else {
                // handle cancel or error
            }
        }
    }
}