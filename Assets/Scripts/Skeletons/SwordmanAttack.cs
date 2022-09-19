using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanAttack : MonoBehaviour
{

    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] float attackRange = .5f;
    [SerializeField] float attackTimer = 1f;

    [SerializeField] float dodgeTimer = 3f;
    [SerializeField] BoxCollider2D dodgeCollider;

    [SerializeField] float movementSpeed = 4f;
    bool isSummoned;
    [SerializeField] float startTimer = 1.5f;
    [SerializeField] Transform rayPos;

    bool isWaveFinished;

    Animator anim;
    Rigidbody2D rb;
    Health health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
    }
    


    private void Update()
    {
        if (isWaveFinished || health.GetHealth() <= 0)
        {
            anim.SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
            return;
        }
        if (GameManager.Instance.isWaveEnded && !isWaveFinished)
        {
            anim.SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
            anim.SetTrigger("death");
            Destroy(gameObject, 1.5f);
            isWaveFinished = true;
        }

        if (startTimer <= 0) isSummoned = true;
        else startTimer -= Time.deltaTime;

        if (isSummoned) MovementOrAttack();
    }

    void MovementOrAttack()
    {
        Vector2 dir = transform.localScale.x * transform.right;
        RaycastHit2D hit = Physics2D.Raycast(rayPos.position, dir, .5f);
        Debug.DrawRay(rayPos.position, dir, Color.red);

        if (hit.collider == null)
        {
            Movement();
        }
        else if (hit.collider.CompareTag("Villager"))
        {
            anim.SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
            
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                StartCoroutine(Attack());
                attackTimer = 1.25f;
            }

            dodgeTimer -= Time.deltaTime;
            if (Input.GetMouseButtonDown(1) && dodgeTimer <= 0)
            {
                StartCoroutine(Dodge());
                dodgeTimer = 3f;
            }
        }
        else
        {
            Movement();
        }
    }

    void Movement()
    {
        anim.SetBool("isWalking", true);
        rb.velocity = new(movementSpeed * transform.localScale.x, 0f);
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("isAttacking");
        SoundEffectController.instance.PlayAudioClip(1);
        yield return new WaitForSeconds(.5f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            Collider2D enemy = hitEnemies[i];
            enemy.GetComponent<Health>().TakeDamage();
        }
    }

    IEnumerator Dodge()
    {
        anim.SetTrigger("isDodging");
        dodgeCollider.enabled = true;
        yield return new WaitForSeconds(.75f);
        dodgeCollider.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
