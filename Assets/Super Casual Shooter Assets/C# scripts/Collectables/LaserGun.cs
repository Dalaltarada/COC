using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class LaserGun : Gun
{
    private float count_shooting = 0;

    [SerializeField]
    private Image shoot_load_prefab;
    private Image shoot_load;

    [SerializeField]
    public int count_additive = 10;

    [Header("Laser Sound")]
    [SerializeField] private AudioClip laserShotSound;
    private AudioSource audioSource;

    protected LineRenderer shoot_line
    {
        get
        {
            return gameObject.GetComponent<LineRenderer>();
        }
    }

    protected override void Start()
    {
        base.Start();

        shoot_load = Instantiate(shoot_load_prefab); // instantiate shoot load bar
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();

        if (is_in_players_pocket)
        {
            shoot_start_position.rotation = transform.parent.rotation;
            shoot_load.transform.SetParent(FindObjectOfType<GameScreen>().transform, false);
        }
    }

    protected override void shoot()
    {
        if (gameObject.activeSelf && is_in_players_pocket && PlayerInput.instance.mouse_left_click && current_bullet > 0)
        {
            current_bullet -= 1;

            // 🔊 Play laser sound
            if (audioSource != null && laserShotSound != null)
            {
                audioSource.PlayOneShot(laserShotSound);
            }

            RaycastHit hit;

            if (Physics.Raycast(shoot_start_position.position, shoot_start_position.forward, out hit))
            {
                if (hit.collider.gameObject.GetComponent<Damagable>() != null)
                {
                    count_shooting += count_additive * Time.deltaTime;

                    if (count_shooting > hit.collider.gameObject.GetComponent<Damagable>().getMaxHealth())
                    {
                        hit.collider.gameObject.GetComponent<Damagable>().takeDamage((int)count_shooting);
                    }

                    shoot_load.gameObject.SetActive(true);
                    shoot_load.fillAmount = count_shooting / hit.collider.gameObject.GetComponent<Damagable>().getMaxHealth();
                }
                else
                {
                    count_shooting = 0;
                    shoot_load.gameObject.SetActive(false);
                }

                // 🔴 Update laser line
                shoot_line.enabled = true;
                Vector3[] shoot_poses = new Vector3[]
                {
                    shoot_start_position.position, hit.point
                };

                shoot_line.SetPositions(shoot_poses);
            }
        }
        else
        {
            shoot_line.enabled = false;
            shoot_load.gameObject.SetActive(false);
        }
    }
}
