using UnityEngine;

public class ShipParticleDamage : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        IDestructable d = other.GetComponent<IDestructable>();

        if (d != null)
            d.Die();
    }
}