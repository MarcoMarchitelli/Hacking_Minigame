namespace Rewind
{
    using UnityEngine;

    public class RewindParticleController : MonoBehaviour
    {
        private ParticleSystem particle;
        private ParticleSystem.VelocityOverLifetimeModule velocityModule;
        private bool rewind;
        private float defaultSpeed;

        void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            velocityModule = particle.velocityOverLifetime;
            defaultSpeed = velocityModule.yMultiplier;
        }

        private void Start()
        {
            RewindManager.Instance.OnRewindStart += HandleRewindStart;
            RewindManager.Instance.OnRewindEnd += HandleRewindEnd;
        }

        private void Update()
        {
            if (rewind)
            {
                velocityModule.yMultiplier = Mathf.Lerp(0, defaultSpeed, RewindManager.REWIND_SPEED) * RewindManager.RewindDirection;
            }
        }

        private void HandleRewindStart()
        {
            rewind = true;
        }

        private void HandleRewindEnd()
        {
            rewind = false;
            velocityModule.yMultiplier = defaultSpeed;
        }
    } 
}