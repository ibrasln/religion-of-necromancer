using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float nextToPlayer;
    GameObject player;

    Rigidbody2D playerRB;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (GameManager.Instance.isWaveEnded)
        {
            Flip();
            Vector2 targetPos = new(player.transform.position.x + nextToPlayer, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);

            if (transform.position.x == targetPos.x && Mathf.Abs(playerRB.velocity.x) <= 0.1)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }
        }
    }

    void Flip()
    {
        if (playerRB.velocity.x <= -0.1f)
        {
            transform.localScale = new(-1, 1, 1);
        }
        else if (playerRB.velocity.x >= 0.1f)
        {
            transform.localScale = Vector3.one;
        }
    }


}
