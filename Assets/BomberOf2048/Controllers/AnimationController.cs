using System;
using BomberOf2048.Components.Sections;
using BomberOf2048.Model;
using BomberOf2048.Utils;
using BomberOf2048.Utils.ObjectPool;
using BomberOf2048.Widgets;
using UnityEngine;

namespace BomberOf2048.Controllers
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private FieldViewController FieldViewController => Singleton<GameSession>.Instance.FieldViewController;
        public Vector3[,] SectionsPos => FieldViewController.SectionsPos;

        public float SectionSize => FieldViewController.SectionSize;

        public void Move(int[] from, int[] to, int sectionValue)
        {
            AnimationMoveAndMerge(from, to, sectionValue, false);
        }

        public void Merge(int[] from, int[] to, int startSectionValue)
        {
            AnimationMoveAndMerge(from, to, startSectionValue, true);
        }
        
        public void Spawn(int[] pos, int sectionValue, bool haveDelay)
        {
            var item = Pool.Instance.Get(_prefab, Vector3.zero, Vector3.one);
            var animationWidget = item.GetComponent<AnimationSectionComponent>();

            var newPos = SectionsPos[pos[0], pos[1]];
            
            animationWidget.Spawn(newPos, sectionValue, haveDelay, () => FieldViewController.UpdateSection(pos[0], pos[1]));
        }

        public void Boom(int[] pos, int sectionValue)
        {
            var item = Pool.Instance.Get(_prefab, Vector3.zero, Vector3.one);
            var animationWidget = item.GetComponent<AnimationSectionComponent>();
            
            FieldViewController.UpdateSection(pos[0], pos[1]);
            
            var newPos = SectionsPos[pos[0], pos[1]];

            animationWidget.Boom(newPos, sectionValue);

        }

        private void AnimationMoveAndMerge(int[] from, int[] to, int sectionValue, bool isMerge)
        {
            var item = Pool.Instance.Get(_prefab, Vector3.zero, Vector3.one);
            var animationWidget = item.GetComponent<AnimationSectionComponent>();
            
            FieldViewController.UpdateSection(from[0], from[1]);


            var fromPos = SectionsPos[from[0], from[1]];
            var toPos = SectionsPos[to[0], to[1]];
            
            if(isMerge)
                animationWidget.Merge(fromPos, toPos, sectionValue, () => FieldViewController.UpdateSection(to[0], to[1]));
            else
                animationWidget.Move(fromPos, toPos, sectionValue, () => FieldViewController.UpdateSection(to[0], to[1]));

        }
        
        
        
        
    }
}