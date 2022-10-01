using System.Collections.Generic;
using UnityEngine;

namespace BloodFx_master.Assets
{
    [RequireComponent(typeof(ParticleSystem))]
    public class StainEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        List<Vector4> particleData = new List<Vector4>();
        public void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystem.Play();
        }

        private void Update()
        {
            _particleSystem.GetCustomParticleData(particleData, ParticleSystemCustomData.Custom1);
            for (var index = 0; index < particleData.Count; index++)
            {
                if (particleData[index].x == 0)
                {
                    float time = Time.time;
                    particleData[index] = new Vector4(time, 0, 0, 0);
                }
            }
            _particleSystem.SetCustomParticleData(particleData, ParticleSystemCustomData.Custom1);
        }
    }
}