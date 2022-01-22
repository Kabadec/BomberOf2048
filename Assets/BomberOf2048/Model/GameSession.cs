using System;
using BomberOf2048.Controllers;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using UnityEngine;

namespace BomberOf2048.Model
{
    [RequireComponent(typeof(MainController), typeof(AnimationController), typeof(FieldViewController))]
    public class GameSession : Singleton<GameSession>
    {
        [SerializeField] private GameData _data;

        
        public GameData Data => _data;
        
        public MainController MainController { get; private set; }
        public FieldViewController FieldViewController { get; private set; }
        public AnimationController AnimationController { get; private set; }

        private void Awake()
        {
            Initialization();
            MainController = GetComponent<MainController>();
            FieldViewController = GetComponent<FieldViewController>();
            AnimationController = GetComponent<AnimationController>();
            
        }

        private void Start()
        {
            FieldViewController.UpdateAllField();
        }

        private void Initialization()
        {
            _data.Initialize();
        }
    }
}