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
        get { return GetComponent<LineRenderer>(); }
    }

    protected override void Start()
    {
        base.Start();

        if (shoot_load_prefab != null)
        {
            shoot_load = Instantiate(shoot_load_prefab);
        }
        else
        {
            Debug.LogWarning("⚠ shoot_load_prefab is not assigned.");
        }

        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();

        if (is_in_players_pocket && shoot_load != null)
        {
            shoot_start_position.rotation = transform.parent.rotation;

            GameScreen gameScreen = FindObjectOfType<GameScreen>();
            if (gameScreen != null)
            {
                shoot_load.transform.SetParent(gameScreen.transform, false);
            }
            else
            {
                Debug.LogWarning("⚠ GameScreen not found in scene.");
            }
        }
    }

    protected override void shoot()
    {
        if (gameObject.activeSelf && is_in_players_pocket && PlayerInput.instance.mouse_left_click && current_bullet > 0)
        {
            current_bullet--;

            if (audioSource != null && laserShotSound != null)
            {
                audioSource.PlayOneShot(laserShotSound);
            }

            RaycastHit hit;

            if (Physics.Raycast(shoot_start_position.position, shoot_start_position.forward, out hit))
            {
                var damagable = hit.collider.gameObject.GetComponent<Damagable>();

                if (damagable != null)
                {
                    count_shooting += count_additive * Time.deltaTime;

                    if (count_shooting > damagable.getMaxHealth())
                    {
                        damagable.takeDamage((int)count_shooting);
                    }

                    if (shoot_load != null)
                    {
                        shoot_load.gameObject.SetActive(true);
                        shoot_load.fillAmount = count_shooting / damagable.getMaxHealth();
                    }
                }
                else
                {
                    count_shooting = 0;
                    if (shoot_load != null) shoot_load.gameObject.SetActive(false);
                }

                shoot_line.enabled = true;
                shoot_line.SetPositions(new Vector3[] { shoot_start_position.position, hit.point });
            }
        }
        else
        {
            shoot_line.enabled = false;
            if (shoot_load != null) shoot_load.gameObject.SetActive(false);
        }
    }
}
