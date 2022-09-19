using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField] Transform buttons;
    [SerializeField] CanvasGroup gameName, fadeScreen;
    [SerializeField] Button volumeOn, volumeOff; 
    [SerializeField] Transform howToPlayPanel, optionsPanel, creditsPanel;
    [SerializeField] AudioSource menuMusic;

    bool isTextScenePlayed;

    private void Start()
    {
        StartCoroutine(OpenMenuElements());
    }

    IEnumerator OpenMenuElements()
    {
        fadeScreen.DOFade(0f, 1.5f);
        menuMusic.PlayDelayed(1.5f);
        yield return new WaitForSeconds(1.5f);
        // Add game music
        gameName.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(1.25f);
        for (int i = 0; i < buttons.childCount; i++)
        {
            buttons.GetChild(i).DOScale(1f, .5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(.15f);
        }
        fadeScreen.blocksRaycasts = false;
    }

    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        SoundEffectController.instance.PlayAudioClip(10);
        fadeScreen.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(3f);
        
        if (!isTextScenePlayed)
        {
            SceneManager.LoadScene(1);
            isTextScenePlayed = true;
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void HowToPlay()
    {
        PanelOpening(howToPlayPanel);
    }

    public void Options()
    {
        PanelOpening(optionsPanel);
    }

    public void Credits()
    {
        PanelOpening(creditsPanel);
    }

    public void Quit()
    {
        SoundEffectController.instance.PlayAudioClip(10);
        Application.Quit();
    }

    public void VolumeOn()
    {
        SoundEffectController.instance.PlayAudioClip(10);
        menuMusic.volume = 1;
        volumeOff.gameObject.SetActive(true);
        volumeOn.gameObject.SetActive(false);
    }

    public void VolumeOff()
    {
        SoundEffectController.instance.PlayAudioClip(10);
        menuMusic.volume = 0;
        volumeOn.gameObject.SetActive(true);
        volumeOff.gameObject.SetActive(false);

    }

    public void Back(Transform panel)
    {
        SoundEffectController.instance.PlayAudioClip(10);
        panel.DOScale(0f, .75f);
        fadeScreen.DOFade(0f, .75f);
        fadeScreen.blocksRaycasts = false;
    }

    public void PanelOpening(Transform panel)
    {
        SoundEffectController.instance.PlayAudioClip(10);
        fadeScreen.DOFade(.5f, .75f);
        fadeScreen.blocksRaycasts = true;
        panel.DOScale(1f, .75f);
    }

}
