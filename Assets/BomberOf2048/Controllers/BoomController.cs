using System;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using UnityEngine;

namespace BomberOf2048.Controllers
{
    public class BoomController
    {
        private GameData GameData => Singleton<GameSession>.Instance.Data;
        
        private AnimationController AnimationController => Singleton<GameSession>.Instance.AnimationController;
        
        private int FieldSize => GameData.FieldSize;
        
        public void Boom(int x, int y, Action onComplete)
        {
            var value = GameData.GameField[x, y].Value;
            var checker = true;
            switch (value)
            {
                case 8:
                    LittleBoom(x, y);
                    break;
                case 9:
                    PreMiddleBoom(x, y);
                    break;
                case 10:
                    MiddleBoom(x, y);
                    break;
                case 11:
                    BigBoom();
                    break;
                default:
                    checker = false;
                    break;
            }
            if(checker)
                onComplete?.Invoke();
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
                GameData.CurrentScore.Value += (int)score;
                
                GameData.GameField[x, y].Value = 0;
                
                AnimationController.Boom(pos, value);

            }
        }
    }
}