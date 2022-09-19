using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearVillager : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayers;
    [SerializeField] float attackRange = .5f;
    [SerializeField] float attackTimer = 2f;

    [SerializeField] float movementSpeed = 3f;
    [SerializeField] Transform rayPos;

    Animator anim;
    Rigidbody2D rb;
    Health health;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Necromancer.instance.isFinished)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isFinished", true);
        }
        else
        {
            if (GameManager.Instance.isWaveEnded || health.GetHealth() <= 0)
            {
                anim.SetBool("isWalking", false);
                rb.velocity = Vector2.zero;
                return;
            }

            MovementOrAttack();
        }
    }

    void MovementOrAttack()
    {
        Vector2 dir = transform.localScale.x * transform.right;
        RaycastHit2D hit = Physics2D.Raycast(rayPos.position, dir, 1f);
        Debug.DrawRay(rayPos.position, dir, Color.red);
        
        if (hit.collider == null)
        {
            Movement();
        }
        else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Skeleton"))
        {
            anim.SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                StartCoroutine(Attack());
                attackTimer = 2f;
            }
        }
    }

    void Movement()
    {
        anim.SetBool("isWalking", true);
        rb.velocity = new(transform.localScale.x * movementSpeed, 0f);
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("isAttacking");
        SoundEffectController.instance.PlayAudioClip(2);
        yield return new WaitForSeconds(.5f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            Collider2D enemy = hitEnemies[i];
            enemy.GetComponent<Health>().TakeDamage();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
