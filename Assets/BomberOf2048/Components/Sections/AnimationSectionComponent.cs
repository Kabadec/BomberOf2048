using System;
using BomberOf2048.Controllers;
using BomberOf2048.Input;
using BomberOf2048.Model;
using BomberOf2048.Model.Definitions;
using BomberOf2048.Utils;
using BomberOf2048.Utils.ObjectPool;
using BomberOf2048.Widgets;
using DG.Tweening;
using UnityEngine;

namespace BomberOf2048.Components.Sections
{
    [RequireComponent(typeof(SectionComponent), typeof(PoolItem))]
    public class AnimationSectionComponent : MonoBehaviour
    {
        [Header("Moving")]
        [SerializeField] private float _moveAnimationTime = 1f;
        [Header("Merging")]
        [SerializeField] private float _mergeAnimationTime = 0.5f;
        [SerializeField] private float _mergeScaleUp = 1.2f;
        [Header("Spawning")] 
        [SerializeField] private float _spawnStartScale = 0;
        [SerializeField] private float _spawnScaleUp = 1.2f;
        [SerializeField] private float _spawnAnimationTime = 1f;
        [Header("Booming")]
        [SerializeField] private float _boomScaleUp = 1.3f;
        [SerializeField] private float _boomTimeScaleUp = 0.1f;
        [SerializeField] private float _boomTimeScaleDown = 0.3f;
        

        private SectionComponent _sectionComponent;
        private PoolItem _poolItem;

        private Lock InputLocker => Singleton<InputManager>.Instance.InputLocker;

        private FieldViewController FieldViewController => Singleton<GameSession>.Instance.FieldViewController;

        private Sequence _sequence;
        private void Start()
        {
            _sectionComponent = GetComponent<SectionComponent>();
            _poolItem = GetComponent<PoolItem>();
            _sectionComponent.SetSize(FieldViewController.SectionSize);
        }

        public void Move(Vector3 from, Vector3 to, int sectionValue, Action onComplete)
        {
            AnimationMoveAndMerge(from, to, sectionValue, onComplete, false);
        }

        public void Merge(Vector3 from, Vector3 to, int sectionValue, Action onComplete)
        {
            AnimationMoveAndMerge(from, to, sectionValue, onComplete, true);
        }

        public void Spawn(Vector3 pos, int sectionValue, bool haveDelay, Action onComplete)
        {
            //InputLocker.Retain(this);
            var def = DefsFacade.I.Sections.Get(sectionValue);
            _sectionComponent = GetComponent<SectionComponent>();
            var sectionWidgetTransform = _sectionComponent.transform;
            _sectionComponent.SetView(def);
            sectionWidgetTransform.position = pos;
            sectionWidgetTransform.localScale = new Vector3(_spawnStartScale, _spawnStartScale, 1f);
            
            _sequence = DOTween.Sequence();
            
            if(haveDelay)
                _sequence.AppendInterval(_boomTimeScaleUp + _boomTimeScaleDown);
            
            _sequence.AppendInterval(_moveAnimationTime / 2);
            _sequence.Append(transform.DOScale(_spawnScaleUp, _spawnAnimationTime / 2));
            _sequence.Append(transform.DOScale(1f, _spawnAnimationTime / 2));
            
            _sequence.AppendCallback(() =>
            {
                onComplete?.Invoke();
                //InputLocker.Release(this);
                DisableSection();
            });
        }
        
        public void Boom(Vector3 pos, int sectionValue)
        {
            InputLocker.Retain(this);
            var def = DefsFacade.I.Sections.Get(sectionValue);
            _sectionComponent = GetComponent<SectionComponent>();
            var sectionWidgetTransform = _sectionComponent.transform;
            _sectionComponent.SetView(def);
            sectionWidgetTransform.position = pos;
            
            _sequence = DOTween.Sequence();

            _sequence.Append(transform.DOScale(_boomScaleUp, _boomTimeScaleUp));
            _sequence.Append(transform.DOScale(0f, _boomTimeScaleDown));
            
            _sequence.AppendCallback(() =>
            {
                InputLocker.Release(this);
                DisableSection();
            });
        }

        private void AnimationMoveAndMerge(Vector3 from, Vector3 to, int sectionValue, Action onComplete, bool isMerge)
        {
            InputLocker.Retain(this);
            var def = DefsFacade.I.Sections.Get(sectionValue);
            _sectionComponent = GetComponent<SectionComponent>();
            //Debug.Log(_sectionWidget);
            _sectionComponent.SetView(def);
            _sectionComponent.transform.position = from;

            _sequence = DOTween.Sequence();

            _sequence.Append(transform.DOMove(to, _moveAnimationTime).SetEase(Ease.InOutQuad));

            _sequence.AppendCallback(() =>
            {
                InputLocker.Release(this);
            });
            
            if (isMerge)
            {
                _sequence.AppendCallback(() =>
                {
                    var newDef = DefsFacade.I.Sections.Get(sectionValue + 1);
                    _sectionComponent.SetView(newDef);
                });
                _sequence.Append(transform.DOScale(_mergeScaleUp, _mergeAnimationTime / 2));
                _sequence.Append(transform.DOScale(1f, _mergeAnimationTime / 2));
            }

            _sequence.AppendCallback(() =>
            {
                onComplete?.Invoke();
                DisableSection();
            });
        }

        private void DisableSection()
        {
            _sequence.Kill();
            _poolItem.Release();
        }

        
    }
}