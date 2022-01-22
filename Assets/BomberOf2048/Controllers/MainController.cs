using System.Collections.Generic;
using BomberOf2048.Input;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using UnityEngine;

namespace BomberOf2048.Controllers
{
    public class MainController : MonoBehaviour
    {
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private InputManager _inputManager;
        
        private GameData GameData => Singleton<GameSession>.Instance.Data;
        private AnimationController AnimationController => Singleton<GameSession>.Instance.AnimationController;
        
        private int FieldSize => GameData.FieldSize;

        private bool[,] _mergedSections;
        private readonly List<int[]> _emptySections = new List<int[]>();
        
        private Camera _mainCamera;
        private BoomController _boomController;



        private void Start()
        {
            _inputManager = Singleton<InputManager>.Instance;
            _mainCamera = Camera.main;
            _boomController = new BoomController();
            _trash.Retain(_inputManager.SubscribeOnSwipe(OnSwipe));
            _trash.Retain(_inputManager.SubscribeOnTapTouch(OnTap));


            _mergedSections = new bool[FieldSize, FieldSize];
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    _mergedSections[i,j] = false;
                    
                    
                    /////////////////
                    GameData.GameField[i,j].Value = 0;
                    /////////////////
                }
            }
            
            /////////////////
            GameData.CurrentScore.Value = 0;
            GameData.GameField[0,0].Value = 11;
            /////////////////
            
            
            SpawnRandomSections(false);
        }

        private void OnTap(Vector2 position, float time)
        {
            //Debug.Log(position);
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

            //Debug.Log($"Tapped on: ( {x}, {y} ).");
            _boomController.Boom(x, y, () => SpawnRandomSections(true));
            
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
                        GameData.CurrentScore.Value += (int)score;
                        
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

            if(canSpawnNewSections)
                SpawnRandomSections(false);
            else
            {
                if (IsFieldFull())
                {
                    if (!IsFieldHaveSomeMerges())
                    {
                        if (!_boomController.IsHaveBombsOnField())
                        {
                            Debug.LogError("Game over!");
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
                if(GameData.GameField[x, y].Value == 0 || GameData.GameField[x, y].Value == 10)
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
            
            for (var i = 0; i < 2; i++)
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

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }

    
}
