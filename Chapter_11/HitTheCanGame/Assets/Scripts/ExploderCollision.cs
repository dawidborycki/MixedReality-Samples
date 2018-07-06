using UnityEngine;
using System.Linq;

public class ExploderCollision : MonoBehaviour
{
    public GameObject ExplosionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsBallHittingTheExploder(collision))
        {
            if (ExplosionEffect != null)
            {
                var explosion = Instantiate(ExplosionEffect, transform);

                Destroy(explosion, GetExplosionDuration());
            }

            Destroy(collision.gameObject);
        }
    }

    private float GetExplosionDuration()
    {
        // Default value
        var explosionDuration = 0.5f;

        var particleSystem = ExplosionEffect.GetComponentsInChildren<ParticleSystem>().First();

        if (particleSystem != null)
        {
            explosionDuration = particleSystem.main.duration;
        }

        return explosionDuration;
    }

    private bool IsBallHittingTheExploder(Collision collision)
    {
        return collision.collider.CompareTag("Yellow Ball");
    }
}
