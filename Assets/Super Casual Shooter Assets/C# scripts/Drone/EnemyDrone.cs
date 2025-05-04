/*
    Written By Olusola Olaoye

    To only be used by those who purchased from the Unity asset store
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Damagable))]
[RequireComponent(typeof(LineRenderer))]
public class EnemyDrone : MonoBehaviour
{
    [SerializeField]
    [Range(1, 6)]
    private int attack_time;

    [SerializeField]
    [Range(1, 4)]
    private int cool_down_time;

    private Rigidbody rigid_body;
    private LineRenderer shoot_line;

    public Transform player { get; set; }

    [SerializeField]
    [Range(0, 1)]
    private float attack_value = 0.1f;

    private float action_counter;
    private bool is_dead = false; // ✅ Initialized here

    private void Start()
    {
        shoot_line = GetComponent<LineRenderer>();
        rigid_body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.LookAt(player);

        if (!is_dead)
        {
            if (action_counter >= attack_time)
            {
                rigid_body.AddForce(Vector3.one * UnityEngine.Random.Range(0, 1));
                shoot_line.enabled = false;
                StartCoroutine(coolDown());
            }
            else
            {
                action_counter += Time.deltaTime;
                rigid_body.AddForce((player.transform.position - transform.position).normalized);
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
        Damagable damagable;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 200))
        {
            damagable = hit.collider.gameObject.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.takeDamage(attack_value);

                Vector3[] shoot_line_points = new Vector3[]
                {
                    transform.position, hit.point
                };

                shoot_line.enabled = true;
                shoot_line.SetPositions(shoot_line_points);
            }
        }
    }

    public void onDeath()
    {
        if (is_dead) return; // ✅ Prevent multiple calls
        is_dead = true;

        shoot_line.enabled = false;
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
