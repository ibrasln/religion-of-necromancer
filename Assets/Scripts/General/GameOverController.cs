using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] GameObject credits;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1f);
        credits.SetActive(true);
        yield return new WaitForSeconds(20f);
        credits.GetComponent<CanvasGroup>().DOFade(0f, 3f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

}
