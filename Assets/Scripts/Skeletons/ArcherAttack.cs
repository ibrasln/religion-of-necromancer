using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ArcherAttack : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float launchForceX, launchForceY;

    public float attackTimer = 3f;

    bool isWaveFinished;

    Animator anim;
    Health health;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (isWaveFinished || health.GetHealth() <= 0)
        {
            attackTimer = 5f;
            return;
        }

        if (GameManager.Instance.isWaveEnded && !isWaveFinished)
        {

            Debug.Log("finished");
            anim.SetTrigger("death");
            Destroy(gameObject, 1.5f);
            isWaveFinished = true;
        }


        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            StartCoroutine(ArrowAttack());
            attackTimer = 2f;
        }
    }

    IEnumerator ArrowAttack()
    {
        if (GameManager.Instance.isWaveStarted)
        {
            anim.SetTrigger("isAttacking");
            yield return new WaitForSeconds(.5f);
            SoundEffectController.instance.PlayAudioClip(0);
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
            arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * launchForceX, 0f), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(1f);
    }
}
