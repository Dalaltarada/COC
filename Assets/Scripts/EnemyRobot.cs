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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    private Animator animator;
    private Rigidbody rb;
    private bool isAttacking = false;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Damagable dmg = GetComponent<Damagable>();
        if (dmg != null)
        {
            dmg.OnDeath.AddListener(onDeath);
        }

        RobotManager.Instance?.RegisterRobot(); // 👈 Register self
    }

    private void Update()
    {
        if (isDead || player == null) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("🔪 Force kill key pressed!");
            onDeath();
            return;
        }

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

        yield return new WaitForSeconds(0.5f);

        Vector3 hitCenter = transform.position + transform.forward * 1.2f;
        float hitRadius = 1.5f;

        Collider[] hits = Physics.OverlapSphere(hitCenter, hitRadius);
        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;

            var damagable = hit.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.takeDamage(attackDamage);

                if (audioSource != null && hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }

                break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void onDeath()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("💀 Robot died!");

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        animator.SetTrigger("die");
        animator.SetBool("isRunning", false);
        animator.ResetTrigger("attack");

        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        ScoreManager.Instance?.AddPoints(70);

        RobotManager.Instance?.RobotDied(); //  Notify manager

        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 hitCenter = transform.position + transform.forward * 1.2f;
        Gizmos.DrawWireSphere(hitCenter, 1.5f);
    }
}
