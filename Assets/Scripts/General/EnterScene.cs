using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterScene : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    public string[] sentences;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1f);
        string sentence = sentences[0];
        StartCoroutine(TypeSentence(sentence));
        yield return new WaitForSeconds(4f);
        sentence = sentences[1];
        StartCoroutine(TypeSentence(sentence));
        yield return new WaitForSeconds(4f);
        sentence = sentences[2];
        StartCoroutine(TypeSentence(sentence));
        yield return new WaitForSeconds(5f);
        sentence = sentences[3];
        StartCoroutine(TypeSentence(sentence));
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(2);
    }

    IEnumerator TypeSentence(string sentence)
    {
        CanvasGroup textCG = _text.GetComponent<CanvasGroup>();
        textCG.alpha = 1;
        _text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _text.text += letter;
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(2f);
        textCG.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
    }
}
