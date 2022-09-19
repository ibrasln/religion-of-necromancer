using UnityEngine;

public class PrototypeAttack : MonoBehaviour
{
    [Header("ArrowAttack")]
    [SerializeField] Transform arrowPoint;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float launchForce;

    [Header("Melee-Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] float attackRange = .5f;

    Animator animator;
    Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * 5f, rb.velocity.y);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Attack();
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    ArrowAttack();
        //}

    }

    //void Attack()
    //{
    //    animator.SetTrigger("isAttacking");

    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        Debug.Log("Hit!");
    //    }
    //}

    //void ArrowAttack()
    //{
    //    GameObject arrow = Instantiate(arrowPrefab, arrowPoint.position, arrowPoint.rotation);
    //    arrow.GetComponent<Rigidbody2D>().AddForce(transform.right * launchForce, ForceMode2D.Impulse);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    //}

}
