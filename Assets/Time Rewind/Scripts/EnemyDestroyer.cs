using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    public LayerMask destructionMask;

    private void OnTriggerEnter(Collider other)
    {
        if(destructionMask == (destructionMask | (1 << other.gameObject.layer)))
        {
            Destroy(other.gameObject);
        }
    }
}