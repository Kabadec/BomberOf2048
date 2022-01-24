using System;
using System.Collections.Generic;
using BomberOf2048.Input;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BomberOf2048.Controllers
{
    public class MainController : IDisposable
    {
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameData GameData => Singleton<GameSession>.Instance.Data;
        private AnimationController AnimationController => Singleton<GameSession>.Instance.AnimationController;
        private ScoreController ScoreController => Singleton<GameSession>.Instance.ScoreController;
        private BoomController BoomController => Singleton<GameSession>.Instance.BoomController;

        private int FieldSize => GameData.FieldSize;

        private readonly bool[,] _mergedSections;
        private readonly List<int[]> _emptySections = new List<int[]>();
        
        private readonly Camera _mainCamera;
        //private readonly BoomController _boomController;



        public MainController()
        {
            var inputManager = Singleton<InputManager>.Instance;
            _mainCamera = Camera.main;
            //_boomController = new BoomController();
            _trash.Retain(inputManager.SubscribeOnSwipe(OnSwipe));
            _trash.Retain(inputManager.SubscribeOnTapTouch(OnTap));


            _mergedSections = new bool[FieldSize, FieldSize];
            
            /////////////////
            ClearField();
            /////////////////
            
            /////////////////
            GameData.CurrentScore.Value = 0;
            //GameData.HighScore.Value = 0;
            //GameData.Level.Value = 1;
            //GameData.LevelScore.Value = 0;
            
            
            
            GameData.GameField[0,0].Value = 8;
            //GameData.GameField[0,2].Value = 9;
            //GameData.GameField[3,0].Value = 10;
            //GameData.GameField[3,3].Value = 11;
            
            
            
            /////////////////
        }

        

        public void Initialize()
        {
            SpawnRandomSections(false);
        }

        private void OnTap(Vector2 position, float time)
        {
            var x = -1;
            var y = -1;
            
            var screenPos = new Vector3(position.x, position.y, 0f);
            var worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;
            
            var halfSectionSize = AnimationController.SectionSize / 2;
            for (var i = 0; i < FieldSize; i++)
            {
                var columnPos = AnimationController.SectionsPos[i, 0];
                var startX = columnPos.x - halfSectionSize;
                var endX = columnPos.x + halfSectionSize;
                if (worldPos.x >= startX && worldPos.x <= endX)
                {
                    x = i;
                    break;
                }
            }
            if(x == -1)
                return;
            
            for (var j = 0; j < FieldSize; j++)
            {
                var linePos = AnimationController.SectionsPos[x, j];
                var startY = linePos.y - halfSectionSize;
                var endY = linePos.y + halfSectionSize;
                if (worldPos.y >= startY && worldPos.y <= endY)
                {
                    y = j;
                    break;
                }
            }
            
            if(y == -1)
                return;

            BoomController.Boom(x, y, () => SpawnRandomSections(true));
        }


        private void OnSwipe(Vector2 swipeSide)
        {
            var startX = swipeSide.x > 0 || swipeSide.y > 0 ? FieldSize - 2 : 1;
            
            var dir = swipeSide.x != 0 ? (int)swipeSide.x : (int)swipeSide.y;

            var canSpawnNewSections = false;
            
            for (var i = 0; i < FieldSize; i++)
            {
                for (var j = startX; j >= 0 && j < FieldSize; j -= dir)
                {
                    var section = swipeSide.x != 0 ? new[] {j, i} : new[] {i, j};
                    var value = GameData.GameField[section[0], section[1]].Value;
                    
                    if(value == 0)
                        continue;
                    var sectionToMerge = FindSectionToMegre(section, swipeSide);
                    if (sectionToMerge != null)
                    {
                        var startValue = GameData.GameField[section[0], section[1]].Value;
                        GameData.GameField[section[0], section[1]].Value = 0;
                        GameData.GameField[sectionToMerge[0], sectionToMerge[1]].Value++;
                        _mergedSections[sectionToMerge[0], sectionToMerge[1]] = true;

                        var score = Mathf.Pow(2, GameData.GameField[sectionToMerge[0], sectionToMerge[1]].Value);
                        ScoreController.AddScore((int)score);
                        
                        AnimationController.Merge(section, sectionToMerge, startValue);
                        canSpawnNewSections = true;
                        
                        continue;
                    }

                    var emptySection = FindEmptySection(section, swipeSide);
                    if (emptySection != null)
                    {
                        GameData.GameField[emptySection[0], emptySection[1]].Value = GameData.GameField[section[0], section[1]].Value;
                        GameData.GameField[section[0], section[1]].Value = 0;
                        canSpawnNewSections = true;

                        AnimationController.Move(section, emptySection, GameData.GameField[emptySection[0], emptySection[1]].Value);
                    }
                }
            }

            if (canSpawnNewSections)
            {
                SpawnRandomSections(false);
            }
            else
            {
                if (IsFieldFull())
                {
                    if (!IsFieldHaveSomeMerges())
                    {
                        if (!BoomController.IsHaveBombsOnField())
                        {
                            Debug.LogError("Game over!");
                            WindowUtils.CreateWindow("UI/GameOverWindow");
                            ClearField();
                        }
                    }
                }
            }
            
            ResetMergedSections();
        }
        
        private int[] FindSectionToMegre(int[] section, Vector2 swipeSide)
        {
            var startX = section[0] + (int)swipeSide.x;
            var startY = section[1] + (int)swipeSide.y;

            for (int x = startX, y = startY;
                x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
                x += (int) swipeSide.x, y += (int) swipeSide.y)
            {
                if(GameData.GameField[x, y].Value == 0/* || GameData.GameField[x, y].Value == 10*/)
                    continue;

                if (GameData.GameField[x, y].Value == GameData.GameField[section[0], section[1]].Value
                    && !_mergedSections[x, y])
                {
                    return new[] {x, y};
                }
                break;
            }

            return null;
        }

        private int[] FindEmptySection(int[] section, Vector2 swipeSide)
        {
            int[] emptySection = null;
            
            var startX = section[0] + (int)swipeSide.x;
            var startY = section[1] + (int)swipeSide.y;
            
            for (int x = startX, y = startY;
                x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
                x += (int) swipeSide.x, y += (int) swipeSide.y)
            {
                if(GameData.GameField[x, y].Value == 0)
                    emptySection = new []{x, y};
                else
                    break;
            }

            return emptySection;
        }
        
        
        private void SpawnRandomSections(bool haveDelay)
        {
            _emptySections.Clear();
            for (var i = 0; i < FieldSize; i++)
            {
                for (var j = 0; j < FieldSize; j++)
                {
                    if (GameData.GameField[i, j].Value == 0)
                    {
                        _emptySections.Add(new int[]{i, j});
                    }
                }
            }
            
            for (var i = 0; i < 1; i++)
            {
                if (_emptySections.Count == 0)
                    return;
                
                var section = Random.Range(0, _emptySections.Count);
                var x = _emptySections[section][0];
                var y = _emptySections[section][1];
                _emptySections.RemoveAt(section);
                
                var someValue = Random.value;
                
                if (someValue > 0.1)
                    GameData.GameField[x, y].Value = 1;
                else
                    GameData.GameField[x, y].Value = 2;
                
                var value = GameData.GameField[x, y].Value;
                AnimationController.Spawn(new int[]{x, y}, value, haveDelay);
            }
        }

        private void ClearField()
        {
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    _mergedSections[i,j] = false;
                    GameData.GameField[i,j].Value = 0;
                }
            }
        }
        
        private bool IsFieldFull()
        {
            var emptySections = new List<int[]>();
            for (var i = 0; i < FieldSize; i++)
            {
                for (var j = 0; j < FieldSize; j++)
                {
                    if (GameData.GameField[i, j].Value == 0)
                    {
                        emptySections.Add(new int[]{i, j});
                    }
                }
            }

            if (emptySections.Count == 0)
            {
                return true;
            }

            return false;
        }

        private bool IsFieldHaveSomeMerges()
        {
            return IsSideHaveSomeMerges(Vector2.left) 
                   || (IsSideHaveSomeMerges(Vector2.right) 
                   || IsSideHaveSomeMerges(Vector2.up) 
                   || IsSideHaveSomeMerges(Vector2.down));

        }

        private bool IsSideHaveSomeMerges(Vector2 swipeSide)
        {
            var startX = swipeSide.x > 0 || swipeSide.y > 0 ? FieldSize - 2 : 1;
            
            var dir = swipeSide.x != 0 ? (int)swipeSide.x : (int)swipeSide.y;

            
            for (var i = 0; i < FieldSize; i++)
            {
                for (var j = startX; j >= 0 && j < FieldSize; j -= dir)
                {
                    var section = swipeSide.x != 0 ? new[] {j, i} : new[] {i, j};
                    var value = GameData.GameField[section[0], section[1]].Value;
                    
                    if(value == 0)
                        continue;
                    var sectionToMerge = FindSectionToMegre(section, swipeSide);
                    if (sectionToMerge != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ResetMergedSections()
        {
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    _mergedSections[i, j] = false;
                }
            }
        }
        

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}
