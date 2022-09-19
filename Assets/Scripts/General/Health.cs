using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject skullHeadPrefab;
    public int health;
    public bool isPlayer;
    public bool isVillager;

    public bool isDead;

    Animator anim;
    Collider2D myCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    public void TakeDamage()
    {
        anim.SetTrigger("hurt");
        health--;

        if (isPlayer)
        {
            SoundEffectController.instance.PlayAudioClip(9);
        }
        else if (isVillager)
        {
            SoundEffectController.instance.PlayAudioClip(3);
        }
        else
        {
            SoundEffectController.instance.PlayAudioClip(6);
        }

        if(health <= 0 && !isDead)
        {
            if (isVillager)
            {
                SoundEffectController.instance.PlayAudioClip(4);
            }
            else if (isPlayer)
            {
                SoundEffectController.instance.PlayAudioClip(5);
            }
            else
            {
                SoundEffectController.instance.PlayAudioClip(6);
            }

            StartCoroutine(Die());
            isDead = true;
        }
    }

    IEnumerator Die()
    {
        myCollider.enabled = false;
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("death");
        yield return new WaitForSeconds(1.5f);
        if (isVillager)
        {
            Instantiate(skullHeadPrefab, new Vector2(transform.position.x, transform.position.y - .5f), Quaternion.identity);
            SoulKeeper.instance.IncreaseSoul();
        }
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return health;
    }

}
