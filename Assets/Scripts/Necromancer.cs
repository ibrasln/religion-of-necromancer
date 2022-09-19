using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : MonoBehaviour
{

    public static Necromancer instance;

    [SerializeField] float movementSpeed = 5f;

    [SerializeField] GameObject archerPrefab, swordmanPrefab;
    [SerializeField] Transform skeletonPositions;

    [SerializeField] GameObject skill;
    [SerializeField] Transform skillPos;
    float skillTimer;

    float dodgeCountdown = 4f;
    float dodgeTimer = 5f;
    bool isDodgeStarted;

    bool isSummoned;
    public bool isFinished;
    [SerializeField] Transform targetPos;

    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D myCollider;
    Health healthScript;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        healthScript = GetComponent<Health>();
    }

    private void Start()
    {
        dodgeTimer = 5f;
    }

    private void Update()
    {
        if (isFinished)
        {
            rb.velocity = Vector2.zero;
            anim.SetFloat("velocity", rb.velocity.x);
            transform.position = Vector2.MoveTowards(transform.position, targetPos.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            if (healthScript.isDead) return;

            Movement();
            Flip();
        
            if (Input.GetKeyDown(KeyCode.Space) && SoulKeeper.instance.GetSoul() != 0 && !GameManager.Instance.isWaveEnded)
                StartCoroutine(SummonSkeletons());

            skillTimer -= Time.deltaTime;
            if (skillTimer <= 0 && Input.GetMouseButtonDown(0))
            {
                skillTimer = 1f;
                anim.SetTrigger("isAttacking");
                GameObject mageSkill = Instantiate(skill, skillPos.position, Quaternion.identity);
                mageSkill.GetComponent<Rigidbody2D>().velocity = new(transform.localScale.x * 10f, 0f);
            }

            dodgeCountdown -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftControl) && dodgeCountdown <= 0)
            {
                isDodgeStarted = true;
                StartCoroutine(Dodge());
            }

            if (isDodgeStarted)
            {
                dodgeTimer -= Time.deltaTime;
                anim.SetFloat("dodgeTimer", dodgeTimer);
                if (dodgeTimer <= 0)
                {
                    myCollider.enabled = true;
                    isDodgeStarted = false;
                    dodgeTimer = 5f;
                    dodgeCountdown = 4f;
                }
            }
        }
    }

    void Movement()
    {
        if (isSummoned || isDodgeStarted) return;
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new(h * movementSpeed, rb.velocity.y);       
        anim.SetFloat("velocity", Mathf.Abs(rb.velocity.x));
    }

    void Flip()
    {
        bool hasPlayerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (hasPlayerHorizontalSpeed) transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    IEnumerator SummonSkeletons()
    {
        anim.SetTrigger("summonSkill");
        isSummoned = true;
        SoundEffectController.instance.PlayAudioClip(8);
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < SoulKeeper.instance.GetSoul(); i++)
        {
            float randNum = Random.Range(0f, 1f);
            if (randNum < .5f) Instantiate(swordmanPrefab, skeletonPositions.GetChild(i).position, Quaternion.identity);
            if (randNum >= .5f) Instantiate(archerPrefab, skeletonPositions.GetChild(i).position, Quaternion.identity);
        }
        SoulKeeper.instance.ResetSoul();
        yield return new WaitForSeconds(.25f);
        isSummoned = false;
    }

    IEnumerator Dodge()
    {
        anim.SetTrigger("dodge");
        yield return new WaitForSeconds(.75f);
        myCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            isFinished = true;
        }
    }

}
