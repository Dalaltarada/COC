/*
    Written By Olusola Olaoye

    To only be used by those who purchased from the Unity asset store
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AutomaticGun : Gun
{
    [SerializeField] private GameObject shoot_particle;

    [SerializeField, Range(10, 20)]
    private float shot_power = 10; // the force that will be applied to rigidbodies shot by this gun

    [SerializeField] private float recoil_amplitude = 0.1f;
    [SerializeField] private float recoil_frequency = 30;

    [Header("Audio")]
    [SerializeField] private AudioClip shootSound; // 🔊 Shooting sound
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void shoot()
    {
        if (current_bullet > 0 && gameObject.activeSelf && is_in_players_pocket && PlayerInput.instance.mouse_left_click)
        {
            shoot_particle.SetActive(true);
            current_bullet -= 1;

            // 🔊 Play shooting sound
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            RaycastHit hit;

            if (Physics.Raycast(shoot_start_position.position, shoot_start_position.forward, out hit))
            {
                Damagable damagable = hit.collider.GetComponent<Damagable>();
                Rigidbody rbody = hit.collider.GetComponent<Rigidbody>();

                if (damagable)
                {
                    damagable.takeDamage((int)gun_damage);
                }

                if (rbody)
                {
                    rbody.AddForce((hit.point - shoot_start_position.position).normalized * shot_power);
                }
            }
        }
        else
        {
            shoot_particle.SetActive(false);
        }
    }
}
