using System;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using BomberOf2048.Utils.ObjectPool;
using UnityEngine;

namespace BomberOf2048.Controllers
{
    public class BoomController
    {
        private GameData GameData => Singleton<GameSession>.Instance.Data;
        
        private AnimationController AnimationController => Singleton<GameSession>.Instance.AnimationController;
        private ScoreController ScoreController => Singleton<GameSession>.Instance.ScoreController;
        private FieldViewController FieldViewController => Singleton<GameSession>.Instance.FieldViewController;

        
        private int FieldSize => GameData.FieldSize;

        private readonly GameObject _preMiddleParticles;
        
        public BoomController(GameObject preMiddleParticles)
        {
            _preMiddleParticles = preMiddleParticles;
        }
        
        public void Boom(int x, int y, Action onComplete)
        {
            var value = GameData.GameField[x, y].Value;
            
            var checker = ValueBoom(value, x, y);
            
            if(checker)
                onComplete?.Invoke();
        }

        private bool ValueBoom(int value, int x, int y)
        {
            switch (value)
            {
                case 8:
                    LittleBoom(x, y);
                    SpawnParticles(x, y);
                    break;
                case 9:
                    PreMiddleBoom(x, y);
                    SpawnParticles(x, y);
                    break;
                case 10:
                    MiddleBoom(x, y);
                    SpawnParticles(x, y);
                    break;
                case 11:
                    BigBoom();
                    SpawnParticles(x, y);
                    break;
                default:
                    return false;
                    //break;
            }

            return true;
        }

        public void SpawnParticles(int x, int y)
        {
            var particlePos = new Vector3(FieldViewController.SectionsPos[x, y].x,
                FieldViewController.SectionsPos[x, y].y, 0f);
            Pool.Instance.Get(_preMiddleParticles, particlePos, Vector3.one);
        }

        public bool IsHaveBombsOnField()
        {
            for (var i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    var value = GameData.GameField[i, j].Value;
                    if (value >= 8)
                        return true;
                }
            }

            return false;
        }

        

        private void LittleBoom(int x, int y)
        {
            BoomSection(x, y);
            BoomSection(x - 1, y);
            BoomSection(x + 1, y);
            BoomSection(x, y - 1);
            BoomSection(x, y + 1);
        }
        
        private void PreMiddleBoom(int x, int y)
        {
            
            BoomSection(x - 1, y - 1);
            BoomSection(x - 1, y);
            BoomSection(x - 1 , y + 1);
            
            BoomSection(x, y - 1);
            BoomSection(x, y);
            BoomSection(x, y + 1);

            
            BoomSection(x + 1, y - 1);
            BoomSection(x + 1, y);
            BoomSection(x + 1, y + 1);
        }
        
        private void MiddleBoom(int x, int y)
        {
            for (var i = 0; i < FieldSize; i++)
            {
                BoomSection(x, i);
                BoomSection(i, y);
            }
        }
        
        private void BigBoom()
        {
            for (var i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    BoomSection(i, j);
                }
            }
        }
        

        private void BoomSection(int x, int y)
        {
            if (x >= 0 && x < FieldSize && y >= 0 && y < FieldSize)
            {
                if(GameData.GameField[x, y].Value == 0)
                    return;
                var pos = new int[] {x, y};
                var value = GameData.GameField[x, y].Value;
                
                var score = Mathf.Pow(2, value);
                ScoreController.AddScore((int)score);
                
                GameData.GameField[x, y].Value = 0;
                
                AnimationController.Boom(pos, value);

            }
        }
    }
}