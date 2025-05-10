using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Damagable))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class EnemyDrone : MonoBehaviour
{
    [SerializeField, Range(1, 6)] private int attack_time;
    [SerializeField, Range(1, 4)] private int cool_down_time;
    [SerializeField, Range(0, 1)] private float attack_value = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;

    private Rigidbody rigid_body;
    private LineRenderer shoot_line;
    private AudioSource audioSource;

    public Transform player { get; set; }

    private float action_counter;
    private bool is_dead = false;

    private void Start()
    {
        shoot_line = GetComponent<LineRenderer>();
        rigid_body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.LookAt(player);

        if (!is_dead)
        {
            if (action_counter >= attack_time)
            {
                rigid_body.AddForce(Vector3.one * Random.Range(0, 1));
                shoot_line.enabled = false;
                StartCoroutine(coolDown());
            }
            else
            {
                action_counter += Time.deltaTime;
                rigid_body.AddForce((player.position - transform.position).normalized);
                attack();
            }
        }
    }

    private IEnumerator coolDown()
    {
        yield return new WaitForSeconds(cool_down_time);
        action_counter = 0;
    }

    private void attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 200))
        {
            Damagable damagable = hit.collider.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.takeDamage(attack_value);

                Vector3[] shoot_line_points = new Vector3[]
                {
                    transform.position, hit.point
                };

                shoot_line.enabled = true;
                shoot_line.SetPositions(shoot_line_points);

                // 🔊 Play attack sound
                if (audioSource != null && attackSound != null)
                {
                    audioSource.PlayOneShot(attackSound);
                }
            }
        }
    }

    public void onDeath()
    {
        if (is_dead) return;
        is_dead = true;

        shoot_line.enabled = false;

        // 🔊 Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        ScoreManager.Instance?.AddPoints(50);

        Destroy(gameObject, 3);

        foreach (Transform children_transforms in gameObject.GetComponentsInChildren<Transform>())
        {
            children_transforms.transform.SetParent(null);
            children_transforms.gameObject.AddComponent<MeshCollider>().convex = true;

            if (!children_transforms.GetComponent<Rigidbody>())
            {
                children_transforms.gameObject.AddComponent<Rigidbody>();
            }

            Destroy(children_transforms.gameObject, 2);
        }
    }
}
