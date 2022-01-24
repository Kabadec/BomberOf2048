using System;
using BomberOf2048.Utils.ObjectPool;
using UnityEngine;

namespace BomberOf2048.Components
{
    [RequireComponent(typeof(PoolItem))]
    public class BoomParticlesComponent : MonoBehaviour
    {
        [SerializeField] private float _durationTime = 5;

        private ParticleSystem[] _particles;
        private float _startTime;

        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }

        private void Start()
        {
            Restart();
        }
        
        public void Restart()
        {
            _startTime = Time.time;
            foreach (var particle in _particles)
            {
                particle.Emit(new ParticleSystem.EmitParams(), 30);
            }
        }

        private void Update()
        {
            if (Time.time - _startTime > _durationTime)
            {
                GetComponent<PoolItem>().Release();
            }
        }
    }
}