using System.Collections;
using UnityEngine;

namespace BomberOf2048.Components
{
    public class LevelUpParticlesComponent : MonoBehaviour
    {
        [SerializeField] private float[] _delays;

        private Coroutine _coroutine;
        private ParticleSystem[] _particles;

        private void Start()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }

        [ContextMenu("Spawn LevelUp Particles")]
        public void SpawnLevelUpParticles()
        {
            if(_coroutine!= null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(SpawnLevelUpParticlesCoroutine());
        }

        private IEnumerator SpawnLevelUpParticlesCoroutine()
        {
            for (var i = 0; i < _particles.Length; i++)
            {
                _particles[i].Emit(new ParticleSystem.EmitParams(), 30);
                yield return new WaitForSeconds(_delays[i]);
            }
        }
    }
}