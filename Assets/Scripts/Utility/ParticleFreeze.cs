using UnityEngine;

public class ParticleFreeze : MonoBehaviour
{
    [SerializeField] float time = .09f;

    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        particle.Simulate(time, true);
        particle.Pause();
    }
}