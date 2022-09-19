using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherVillager : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform rayPoint;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float launchForceX, launchForceY;
    [SerializeField] float movementSpeed = 2.5f;
    public bool isStarted;
    [SerializeField] float attackTimer = 2f;

    public bool isFounded;

    Animator anim;
    Rigidbody2D rb;
    Health health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        attackTimer = 2f;
    }

    private void Update()
    {

        if (Necromancer.instance.isFinished)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isWalking", false);
            anim.SetBool("isFinished", true);
        }
        else
        {
            if (health.GetHealth() <= 0)
            {
                attackTimer = 5f;
                return;
            }

            if (isFounded)
            {
                anim.SetBool("isWalking", false);
                rb.velocity = Vector2.zero;
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    StartCoroutine(ArrowAttack());
                    attackTimer = 3f;
                }
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(rayPoint.position, transform.localScale.x * transform.right, 10f);
            if (hit.collider == null)
            {
                Movement();
            }
            else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Skeleton"))
            {
                isFounded = true;
            }
            else
            {
                Movement();
            }

        }
    }

    void Movement()
    {
        anim.SetBool("isWalking", true);
        rb.velocity = new(transform.localScale.x * movementSpeed, 0f);
    }

    IEnumerator ArrowAttack()
    {
        anim.SetTrigger("isAttacking");
        yield return new WaitForSeconds(.5f);
        SoundEffectController.instance.PlayAudioClip(0);
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
        arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * launchForceX, 0f), ForceMode2D.Impulse);
    }
}
