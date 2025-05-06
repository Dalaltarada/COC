using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Damagable))]
public class EnemyRobot : MonoBehaviour
{
    public Transform player;

    [SerializeField] private float detectRange = 15f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackDamage = 0.2f;
    [SerializeField] private float attackCooldown = 2f;

    private Animator animator;
    private Rigidbody rb;
    private bool isAttacking = false;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        transform.LookAt(player);

        if (distance <= attackRange)
        {
            animator.SetBool("isRunning", false);
            if (!isAttacking)
                StartCoroutine(PerformAttack());
        }
        else if (distance <= detectRange)
        {
            animator.SetBool("isRunning", true);
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0;
        direction.Normalize();

        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("attack");
        animator.SetBool("isRunning", false);

        yield return new WaitForSeconds(0.5f); // wait for animation to reach hit point

        // 👇 Enhanced range using OverlapSphere
        Vector3 hitCenter = transform.position + transform.forward * 1.2f;
        float hitRadius = 1.5f;

        Collider[] hits = Physics.OverlapSphere(hitCenter, hitRadius);
        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue; // skip self

            var damagable = hit.GetComponent<Damagable>();
            if (damagable)
            {
                damagable.takeDamage(attackDamage);
                break; // only damage one target
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void OnDeath()
    {
        isDead = true;
        animator.SetTrigger("die");
        Destroy(gameObject, 3);
    }

    // Optional: visualize hit radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 hitCenter = transform.position + transform.forward * 1.2f;
        Gizmos.DrawWireSphere(hitCenter, 1.5f);
    }
}
