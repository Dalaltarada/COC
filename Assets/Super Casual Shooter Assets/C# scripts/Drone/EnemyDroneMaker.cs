using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyDroneMaker : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform drone_position;

    [SerializeField]
    [Range(1, 6)]
    private int number_of_enemy_drones = 2; // default to 2

    [SerializeField]
    protected EnemyDrone[] villain_drone_prefabs;

    private bool hasSpawned = false; // ✅ added

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.gameObject == player.gameObject)
        {
            for (int i = 0; i < number_of_enemy_drones; i++)
            {
                spawnRandomDrone();
            }

            hasSpawned = true; // ✅ prevent further spawns
        }
    }

    private void spawnRandomDrone()
    {
        int rand = UnityEngine.Random.Range(0, villain_drone_prefabs.Length);

        EnemyDrone drone = Instantiate(villain_drone_prefabs[rand], drone_position.position, Quaternion.identity);
        drone.player = player;

        Damagable damagable = drone.GetComponent<Damagable>();
        if (damagable != null)
        {
            damagable.OnDeath.AddListener(drone.onDeath);
        }
    }
}
