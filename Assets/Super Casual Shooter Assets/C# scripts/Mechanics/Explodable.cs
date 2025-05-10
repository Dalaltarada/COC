using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damagable))]
public class Explodable : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion_particle; // Particle for explosion
    [SerializeField] private AudioClip explosionSound; // 💥 Explosion sound
    [SerializeField] private AudioSource audioSource;  // 🔊 AudioSource to play the sound

    [SerializeField] private int explosion_range_radius = 10; // Radius for affecting objects
    [SerializeField] private int explosion_force = 20;        // Force applied to rigidbodies
    [SerializeField] private int attack_value = 1;            // Damage to damagables

    public void nowExplode()
    {
        // 🔊 Play explosion sound louder
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound, 10.0f); // Increase volume here
        }

        // 💨 Play explosion particle effect
        Instantiate(explosion_particle, transform.position, Quaternion.identity);

        // 💣 Affect nearby rigidbodies and damagables
        foreach (Collider collider in Physics.OverlapSphere(transform.position, explosion_range_radius))
        {
            Rigidbody rigidbody_ = collider.GetComponent<Rigidbody>();
            if (rigidbody_ != null)
            {
                rigidbody_.AddExplosionForce(explosion_force, transform.position, explosion_range_radius);
            }

            Damagable damagable = collider.gameObject.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.takeDamage(attack_value);
            }
        }

        // 🧨 Destroy this object after short delay
        Destroy(gameObject, 0.5f);
    }
}
