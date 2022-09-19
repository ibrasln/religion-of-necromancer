using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] Transform gameOverPanel, pausePanel;
    [SerializeField] CanvasGroup fadeScreen;
    [SerializeField] Button volumeOn, volumeOff;
    [SerializeField] AudioSource gameMusic;
    [SerializeField] Health playerHealth;
    [SerializeField] Image heart1, heart2, heart3;
    [SerializeField] Sprite filledHeart, halfHeart, emptyHeart;
    [SerializeField] TMP_Text soulText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        fadeScreen.DOFade(0f, 1.5f);
        gameMusic.PlayDelayed(2f);
        InvokeRepeating("CheckSomeStuff", 5f, 2f);
    }

    private void Update()
    {
        UpdateHealth();

        soulText.text = "x" + SoulKeeper.instance.GetSoul();

    }

    public void CheckSomeStuff()
    {
        if (playerHealth.isDead)
        {
            StartCoroutine(GameOver());
        }
        else if (Necromancer.instance.isFinished) StartCoroutine(GameWon());

    }

    public void VolumeOn()
    {
        gameMusic.volume = 1;
        volumeOff.gameObject.SetActive(true);
        volumeOn.gameObject.SetActive(false);
    }

    public void VolumeOff()
    {
        gameMusic.volume = 0;
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);

    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(2.5f);
        fadeScreen.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.75f);
        fadeScreen.blocksRaycasts = true;
        gameOverPanel.DOScale(1f, 1f);
        fadeScreen.DOFade(.5f, 1f);
    }

    public void Pause()
    {
        StartCoroutine(PauseRoutine());
        Time.timeScale = 0f;
    }

    IEnumerator PauseRoutine()
    {
        //SoundEffectController.instance.SoundEffect(0);
        float scale = 0;
        while (scale != 1)
        {
            scale += Time.unscaledDeltaTime;
            scale = Mathf.Clamp01(scale);
            fadeScreen.blocksRaycasts = true;
            pausePanel.localScale = new Vector3(scale, scale, scale);
            fadeScreen.alpha = scale / 2;
            yield return null;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        //SoundEffectController.instance.SoundEffect(0);
        fadeScreen.blocksRaycasts = false;
        pausePanel.DOScale(0f, 1f);
        fadeScreen.DOFade(0f, 1f);
    }

    //public void VolumeOnOff()
    //{
    //    //SoundEffectController.instance.SoundEffect(0);
    //    if (volumeButtonImage.sprite == volumeOn)
    //    {
    //        volumeButtonImage.sprite = volumeOff;
    //        gameMusic.volume = 0f;
    //    }
    //    else if (volumeButtonImage.sprite == volumeOff)
    //    {
    //        volumeButtonImage.sprite = volumeOn;
    //        gameMusic.volume = .5f;
    //    }
    //}

    public void Restart()
    {
        //SoundEffectController.instance.SoundEffect(0);
        fadeScreen.blocksRaycasts = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        //SoundEffectController.instance.SoundEffect(0);
        Time.timeScale = 1;
        StartCoroutine(MenuRoutine());
    }

    IEnumerator MenuRoutine()
    {
        pausePanel.DOScale(0f, .75f);
        fadeScreen.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    void UpdateHealth()
    {
        switch (playerHealth.GetHealth())
        {
            case 6:
                heart1.sprite = filledHeart;
                heart2.sprite = filledHeart;
                heart3.sprite = filledHeart;
                break;
            case 5:
                heart1.sprite = filledHeart;
                heart2.sprite = filledHeart;
                heart3.sprite = halfHeart;
                break;
            case 4:
                heart1.sprite = filledHeart;
                heart2.sprite = filledHeart;
                heart3.sprite = emptyHeart;
                break;
            case 3:
                heart1.sprite = filledHeart;
                heart2.sprite = halfHeart;
                heart3.sprite = emptyHeart;
                break;
            case 2:
                heart1.sprite = filledHeart;
                heart2.sprite = emptyHeart;
                heart3.sprite = emptyHeart;
                break;
            case 1:
                heart1.sprite = halfHeart;
                heart2.sprite = emptyHeart;
                heart3.sprite = emptyHeart;
                break;
            case 0:
                heart1.sprite = emptyHeart;
                heart2.sprite = emptyHeart;
                heart3.sprite = emptyHeart;
                break;
        }
    }
}
